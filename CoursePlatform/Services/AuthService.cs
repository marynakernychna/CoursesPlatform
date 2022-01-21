using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Interfaces.Queries;
using CoursesPlatform.Models;
using CoursesPlatform.Models.Facebook;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoursesPlatform.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly IJwtUtils jwtUtils;
        private readonly IUserQueries userQueries;
        private readonly IEmailService emailService;
        private readonly IUtils utils;
        private readonly SignInManager<User> signInManager;
        private readonly IHttpClientFactory httpClientFactory;
        private AppDbContext appDbContext;

        public AuthService(UserManager<User> userManager,
                           IJwtUtils jwtUtils,
                           IUserQueries userQueries,
                           IHttpClientFactory httpClientFactory,
                           IEmailService emailService,
                           SignInManager<User> signInManager,
                           IUtils utils,
                           AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.httpClientFactory = httpClientFactory;
            this.jwtUtils = jwtUtils;
            this.userQueries = userQueries;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.utils = utils;
            this.appDbContext = appDbContext;
        }

        public async Task<AuthenticateResponse> LogInAsync(AuthenticateRequest request, string ipAddress)
        {
            var user = userQueries.GetUserByEmail(request.Email);

            await SignInAsync(user, request.Password, false);

            if (!user.EmailConfirmed)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Email is not confirmed!" });
            }

            var accessToken = await jwtUtils.GenerateAccessTokenAsync(user);

            var isRefreshTokenActive = jwtUtils.CheckIsUserHasActiveRefreshToken(user);

            string refreshToken = null;

            if (isRefreshTokenActive)
            {
                refreshToken = jwtUtils.GetRefreshToken(user);
            }
            else
            {
                var newRefreshToken = jwtUtils.GenerateRefreshToken(ipAddress);

                jwtUtils.SaveRefreshToken(newRefreshToken, user);

                refreshToken = newRefreshToken.Token;
            }

            return new AuthenticateResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private async Task SignInAsync(User user, string password, bool lockoutOnFailure)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);

            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid login or password !" });
            }
        }

        public async Task RegisterUserAsync(RegisterRequest request, HttpRequest httpRequest)
        {
            var isEmailExists = userQueries.CheckIsUserExistsByEmail(request.Email);

            if (isEmailExists)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "There is already a user with this email!" });
            }

            var user = CreateNewUserModel(request);

            await RegisterStudentAsync(user, request.Password);

            await emailService.SendConfirmationEmailAsync(user);
        }

        private User CreateNewUserModel(RegisterRequest request)
        {
            return new User
            {
                UserName = request.Email,
                Email = request.Email,
                Birthday = request.Birthday,
                Surname = request.Surname,
                Name = request.Name,
                RegisteredDate = DateTime.UtcNow
            };
        }

        private async Task RegisterStudentAsync(User user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.InternalServerError, new { Message = "Failed to create user!" });
            }

            result = await userManager.AddToRoleAsync(user, StringConstants.StudentRole);

            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.InternalServerError, new { Message = "Failed to add user to role!" });
            }

            appDbContext.SaveChanges();
        }

        public async Task<AuthenticateResponse> LogInViaFacebookAsync(StringRequest facebookToken, string ipAddress)
        {
            var facebookUser = await GetUserFromFacebookAsync(facebookToken.Value);

            var isEmailExists = userQueries.CheckIsUserExistsByEmail(facebookUser.Email);

            User user = null;

            if (!isEmailExists)
            {
                var password = utils.GenerateRandomPassword();

                user = CreateNewUserModel(new RegisterRequest
                {
                    Name = facebookUser.FirstName,
                    Surname = facebookUser.LastName,
                    Email = facebookUser.Email,
                    Birthday = DateTime.UtcNow,
                    Password = password
                });

                await RegisterStudentAsync(user, password);
            }
            else
            {
                user = userQueries.GetUserByEmail(facebookUser.Email);
            }

            var accessToken = await jwtUtils.GenerateAccessTokenAsync(user);

            var isRefreshTokenActive = jwtUtils.CheckIsUserHasActiveRefreshToken(user);

            string refreshToken = null;

            if (isRefreshTokenActive)
            {
                refreshToken = jwtUtils.GetRefreshToken(user);
            }
            else
            {
                var newRefreshToken = jwtUtils.GenerateRefreshToken(ipAddress);

                jwtUtils.SaveRefreshToken(newRefreshToken, user);

                refreshToken = newRefreshToken.Token;
            }

            return new AuthenticateResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private async Task<FacebookAccount> GetUserFromFacebookAsync(string facebookToken)
        {
            string facebookGraphUrl = $"https://graph.facebook.com/v4.0/me?access_token={facebookToken}&fields=email,first_name,last_name";

            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(facebookGraphUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new RestException(HttpStatusCode.InternalServerError, new { Message = "Failed to get Facebook user from token!" });
            }

            var result = await response.Content.ReadAsStringAsync();
            var facebookAccount = JsonConvert.DeserializeObject<FacebookAccount>(result);

            return facebookAccount;
        }

        public async Task ConfirmEmailAsync(EmailConfirmationRequest request)
        {
            var user = userQueries.GetUserByEmail(request.Email);

            await userManager.ConfirmEmailAsync(user, request.Token);
        }

        public async Task<string> RefreshAccessTokenAsync(TokenRequest request)
        {
            var user = jwtUtils.GetUserByRefreshToken(request.Token);

            return await jwtUtils.GenerateAccessTokenAsync(user);
        }
    }
}
