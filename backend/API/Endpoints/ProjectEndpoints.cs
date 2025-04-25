using System.ComponentModel;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using PRegSys.API.DTO;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

public class ProjectEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/projects",
            async Task<IEnumerable<ProjectReadDto>> (
                ProjectService projects,
                [Description("filter projects by which course they belong to")] string? course = null) =>
            {
                return course is null
                    ? (await projects.GetAllProjects()).ToDto()
                    : (await projects.GetProjectsInCourse(course)).ToDto();
            })
            .WithName("GetAllProjects");

        group.MapGet("/projects/{id}",
            async Task<Results<Ok<ProjectReadDto>, NotFound>> (ProjectService projects, int id) =>
            {
                return (await projects.GetProjectById(id) is Project project)
                    ? TypedResults.Ok(project.ToDto())
                    : TypedResults.NotFound();
            })
            .WithName("GetProjectById");

        group.MapPost("/projects",
            async Task<Results<Created<ProjectReadDto>, BadRequest<string>>>
                (ProjectService projects, ProjectWriteDto project) =>
            {
                try
                {
                    var createdProject = await projects.CreateProject(project.ToEntity(default));
                    return TypedResults.Created($"/projects/{createdProject.Id}", createdProject.ToDto());
                }
                catch (DbUpdateException ex)
                {
                    return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            })
            .WithName("CreateProject");

        group.MapPut("/projects/{id}",
            async Task<Results<Ok<ProjectReadDto>, BadRequest<string>>>
                (ProjectService projects, int id, ProjectWriteDto project) =>
            {
                try
                {
                    var updatedProject = await projects.UpdateProject(project.ToEntity(id));
                    return TypedResults.Ok(updatedProject.ToDto());
                }
                catch (DbUpdateException ex)
                {
                    return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            })
            .WithName("UpdateProject");

        group.MapDelete("/projects/{id}",
            async (ProjectService projects, int id) =>
            {
                await projects.DeleteProject(id);
                return TypedResults.NoContent();
            })
            .WithName("DeleteProject");
    }
}