using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PostService.Application.Exceptions;
using PostService.Application.Features.Common;
using System.Text.Json;

namespace PostService.API.ExceptionHandler
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for request {Method} {Path}",
                context.Request.Method, context.Request.Path);

            var (statusCode, message) = ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, ex.Message),
                UnauthorizedAccessException => (StatusCodes.Status403Forbidden, ex.Message),
                ConflictException => (StatusCodes.Status409Conflict, ex.Message),
                ValidationException validation => (StatusCodes.Status400BadRequest, string.Join(" ", validation.Errors.Select(e => e.ErrorMessage))),
                // A unique index rejected a concurrent duplicate (second active post, same identity number, ...).
                DbUpdateException { InnerException: PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } }
                    => (StatusCodes.Status409Conflict, "The request conflicts with existing data."),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;
                var errorResponse = new StandardResponse<object>
                {
                    Response = null,
                    IsSuccess = false,
                    ResultCode = statusCode.ToString(),
                    ResultMessage = message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, _jsonOptions));
            }
            else
            {
                _logger.LogWarning("Response has already started. Unable to write error response.");
            }
        }
    }
}
