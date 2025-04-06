using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

static class ProjectEndpoints
{
    public static RouteGroupBuilder MapProjectEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/projects/", async Task<IEnumerable<Project>>
            (ProjectService projects, string? course = null) => {
                return course is null
                    ? await projects.GetAllProjects()
                    : await projects.GetProjectsInCourse(course);
            })
            .WithName("GetAllProjects");

        group.MapGet("/projects/{id}", async Task<Results<Ok<Project>, NotFound>>
            (ProjectService projects, int id) => {
                return (await projects.GetProjectById(id) is Project project)
                    ? TypedResults.Ok(project)
                    : TypedResults.NotFound();
            })
            .WithName("GetProjectById");

        group.MapPost("/projects/", async Task<Results<Created<Project>, BadRequest<string>>>
            (ProjectService projects, Project project) => {
                try
                {
                    await projects.CreateProject(project);
                    return TypedResults.Created($"/projects/{project.Id}", project);
                }
                catch (DbUpdateException ex)
                {
                    return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            })
            .WithName("CreateProject");

        group.MapPut("/projects/{id}", async Task<Results<Ok<Project>, BadRequest<string>>>
            (ProjectService projects, int id, Project project) => {
                project.Id = id;
                try
                {
                    await projects.UpdateProject(project);
                    return TypedResults.Ok(project);
                }
                catch (DbUpdateException ex)
                {
                    return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            })
            .WithName("UpdateProject");

        group.MapDelete("/projects/{id}", async (ProjectService projects, int id) => {
            await projects.DeleteProject(id);
            return TypedResults.NoContent();
        })
            .WithName("DeleteProject");

        return group;
    }
}
