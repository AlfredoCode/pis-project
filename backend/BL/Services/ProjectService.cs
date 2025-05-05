using PRegSys.DAL.Repositories;
using PRegSys.DAL.Entities;

namespace PRegSys.BL.Services;

public class ProjectService(ProjectRepository projects, TeamRepository teams)
{
    public async Task<IEnumerable<ProjectView>> GetAllProjects()
    {
        return await projects.GetAllProjectViews();
    }

    public async Task<ProjectView?> GetProjectById(int id)
    {
        return await projects.GetProjectViewById(id);
    }

    public async Task<IEnumerable<ProjectView>> GetProjectsInCourse(string course)
    {
        return await projects.GetProjectViewsInCourse(course);
    }

    public async Task<IEnumerable<ProjectView>> GetProjectsByOwnerId(int userId)
    {
        return await projects.GetProjectViewsByOwnerId(userId);
    }

    public async Task<IEnumerable<(Project project, Team? team)>> GetStudentViews(Student student)
    {
        return (await teams.GetTeamsByStudentId(student.Id, includeSolutions: true))
            .Select(t => (t.Project, (Team?)t));
    }

    public async Task<(Project project, Team? team)?> GetStudentView(Student student, int projectId)
    {
        if (await teams.GetStudentTeamForProject(student.Id, projectId, includeSolutions: true) is not { } team)
            return null;

        return (team.Project, team);
    }

    public async Task<ProjectView?> GetTeacherView(int teacherId, int projectId)
    {
        return await projects.GetProjectView(teacherId, projectId);
    }

    public async Task<IEnumerable<ProjectView>> GetTeacherViews(int teacherId)
    {
        return await projects.GetProjectViewsByOwnerId(teacherId);
    }

    public async Task<Project> CreateProject(Project project)
    {
        return await projects.CreateProject(project);
    }

    public async Task<Project> UpdateProject(Project project)
    {
        return await projects.UpdateProject(project);
    }

    public async Task DeleteProject(int id)
    {
        await projects.DeleteProject(id);
    }
}
