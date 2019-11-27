using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VerticalSlice.POC.Core;

namespace VerticalSlice.POC.Infrastructure.MiddlewareFilters
{
    public class ErrorHandlingFilter
    {
        private readonly RequestDelegate next;

        public ErrorHandlingFilter(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger", true);
                    return;
                }
                await next(context);
                if (context.Response.StatusCode == 404)
                    throw new CoreException($"The call you made was not found. For all available endpoints, check <a href=\"/swagger\">swaggegr</a>");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            string message = "";
            object jContent = null;

            if (exception is CoreException ce)
            {
                code = HttpStatusCode.BadRequest;
                message = ce.Message;
            }
            else if (exception is FluentValidation.ValidationException ve)
            {
                code = HttpStatusCode.BadRequest;
                jContent = new ErrorsResponse(ve.Errors);
            }
            else
            {
                message = exception.Message;
            }

            var result = JsonConvert.SerializeObject(new { Error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;


            return context.Response.WriteAsync(JsonConvert.SerializeObject(jContent).Equals("null") ? result : JsonConvert.SerializeObject(jContent));
        }

        public class ErrorsResponse
        {
            public ErrorsResponse(IEnumerable<FluentValidation.Results.ValidationFailure> validationFailures)
            {
                Errors = validationFailures.Select(x => new Error
                {
                    PropertyName = x.PropertyName,
                    ErrorMessage = x.ErrorMessage,
                    AttemptedValue = x.AttemptedValue
                }).ToList();
            }
            public class Error
            {
                public string PropertyName { get; set; }
                public string ErrorMessage { get; set; }
                public object AttemptedValue { get; set; }
            }

            public IList<Error> Errors { get; set; }
        }
    }
}
