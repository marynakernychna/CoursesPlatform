using System;
using System.Globalization;

namespace CoursesPlatform.ErrorMiddleware.Errors
{
    public class Unauthorized : Exception
    {
        public Unauthorized() : base() { }

        public Unauthorized(string message) : base(message) { }

        public Unauthorized(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}
