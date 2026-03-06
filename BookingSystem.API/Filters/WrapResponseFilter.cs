using BookingSystem.API.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookingSystem.API.Filters;

public sealed class WrapResponseFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // თუ უკვე ApiResponse აბრუნებს, აღარ გავეხვიოთ მეორედ
        if (context.Result is ObjectResult obj && obj.Value is not null)
        {
            var valType = obj.Value.GetType();
            if (valType.IsGenericType && valType.GetGenericTypeDefinition() == typeof(ApiResponse<>))
            {
                await next();
                return;
            }
        }

        // 204 No Content არ შევფუთოთ
        if (context.Result is NoContentResult)
        {
            await next();
            return;
        }

        // 4xx/5xx არ შევფუთოთ (validation errors აქ ხვდება)
        if (context.Result is ObjectResult o && (o.StatusCode ?? 200) >= 400)
        {
            await next();
            return;
        }

        var traceId = context.HttpContext.TraceIdentifier;

        switch (context.Result)
        {
            case ObjectResult objectResult:
                // OkObjectResult, CreatedAtActionResult
                context.Result = new ObjectResult(Wrap(objectResult.Value, traceId))
                {
                    StatusCode = objectResult.StatusCode ?? StatusCodes.Status200OK
                };
                break;

            case JsonResult jsonResult:
                context.Result = new JsonResult(Wrap(jsonResult.Value, traceId))
                {
                    StatusCode = jsonResult.StatusCode ?? StatusCodes.Status200OK
                };
                break;

            case EmptyResult:
                // მაგალითად Ok() body-ის გარეშე
                context.Result = new ObjectResult(ApiResponse<object>.Ok(null, traceId))
                {
                    StatusCode = StatusCodes.Status200OK
                };
                break;

            case StatusCodeResult sc when sc.StatusCode is >= 200 and < 300:
                // მაგალითად StatusCode(201)
                context.Result = new ObjectResult(ApiResponse<object>.Ok(null, traceId))
                {
                    StatusCode = sc.StatusCode
                };
                break;
        }

        await next();
    }

    private static object Wrap(object? value, string traceId)
    {
        // Generic type runtime-ზე ავაგოთ: ApiResponse<T>
        var t = value?.GetType() ?? typeof(object);
        var apiType = typeof(ApiResponse<>).MakeGenericType(t);
        return Activator.CreateInstance(apiType, true, value, null, traceId)
               ?? ApiResponse<object>.Ok(value, traceId);
    }
}