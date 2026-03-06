using BookingSystem.API.Contracts;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BookingSystem.API.Swagger;

public sealed class ApiResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var kv in operation.Responses)
        {
            var statusCode = kv.Key;
            var apiResponse = kv.Value;

            // მხოლოდ 2xx responses ვფუთავთ
            if (!statusCode.StartsWith("2"))
                continue;

            // 204 No Content -> body არ აქვს, ამიტომ გამოტოვე
            if (statusCode == "204")
                continue;

            // Content შეიძლება null/empty იყოს
            if (apiResponse.Content is null || apiResponse.Content.Count == 0)
                continue;

            // ვიპოვოთ json media type (application/json ან text/json)
            var jsonMedia = apiResponse.Content
                .FirstOrDefault(x =>
                    x.Key.Equals("application/json", StringComparison.OrdinalIgnoreCase) ||
                    x.Key.Equals("text/json", StringComparison.OrdinalIgnoreCase));

            // თუ json საერთოდ არ აქვს, skip
            if (string.IsNullOrWhiteSpace(jsonMedia.Key))
                continue;

            // Return type -> unwrap Task<>
            var returnType = context.MethodInfo.ReturnType;
            if (typeof(Task).IsAssignableFrom(returnType))
                returnType = returnType.IsGenericType ? returnType.GetGenericArguments()[0] : typeof(void);

            // IActionResult ან void -> object wrapper
            Type wrappedType;

            // თუ returnType არის ApiResponse<T> უკვე, აღარ შევცვალოთ
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ApiResponse<>))
                continue;

            // თუ returnType არის ActionResult<T> -> T
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition().Name.StartsWith("ActionResult"))
                returnType = returnType.GetGenericArguments()[0];

            // wrap
            wrappedType = typeof(ApiResponse<>).MakeGenericType(returnType == typeof(void) ? typeof(object) : returnType);

            var schema = context.SchemaGenerator.GenerateSchema(wrappedType, context.SchemaRepository);
            apiResponse.Content[jsonMedia.Key].Schema = schema;
        }
    }
}