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

        // Add full type name for non-builtin types
        if (context.JsonTypeInfo.Type?.FullName?.StartsWith("System.") is false)
        {
            schema.Description += $"\n\n`{context.JsonTypeInfo.Type?.FullName}`";
        }

        if (context.JsonPropertyInfo is { } property)
        {
            // Mark Entity ID and navigation properties as read-only (only relevant for GET requests)
            if (property.Name == "id"
                || property.PropertyType.IsAssignableTo(typeof(DAL.Entities.IEntity)))
            {
                schema.ReadOnly = true;
            }

            // Mark properties with no setter and no constructor argument as read-only
            if (property is { Get: not null, Set: null, AssociatedParameter: null })
                schema.ReadOnly = true;
        }

        if (schema is { Type: "integer", Description: null })
        {
            // prevent the useless default description for numerical properties in Scalar UI
            schema.Description = " ";
        }

        return Task.CompletedTask;
    }
}
