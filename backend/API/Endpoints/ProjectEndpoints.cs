using System.ComponentModel;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using PRegSys.API.DTO;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;
using PRegSys.DAL.Repositories;

namespace PRegSys.API.Endpoints;

public class ProjectEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/projects",
            async Task<IEnumerable<ProjectViewDto>> (
                ProjectService projects,
                [Description("filter projects by which course they belong to")] string? course = null) =>
            {
                return course is null
                    ? (await projects.GetAllProjects()).ToDto()
                    : (await projects.GetProjectsInCourse(course)).ToDto();
            })
            .WithName("GetAllProjects");

        group.MapGet("/projects/{id}",
            async Task<Results<Ok<ProjectViewDto>, NotFound>> (ProjectService projects, int id) =>
            {
                return (await projects.GetProjectById(id) is ProjectView project)
                    ? TypedResults.Ok(project.ToDto())
                    : TypedResults.NotFound();
            })
            .WithName("GetProjectById");

        group.MapGet("/users/{userId}/projects",
            async Task<Results<Ok<IEnumerable<ProjectStudentViewDto>>, Ok<IEnumerable<ProjectViewDto>>, NotFound>>
                (UserService users, ProjectService projects, int userId) =>
            {
                switch (await users.GetUserById(userId))
                {
                    case Student student:
                    {
                        var projectsByUser = await projects.GetStudentViews(student);
                        return TypedResults.Ok(projectsByUser
                            .Select(p => new ProjectStudentViewDto(p.project, p.team, student)));
                    }
                    case Teacher teacher:
                    {
                        var ownedProjects = await projects.GetTeacherViews(teacher.Id);
                        return TypedResults.Ok(ownedProjects.ToDto());
                    }
                }
                return TypedResults.NotFound();
            })
            .WithName("GetProjectsByUserId")
            .WithDescription("""
                Gets all projects for a user. 
                If the user is a student, it gets all projects for the teams they are in. 
                If the user is a teacher, it gets all projects they own.
                """);

        group.MapGet("/users/{userId}/projects/{projectId}",
            async Task<Results<Ok<ProjectStudentViewDto>, Ok<ProjectViewDto>, NotFound>>
                (UserService users, ProjectService projects, int userId, int projectId) => {
                    switch (await users.GetUserById(userId))
                    {
                        case Student student:
                        {
                            return await projects.GetStudentView(student, projectId) is { } studentView
                                ? TypedResults.Ok(new ProjectStudentViewDto(studentView.project, studentView.team, student))
                                : TypedResults.NotFound();
                        }
                        case Teacher teacher:
                        {
                            var teacherView = await projects.GetTeacherView(teacher.Id, projectId);
                            return teacherView is not null
                                ? TypedResults.Ok(teacherView.ToDto())
                                : TypedResults.NotFound();
                        }
                    }
                    return TypedResults.NotFound();
                })
            .WithName("GetProjectByUserId");

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