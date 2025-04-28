using Microsoft.AspNetCore.Http.HttpResults;

using NodaTime;

using PRegSys.API.DTO;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

public class SolutionEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/solutions/{id}",
            async Task<Results<Ok<SolutionReadDto>, NotFound>> (SolutionService solutions, int id) =>
            {
                return (await solutions.GetSolutionById(id) is Solution solution)
                    ? TypedResults.Ok(solution.ToDto())
                    : TypedResults.NotFound();
            })
            .WithName("GetSolutionById");

        group.MapGet("/projects/{projectId}/solutions",
            async Task<IEnumerable<SolutionReadDto>> (SolutionService solutions, int projectId) =>
            {
                return (await solutions.GetSolutionsByProjectId(projectId)).ToDto();
            })
            .WithName("GetSolutionsByProjectId");

        group.MapGet("/teams/{teamId}/solutions",
            async Task<IEnumerable<SolutionReadDto>> (SolutionService solutions, int teamId) =>
            {
                return (await solutions.GetSolutionsByTeamId(teamId))
                    .OrderByDescending(s => s.SubmissionDate)
                    .ToDto();
            })
            .WithName("GetSolutionsByTeamId")
            .WithDescription("""
                Gets all solutions to a project submitted by 
                a specific team, ordered by date (most recently submitted first).
                """);

        group.MapPost("/solutions",
            async Task<Created<SolutionReadDto>> (SolutionService solutions, SolutionWriteDto solution, IClock clock) =>
            {
                var created = await solutions.SubmitSolution(solution.ToEntity(clock));
                return TypedResults.Created($"/solutions/{created.Id}", created.ToDto());
            })
            .WithName("SubmitSolution");

        group.MapDelete("/solutions/{id}",
            async (SolutionService solutions, int id) =>
            {
                await solutions.DeleteSolution(id);
                return TypedResults.NoContent();
            })
            .WithName("DeleteSolution");
    }
}