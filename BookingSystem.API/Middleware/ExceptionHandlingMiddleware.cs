using BookingSystem.API.Contracts;
using BookingSystem.Application.Common.Exceptions;
using FluentValidation;
using System.Text.Json;

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
            await WriteError(context, 400, "validation_error", "Validation failed",
                ex.Errors
                  .GroupBy(e => e.PropertyName)
                  .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray()));
        }
        catch (NotFoundException ex)
        {
            await WriteError(context, 404, "not_found", ex.Message);
        }
        catch (ConflictException ex)
        {
            await WriteError(context, 409, "conflict", ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            await WriteError(context, 401, "unauthorized", "Unauthorized");
        }
        catch (Exception)
        {
            // production-ში დეტალები არ გაამჟღავნო
            await WriteError(context, 500, "server_error", "Internal Server Error");
        }
    }

    private static async Task WriteError(
        HttpContext context,
        int status,
        string type,
        string title,
        IDictionary<string, string[]>? errors = null)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";

        var traceId = context.TraceIdentifier;

        var payload = ApiResponse<object>.Fail(
            new ApiError(type, title, status, errors),
            traceId);

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}