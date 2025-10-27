using CMS.DTOs;
using System.Net;
using System.Text.Json;

namespace CMS.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ApiResponse response;

            switch (exception)
            {
                case ArgumentNullException:
                    response = ApiResponse.ErrorResponse("Invalid request: Required parameter is missing");
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case ArgumentException:
                    response = ApiResponse.ErrorResponse($"Invalid request: {exception.Message}");
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedAccessException:
                    response = ApiResponse.ErrorResponse("Unauthorized access");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case KeyNotFoundException:
                    response = ApiResponse.ErrorResponse("Resource not found");
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case InvalidOperationException:
                    response = ApiResponse.ErrorResponse(exception.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    response = ApiResponse.ErrorResponse("An unexpected error occurred");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
