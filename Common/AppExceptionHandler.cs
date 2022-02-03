using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

using Microsoft.AspNetCore.Diagnostics;
using Eauction_Seller_API.Models;

namespace Eauction_Seller_API.Common
{
    public class AppExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AppExceptionHandler> _logger;

        public AppExceptionHandler(RequestDelegate next, ILogger<AppExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            if (exception is ArgumentException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = exception.Message
                }));
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error."
                }));
            }
            _logger.LogError($"Something went wrong: {exception.Message}");
        }
    }
}
