using ShareSpoon.Api.Contracts;
using ShareSpoon.App.Exceptions;
using ShareSpoon.Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;

namespace ShareSpoon.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (Exception ex) when (ex is EntityNotFoundException || ex is LikeNotFoundException || ex is BlobNotFoundException)
            {
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (Exception ex) when (ex is EntityAlreadyExistsException || ex is UserAlreadyExistsException)
            {
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.Conflict);
            }
            catch (InvalidCredentialsException ex)
            {
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
            }
            catch (Exception ex) when (ex is EmptyIngredientsListException || ex is EmptyTagsListException || ex is InvalidImageFormatException)
            {
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleCustomExceptionAsync(HttpContext context, Exception ex, HttpStatusCode httpStatusCode)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;
            var error = new Error
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            };

            // Json field names should start with lower letters
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedError = JsonSerializer.Serialize(error, options);

            await context.Response.WriteAsync(serializedError);
        }
    }
}
