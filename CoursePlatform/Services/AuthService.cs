using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models.Facebook;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoursesPlatform.Services
{
    public class AuthService : IAuthService
    {
        private AppDbContext appDbContext;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IJwtUtils jwtUtils;
        private readonly IHttpClientFactory httpClientFactory;

        public AuthService(AppDbContext appDbContext,
                           UserManager<User> userManager,
                           IJwtUtils jwtUtils,
                           IHttpClientFactory httpClientFactory,
                           SignInManager<User> signInManager)
        {
            this.appDbContext = appDbContext;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.httpClientFactory = httpClientFactory;
            this.jwtUtils = jwtUtils;
        }

        public async Task SignInAsync(User user, string password, bool lockoutOnFailure)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);

            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid login or password !" });
            }
        }

        public async Task RegisterUser(User user, string password)
        {
            IdentityResult result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new InternalServerError();
            }

            result = await userManager.AddToRoleAsync(user, "Student");

            if (!result.Succeeded)
            {
                throw new InternalServerError();
            }

            appDbContext.SaveChanges();
        }

        public User CreateNewUserModel(RegisterRequest request)
        {
            return new User
            {
                UserName = request.Email,
                Email = request.Email,
                Birthday = request.Birthday,
                Surname = request.Surname,
                Name = request.Name,
                RegisteredDate = DateTime.Now
            };
        }

        public async Task<FacebookAccount> GetUserFromFacebookAsync(string facebookToken)
        {
            string facebookGraphUrl = $"https://graph.facebook.com/v4.0/me?access_token={facebookToken}&fields=email,first_name,last_name";

            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(facebookGraphUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Response wasn't success !" });
            }

            var result = await response.Content.ReadAsStringAsync();
            var facebookAccount = JsonConvert.DeserializeObject<FacebookAccount>(result);

            return facebookAccount;
        }
    }
}
