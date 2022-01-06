using CoursesPlatform.ErrorMiddleware.Errors;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CoursesPlatform.ErrorMiddleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public ErrorHandlerMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception error)
            {
                var response = context.Response;

                response.ContentType = "application/json";

                object errors = null;

                switch (error)
                {
                    case RestException ex:
                        {
                            errors = ex.Errors;
                            response.StatusCode = (int)ex.StatusCode;
                            break;
                        }
                    default:
                        {
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                        }
                }

                var result = JsonConvert.SerializeObject(new
                {
                    errors
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}
