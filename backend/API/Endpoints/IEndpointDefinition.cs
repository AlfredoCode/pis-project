namespace PRegSys.API.Endpoints;

public interface IEndpointDefinition
{
    void RegisterEndpoints(RouteGroupBuilder group);
}