using System;
using System.Net;

namespace CoursesPlatform.ErrorMiddleware.Errors
{
    public class RestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public object Errors { get; }

        public RestException(HttpStatusCode statusCode, object errors = null)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
