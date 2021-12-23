using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CoursesPlatform.Helpers
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            string id = httpContextAccessor.HttpContext.User?.Claims?
                                                             .FirstOrDefault(x => x.Type == "id")?.Value;

            if (string.IsNullOrEmpty(id))
            {
                throw new InternalServerError();
            }

            return id;
        }
    }
}
