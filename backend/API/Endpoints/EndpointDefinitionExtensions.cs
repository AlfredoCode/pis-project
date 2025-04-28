using System.Reflection;

namespace PRegSys.API.Endpoints;

public static class EndpointDefinitionExtensions
{
    public static void RegisterEndpointDefinitions(this WebApplication app)
    {
        var api = app.MapGroup("/api");

        var definitions = Assembly.GetExecutingAssembly()
            .DefinedTypes
            .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>();

        foreach (var definition in definitions)
        {
            string tag = removeSuffix(definition.GetType().Name, "Endpoints");

            var group = api.MapGroup("").WithTags("all endpoints", tag);

            if (!definition.GetType().Name.Equals("AuthEndpoints", StringComparison.OrdinalIgnoreCase))
            {
                group.RequireAuthorization();
            }

            definition.RegisterEndpoints(group);
        }

        static string removeSuffix(string str, string suffix)
            => str.EndsWith(suffix) ? str[..^suffix.Length] : str;
    }
}
