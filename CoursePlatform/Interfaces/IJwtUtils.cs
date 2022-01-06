using CoursesPlatform.EntityFramework.Models;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface IJwtUtils
    {
        public Task<string> GenerateAccessTokenAsync(User user);

        public RefreshToken GenerateRefreshToken(string ipAddress);

        public void RevokeAccess(User user, string ipAddress);

        void SaveRefreshToken(RefreshToken token, User user);

        bool CheckIsUserHasActiveRefreshToken(User user);

        string GetRefreshToken(User user);

        User GetUserByRefreshToken(string token);
    }
}
