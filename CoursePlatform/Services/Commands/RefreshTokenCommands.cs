using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Interfaces.Commands;
using System;

namespace CoursesPlatform.Services.Commands
{
    public class RefreshTokenCommands : IRefreshTokenCommands
    {
        private AppDbContext appDbContext;

        public RefreshTokenCommands(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
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

        public void CreateRefreshToken(RefreshToken token, User user)
        {
            user.RefreshTokens.Add(token);

            appDbContext.Update(user);
            appDbContext.SaveChanges();
        }
    }
}
