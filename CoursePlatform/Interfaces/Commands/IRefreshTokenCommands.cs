using CoursesPlatform.EntityFramework.Models;

namespace CoursesPlatform.Interfaces.Commands
{
    public interface IRefreshTokenCommands
    {
        void RevokeRefreshToken(User user, RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null);

        void CreateRefreshToken(RefreshToken token, User user);
    }
}
