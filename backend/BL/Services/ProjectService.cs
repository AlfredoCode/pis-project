using PRegSys.DAL.Repositories;
using PRegSys.DAL.Entities;

namespace PRegSys.BL.Services;

public class ProjectService(ProjectRepository projects, TeamRepository teams)
{
    public async Task<IEnumerable<Project>> GetAllProjects()
    {
        return await projects.GetAllProjects();
    }

    public async Task<Project?> GetProjectById(int id)
    {
        return await projects.GetProjectById(id);
    }

    public async Task<IEnumerable<Project>> GetProjectsInCourse(string course)
    {
        return await projects.GetProjectsInCourse(course);
    }

    public async Task<IEnumerable<Project>> GetProjectsByOwnerId(int userId)
    {
        return await projects.GetProjectsByOwnerId(userId);
    }

    public async Task<IEnumerable<(Project project, Team? team)>> GetStudentViews(Student student)
    {
        return (await teams.GetTeamsByStudentId(student.Id, includeSolution: true))
            .Select(t => (t.Project, (Team?)t));
    }

    public async Task<(Project project, Team? team)?> GetStudentView(Student student, int projectId)
    {
        if (await teams.GetStudentTeamForProject(student.Id, projectId, includeSolution: true) is not { } team)
            return null;

        return (team.Project, team);
    }

    public async Task<ProjectTeacherView?> GetTeacherView(int teacherId, int projectId)
    {
        return await projects.GetProjectTeacherView(teacherId, projectId);
    }

    public async Task<IEnumerable<ProjectTeacherView>> GetTeacherViews(int teacherId)
    {
        return await projects.GetProjectTeacherViews(teacherId);
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
