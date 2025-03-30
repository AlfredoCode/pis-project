using Microsoft.EntityFrameworkCore;

using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Repositories;

public class ProjectRepository(PregsysDbContext db)
{
    public async Task<IEnumerable<Project>> GetAllProjects() => await db.Projects.ToListAsync();
    public async Task<Project?> GetProjectById(int id) => await db.Projects.FindAsync(id);

    public async Task<IEnumerable<Project>> GetProjectsInCourse(string course)
    {
        return await db.Projects.Where(p => p.Course == course).ToListAsync();
    }

    public async Task<Project> CreateProject(Project project)
    {
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateProject(Project project)
    {
        db.Projects.Update(project);
        await db.SaveChangesAsync();
        return project;
    }

    public async Task DeleteProject(int id)
    {
        await db.Projects.Where(p => p.Id == id).ExecuteDeleteAsync();
    }
}
