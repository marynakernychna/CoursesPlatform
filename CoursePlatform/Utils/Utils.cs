using System;
using System.Linq;
using CoursesPlatform.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CoursesPlatform.Utils
{
    public class Utils : IUtils
    {
        private Random random = new Random();

        public string GenerateRandomPassword()
        {
            return new string(Enumerable.Repeat(StringConstants.SymbolsForGeneratePassword, 10)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string GetIpAddressOfCurrentRequest(HttpRequest request, HttpContext httpContext)
        {
            if (request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return request.Headers["X-Forwarded-For"];
            }
            else
            {
                return httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}
