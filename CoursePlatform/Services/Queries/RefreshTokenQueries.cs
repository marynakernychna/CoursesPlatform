using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces.Queries;
using System;
using System.Linq;
using System.Net;

namespace CoursesPlatform.Services.Queries
{
    public class RefreshTokenQueries : IRefreshTokenQueries
    {
        private AppDbContext appDbContext;

        public RefreshTokenQueries(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public bool CheckIsUserHasActiveRefreshToken(User user)
        {
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
            var user = appDbContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid token !" });
            }

            return user;
        }

        public void RevokeRefreshToken(User user, RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
            token.RevokedByIp = ipAddress;

            appDbContext.Update(user);
            appDbContext.SaveChanges();
        }

        public void SaveRefreshToken(RefreshToken token, User user)
        {
            user.RefreshTokens.Add(token);

            appDbContext.Update(user);
            appDbContext.SaveChanges();
        }
    }
}
