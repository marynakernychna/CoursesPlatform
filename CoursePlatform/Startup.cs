using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using CoursesPlatform.EntityFramework;
using CoursesPlatform.Helpers;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Users;
using CoursesPlatform.Services;
using System;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using CoursesPlatform.ErrorMiddleware;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Services.Commands;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Utils;

namespace CoursesPlatform
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();

            services.AddDbContext<AppDbContext>(opt =>
                  opt.UseSqlServer(Configuration["DefaultConnection"])
              );

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            string secureKey = "qUF6U9xyx943jk4TnY49nRV6WR2kRhhbwJZjRxG2Y77WnDLnPQ92aHT6jWjw9sfY3YcYsYjPHpSqZvuYd2yDK3z47n";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signingKey,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddHttpClient();

            services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(Configuration["DefaultConnection"], new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));
            services.AddHangfireServer();


            services.AddMvc().AddFluentValidation();

            services.AddTransient<IValidator<AuthenticateRequest>, AuthenticateRequestValidator>();
            services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>();
            services.AddTransient<IValidator<EmailConfirmationRequest>, EmailConfirmationRequestValidator>();

            services.AddTransient<IValidator<FilterQuery>, FilterQueryValidator>();
            services.AddTransient<IValidator<StudentsOnPageRequest>, StudentsOnPageRequestValidator>();
            services.AddTransient<IValidator<SearchStudentsRequest>, SearchStudentsRequestValidator>();
            services.AddTransient<IValidator<AddCourseRequest>, AddCourseRequestValidator>();
            services.AddTransient<IValidator<CourseDTO>, CourseDTOValidator>();

            ///
            services.AddTransient<IValidator<EnrollCourseRequest>, EnrolCourseRequestValidator>();
            services.AddTransient<IValidator<UnsubscribeRequest>, UnsubscribeRequestValidator>();

            services.AddTransient<IValidator<UserDTO>, UserDTOValidator>();
            services.AddTransient<IValidator<EditUserRequest>, EditUserRequestValidator>();
            services.AddTransient<IValidator<EditProfileRequest>, EditProfileRequestValidator>();
            
            services.AddTransient<IValidator<StringRequest>, StringRequestValidator>();
            services.AddTransient<IValidator<TokenRequest>, TokenRequestValidator>();
            ///
            services.AddScoped<ICoursesCommands, CoursesCommands>();
            services.AddScoped<IUsersCommands, UsersCommands>();
            services.AddScoped<IHangfireService, HangfireService>();

            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IBulkMailingService, BulkMailingService>();

            services.AddScoped<ITemplateHelper, TemplateHelper>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddFile("Logs/myapp-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseMiddleware<ErrorHandlerMiddleware>();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard();

            //app.UseSwagger();
            //app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET Core Sign-up and Verification API"));

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            //config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHangfireDashboard();

            });

            //CreateRoles(serviceProvider);
        }

        private void CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            Task<IdentityResult> roleResult;

            roleResult = roleManager.CreateAsync(new IdentityRole("Administrator"));
            roleResult.Wait();
            roleResult = roleManager.CreateAsync(new IdentityRole("Student"));
            roleResult.Wait();

        }
    }
}
