using CoursesPlatform.EntityFramework.Models;

namespace CoursesPlatform.Interfaces
{
    public interface IJwtUtils
    {
        public string GenerateAccessToken(User user);

        public RefreshToken GenerateRefreshToken(string ipAddress);

        public void SaveRefreshToken(RefreshToken token, User user);

        void RevokeRefreshToken(string token, string ipAddress);

        bool CheckIsUserHasActiveRefreshToken(User user);

        string GetRefreshToken(User user);

        public User GetUserByRefreshToken(string token);

        public void RevokeAccess(User user, string ipAddress);

    }
}
