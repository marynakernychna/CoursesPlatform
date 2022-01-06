using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Interfaces.Queries;
using CoursesPlatform.Models;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CoursesPlatform.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly IJwtUtils jwtUtils;
        private readonly IUserQueries userQueries;
        private readonly IUsersCommands usersCommands;
        private readonly IEmailService emailService;
        private readonly IUtils utils;

        public AuthService(UserManager<User> userManager,
                           IJwtUtils jwtUtils,
                           IUserQueries userQueries,
                           IUsersCommands usersCommands,
                           IEmailService emailService,
                           IUtils utils)
        {
            this.userManager = userManager;
            this.jwtUtils = jwtUtils;
            this.userQueries = userQueries;
            this.usersCommands = usersCommands;
            this.emailService = emailService;
            this.utils = utils;
        }

        public async Task<AuthenticateResponse> LogInAsync(AuthenticateRequest request, string ipAddress)
        {
            var user = userQueries.GetUserByEmail(request.Email);

            await usersCommands.SignInAsync(user, request.Password, false);

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

        public async Task RegisterUserAsync(RegisterRequest request, HttpRequest httpRequest)
        {
            var isEmailExists = userQueries.CheckIsUserExistsByEmail(request.Email);

            if (isEmailExists)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "There is already a user with this email!" });
            }

            var user = usersCommands.CreateNewUserModel(request);

            await userQueries.RegisterStudentAsync(user, request.Password);

            await emailService.SendConfirmationEmailAsync(user);
        }

        public async Task<AuthenticateResponse> LogInViaFacebookAsync(StringRequest facebookToken, string ipAddress)
        {
            var facebookUser = await usersCommands.GetUserFromFacebookAsync(facebookToken.Value);

            var isEmailExists = userQueries.CheckIsUserExistsByEmail(facebookUser.Email);

            User user = null;

            if (!isEmailExists)
            {
                var password = utils.GenerateRandomPassword();

                user = usersCommands.CreateNewUserModel(new RegisterRequest
                {
                    Name = facebookUser.FirstName,
                    Surname = facebookUser.LastName,
                    Email = facebookUser.Email,
                    Birthday = DateTime.UtcNow,
                    Password = password
                });

                await userQueries.RegisterStudentAsync(user, password);
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
