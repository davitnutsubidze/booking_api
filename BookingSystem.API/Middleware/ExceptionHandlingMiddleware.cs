using System.Net;
using FluentValidation;
using BookingSystem.Application.Common.Exceptions;

namespace BookingSystem.API.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            // FluentValidation errors -> { field: [messages] }
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );

            await context.Response.WriteAsJsonAsync(new
            {
                type = "validation_error",
                title = "Validation failed",
                errors
            });
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                type = "not_found",
                title = ex.Message
            });
        }
        catch (ConflictException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                type = "conflict",
                title = ex.Message
            });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            // პროდაქშენში დეტალს არ  უნდა დავაბრუნოთ
            await context.Response.WriteAsJsonAsync(new
            {
                type = "server_error",
                title = "An unexpected error occurred."
            });
        }
    }
}
