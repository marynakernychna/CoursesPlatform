using CoursesPlatform.EntityFramework.Models;

namespace CoursesPlatform.Interfaces.Queries
{
    public interface IRefreshTokenQueries
    {
        bool CheckIsUserHasActiveRefreshToken(User user);

        string GetRefreshToken(User user);

        User GetUserByRefreshToken(string token);
    }
}
