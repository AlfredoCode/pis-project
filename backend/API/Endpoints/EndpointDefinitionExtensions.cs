using System.Reflection;

namespace PRegSys.API.Endpoints;

public static class EndpointDefinitionExtensions
{
    public static void RegisterEndpointDefinitions(this WebApplication app)
    {
        var group = app.MapGroup("/api");

        var definitions = Assembly.GetExecutingAssembly()
            .DefinedTypes
            .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>();

        foreach (var definition in definitions)
        {
            definition.RegisterEndpoints(group);
        }
    }
}