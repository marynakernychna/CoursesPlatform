using CoursesPlatform.EntityFramework.Models;

namespace CoursesPlatform.Interfaces.Queries
{
    public interface IRefreshTokenQueries
    {
        bool CheckIsUserHasActiveRefreshToken(User user);

        string GetRefreshToken(User user);

        User GetUserByRefreshToken(string token);

        void RevokeRefreshToken(User user, RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null);

        void SaveRefreshToken(RefreshToken token, User user);
    }
}
