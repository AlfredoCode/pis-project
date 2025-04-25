using Microsoft.EntityFrameworkCore;

using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Repositories;

public class ProjectRepository(PregsysDbContext db)
{
    IQueryable<Project> ProjectsQuery => db.Projects
        .Include(p => p.Owner);

    public async Task<IEnumerable<Project>> GetAllProjects()
        => await ProjectsQuery.ToListAsync();

    public async Task<Project?> GetProjectById(int id)
        => await ProjectsQuery.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Project>> GetProjectsInCourse(string course)
    {
        return await ProjectsQuery
            .Where(p => p.Course == course)
            .ToListAsync();
    }

    public async Task<Project> CreateProject(Project project)
    {
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return (await GetProjectById(project.Id))!;
    }

    public async Task<Project> UpdateProject(Project project)
    {
        db.Projects.Update(project);
        await db.SaveChangesAsync();
        return (await GetProjectById(project.Id))!;
    }

    public async Task DeleteProject(int id)
    {
        await db.Projects.Where(p => p.Id == id).ExecuteDeleteAsync();
    }
}
