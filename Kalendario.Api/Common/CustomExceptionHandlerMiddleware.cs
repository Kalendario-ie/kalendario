using System;
using System.Net;
using System.Threading.Tasks;
using Kalendario.Application.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kalendario.Api.Common
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                    {NamingStrategy = new CamelCaseNamingStrategy {ProcessDictionaryKeys = true}},
            };

            var code = HttpStatusCode.InternalServerError;

            var detail = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.UnprocessableEntity;
                    detail = JsonConvert.SerializeObject(validationException.Failures, settings);
                    break;
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    detail = badRequestException.Message;
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;

            if (detail == string.Empty)
            {
                detail = JsonConvert.SerializeObject(new {error = exception.Message});
            }

            var problem = new ProblemDetails
            {
                Detail = detail,
                Status = (int) code,
            };

            var result = JsonConvert.SerializeObject(problem, settings);

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}