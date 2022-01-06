using Microsoft.AspNetCore.Http;

namespace CoursesPlatform.Interfaces
{
    public interface IUtils
    {
        public string GetIpAddressOfCurrentRequest(HttpRequest request, HttpContext httpContext);

        public string GenerateRandomPassword();

    }
}
