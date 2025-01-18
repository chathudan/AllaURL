using System.Net;
using AllaURL.Domain.Exceptions;

namespace AllaURL.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (TokenNotFoundException notFoundEx)
            {
                await HandleExceptionAsync(context, notFoundEx, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode httpCode)
        {
            _logger.LogError(exception, "An error occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpCode;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message // Optionally include the exception message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}