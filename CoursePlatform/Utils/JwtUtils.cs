using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using CoursesPlatform.EntityFramework;
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

namespace CoursesPlatform.Utils
{
    public class JwtUtils : IJwtUtils
    {
        private readonly UserManager<User> userManager;
        private AppDbContext appDbContext;

        public JwtUtils(UserManager<User> userManager,
                        AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.appDbContext = appDbContext;
        }

        public bool CheckIsUserHasActiveRefreshToken(User user)
        {
            return user.RefreshTokens.FirstOrDefault(t => t.IsActive) != null;
        }

        public string GenerateAccessToken(User user)
        {
            var roles = userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            string jwtTokenSecretKey = "qUF6U9xyx943jk4TnY49nRV6WR2kRhhbwJZjRxG2Y77WnDLnPQ92aHT6jWjw9sfY3YcYsYjPHpSqZvuYd2yDK3z47n";

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSecretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                                            signingCredentials: signingCredentials,
                                            claims: claims,
                                            expires: DateTime.Now.AddSeconds(20)
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
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            return refreshToken;
        }

        public string GetRefreshToken(User user)
        {
            var refreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);

            if (refreshToken == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "No active refresh token !" });
            }

            return refreshToken.Token;
        }

        public void RevokeRefreshToken(string token, string ipAddress)
        {
            var user = GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid token" });
            }

            RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");

            appDbContext.Update(user);
            appDbContext.SaveChanges();
        }

        private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        public void SaveRefreshToken(RefreshToken token, User user)
        {
            user.RefreshTokens.Add(token);

            appDbContext.Update(user);
            appDbContext.SaveChanges();
        }

        public User GetUserByRefreshToken(string token)
        {
            var user = appDbContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid token !" });
            }

            return user;
        }

        public void RevokeAccess(User user, string ipAddress)
        {
            bool isUserHasActiveRefreshToken = CheckIsUserHasActiveRefreshToken(user);

            if (isUserHasActiveRefreshToken)
            {
                var refreshToken = GetRefreshToken(user);
                RevokeRefreshToken(refreshToken, ipAddress);
            }
        }
    }
}
