using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CoursesPlatform.Interfaces;
using System.Threading.Tasks;
using CoursesPlatform.Interfaces.Queries;

namespace CoursesPlatform.Utils
{
    public class JwtUtils : IJwtUtils
    {
        private readonly UserManager<User> userManager;
        private readonly IRefreshTokenQueries refreshTokenQueries;

        public JwtUtils(UserManager<User> userManager,
                        IRefreshTokenQueries refreshTokenQueries)
        {
            this.userManager = userManager;
            this.refreshTokenQueries = refreshTokenQueries;
        }

        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StringConstants.JwtTokenSecretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                                            signingCredentials: signingCredentials,
                                            claims: claims,
                                            expires: DateTime.Now.AddHours(3)
                                          );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(3),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            return refreshToken;
        }

        private void RevokeRefreshToken(string token, string ipAddress)
        {
            var user = refreshTokenQueries.GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid token!" });
            }

            refreshTokenQueries.RevokeRefreshToken(user, refreshToken, ipAddress, "Revoked without replacement");
        }

        public void RevokeAccess(User user, string ipAddress)
        {
            bool isUserHasActiveRefreshToken = refreshTokenQueries.CheckIsUserHasActiveRefreshToken(user);

            if (isUserHasActiveRefreshToken)
            {
                var refreshToken = refreshTokenQueries.GetRefreshToken(user);

                RevokeRefreshToken(refreshToken, ipAddress);
            }
        }

        public void SaveRefreshToken(RefreshToken token, User user)
        {
            refreshTokenQueries.SaveRefreshToken(token, user);
        }

        public bool CheckIsUserHasActiveRefreshToken(User user)
        {
            if (user.RefreshTokens == null)
            {
                return false;
            }

            return user.RefreshTokens.FirstOrDefault(t => t.IsActive) != null;
        }

        public string GetRefreshToken(User user)
        {
            var refreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);

            if (refreshToken == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Message = "Active refresh token not found!" });
            }

            return refreshToken.Token;
        }

        public User GetUserByRefreshToken(string token)
        {
            return refreshTokenQueries.GetUserByRefreshToken(token);
        }
    }
}
