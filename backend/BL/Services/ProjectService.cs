using PRegSys.DAL.Repositories;
using PRegSys.DAL.Entities;

namespace PRegSys.BL.Services;

public class ProjectService(ProjectRepository projects)
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
