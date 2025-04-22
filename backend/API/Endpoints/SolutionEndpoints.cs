using Microsoft.AspNetCore.Http.HttpResults;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

public class SolutionEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/solutions/{id}", async Task<Results<Ok<Solution>, NotFound>> (SolutionService solutions, int id) =>
        {
            return (await solutions.GetSolutionById(id) is Solution solution)
                ? TypedResults.Ok(solution)
                : TypedResults.NotFound();
        }).WithName("GetSolutionById");

        group.MapGet("/projects/{projectId}/solutions", async Task<IEnumerable<Solution>> (SolutionService solutions, int projectId) =>
        {
            return await solutions.GetSolutionsByProjectId(projectId);
        }).WithName("GetSolutionsByProjectId");

        group.MapGet("/teams/{teamId}/solutions", async Task<IEnumerable<Solution>> (SolutionService solutions, int teamId) =>
        {
            return await solutions.GetSolutionsByTeamId(teamId);
        }).WithName("GetSolutionsByTeamId");

        group.MapPost("/solutions", async Task<Created<Solution>> (SolutionService solutions, Solution solution) =>
        {
            var created = await solutions.SubmitSolution(solution);
            return TypedResults.Created($"/solutions/{created.Id}", created);
        }).WithName("SubmitSolution");

        group.MapDelete("/solutions/{id}", async (SolutionService solutions, int id) =>
        {
            await solutions.DeleteSolution(id);
            return TypedResults.NoContent();
        }).WithName("DeleteSolution");
    }
}