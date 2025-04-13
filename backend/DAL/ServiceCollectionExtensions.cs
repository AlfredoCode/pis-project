using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using PRegSys.DAL.Repositories;

namespace PRegSys.DAL;

public static class ServiceCollectionExtensions
{
    public static void AddDALServices(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace("Connection string is not set");

        services.AddDbContextPool<PregsysDbContext>(o => {
            o.UseNpgsql(connectionString, o => {
                // configure the PostgreSQL connection
                o.UseNodaTime();
            });
            o.UseSnakeCaseNamingConvention();
        });
        services.AddScoped<ProjectRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<TeamRepository>();
        services.AddScoped<SolutionRepository>();
        services.AddScoped<EvaluationRepository>();
        services.AddScoped<SignRequestRepository>();
    }
}