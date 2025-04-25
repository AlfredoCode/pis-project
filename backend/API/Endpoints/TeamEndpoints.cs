using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using PRegSys.API.DTO;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

public class TeamEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/teams",
            async Task<IEnumerable<TeamReadDto>> (TeamService teams) =>
            {
                return (await teams.GetAllTeams()).ToDto();
            })
            .WithName("GetAllTeams");

        group.MapGet("/teams/{id}",
            async Task<Results<Ok<TeamReadDto>, NotFound>> (TeamService teams, int id) =>
            {
                return (await teams.GetTeamById(id) is Team team)
                    ? TypedResults.Ok(team.ToDto())
                    : TypedResults.NotFound();
            })
            .WithName("GetTeamById");

        group.MapGet("/projects/{projectId}/teams",
            async Task<IEnumerable<TeamReadDto>> (TeamService teams, int projectId) =>
            {
                return (await teams.GetTeamsByProjectId(projectId)).ToDto();
            })
            .WithName("GetTeamsByProjectId");

        group.MapGet("/students/{studentId}/teams",
            async Task<IEnumerable<TeamReadDto>> (TeamService teams, int studentId) =>
            {
                return (await teams.GetTeamsByStudentId(studentId)).ToDto();
            })
            .WithName("GetTeamsByStudentId")
            .WithDescription("Get all teams a student is in");

        group.MapGet("/teams/{teamId}/students",
            async Task<Results<Ok<IEnumerable<StudentReadDto>>, NotFound>> (TeamService teams, int teamId) =>
            {
                return (await teams.GetStudentsInTeam(teamId) is { } students)
                    ? TypedResults.Ok(students.ToDto())
                    : TypedResults.NotFound();
            })
            .WithName("GetStudentsInTeam");

        group.MapPost("/teams",
            async Task<Results<Created<TeamReadDto>, BadRequest<string>>> (TeamService teams, TeamWriteDto team) =>
            {
                try
                {
                    var created = await teams.CreateTeam(team.ToEntity(default));
                    return TypedResults.Created($"/teams/{created.Id}", created.ToDto());
                }
                catch (DbUpdateException ex)
                {
                    return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            })
            .WithName("CreateTeam");

        group.MapPut("/teams/{id}",
            async Task<Results<Ok<TeamReadDto>, BadRequest<string>>> (TeamService teams, int id, TeamWriteDto team) =>
            {
                try
                {
                    var updated = await teams.UpdateTeam(team.ToEntity(id));
                    return TypedResults.Ok(updated.ToDto());
                }
                catch (DbUpdateException ex)
                {
                    return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            })
            .WithName("UpdateTeam");

        group.MapDelete("/teams/{id}",
            async (TeamService teams, int id) =>
            {
                await teams.DeleteProject(id);
                return TypedResults.NoContent();
            })
            .WithName("DeleteTeam");
    }
}