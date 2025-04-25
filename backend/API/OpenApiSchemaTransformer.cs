using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace PRegSys.API;

public class OpenApiSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        // Fill in the schema title from type name if it's not set
        // (so that the API documentation page in Scalar is more readable)
        schema.Title ??= context.JsonTypeInfo.Type?.Name;

        // workaround for a bug where the type of a get-only property is incorrectly shown as nullable
        if (context.JsonPropertyInfo is { IsGetNullable: false, Set: null })
        {
            schema.Type &= ~JsonSchemaType.Null; // mark the type as non-nullable
        }

        // Add full type name for non-builtin types
        if (context.JsonTypeInfo.Type?.FullName?.StartsWith("System.") is false)
        {
            schema.Description += $"\n\n`{context.JsonTypeInfo.Type?.FullName}`";
        }

        if (context.JsonTypeInfo.Type == typeof(NodaTime.Instant))
        {
            schema.Type ??= JsonSchemaType.String;
            schema.Format ??= "date-time";
            schema.Example ??= "2025-05-17T11:15:05Z";
        }

        return Task.CompletedTask;
    }
}
