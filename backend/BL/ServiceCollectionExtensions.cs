using Microsoft.Extensions.DependencyInjection;

using PRegSys.BL.Services;

namespace PRegSys.BL;

public static class ServiceCollectionExtensions
{
    public static void AddBLServices(this IServiceCollection services)
    {
        services.AddScoped<ProjectService>();
        services.AddScoped<UserService>();
        services.AddScoped<TeamService>();
        services.AddScoped<EvaluationService>();
        services.AddScoped<SolutionService>();
        services.AddScoped<SignUpRequestService>();
    }
}