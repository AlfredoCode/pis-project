using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

public class TeamEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/teams", async Task<IEnumerable<Team>> (TeamService teams) =>
        {
            return await teams.GetAllTeams();
        }).WithName("GetAllTeams");

        group.MapGet("/teams/{id}", async Task<Results<Ok<Team>, NotFound>> (TeamService teams, int id) =>
        {
            return (await teams.GetTeamById(id) is Team team)
                ? TypedResults.Ok(team)
                : TypedResults.NotFound();
        }).WithName("GetTeamById");

        group.MapGet("/projects/{projectId}/teams", async Task<IEnumerable<Team>> (TeamService teams, int projectId) =>
        {
            return await teams.GetTeamsByProjectId(projectId);
        }).WithName("GetTeamsByProjectId");

        group.MapGet("/students/{studentId}/teams", async Task<IEnumerable<Team>> (TeamService teams, int studentId) =>
        {
            return await teams.GetTeamsByStudentId(studentId);
        }).WithName("GetTeamsByStudentId");

        group.MapPost("/teams", async Task<Results<Created<Team>, BadRequest<string>>> (TeamService teams, Team team) =>
        {
            try
            {
                var created = await teams.CreateTeam(team);
                return TypedResults.Created($"/teams/{created.Id}", created);
            }
            catch (DbUpdateException ex)
            {
                return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }).WithName("CreateTeam");

        group.MapPut("/teams/{id}", async Task<Results<Ok<Team>, BadRequest<string>>> (TeamService teams, int id, Team team) =>
        {
            team.Id = id;
            try
            {
                var updated = await teams.UpdateTeam(team);
                return TypedResults.Ok(updated);
            }
            catch (DbUpdateException ex)
            {
                return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }).WithName("UpdateTeam");

        group.MapDelete("/teams/{id}", async (TeamService teams, int id) =>
        {
            await teams.DeleteProject(id);
            return TypedResults.NoContent();
        }).WithName("DeleteTeam");
    }
}