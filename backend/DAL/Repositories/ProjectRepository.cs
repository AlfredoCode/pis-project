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

    public async Task<IEnumerable<ProjectView>> GetAllProjectViews()
        => await GetProjectViewQuery(ProjectsQuery).ToListAsync();

    public async Task<ProjectView?> GetProjectViewById(int id)
        => await GetProjectViewQuery(ProjectsQuery.Where(p => p.Id == id))
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Project>> GetProjectsInCourse(string course)
    {
        return await ProjectsQuery
            .Where(p => p.Course == course)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectView>> GetProjectViewsInCourse(string course)
    {
        return await GetProjectViewQuery(ProjectsQuery
                .Where(p => p.Course == course))
            .ToListAsync();
    }

    public async Task<ProjectView?> GetProjectView(int teacherId, int projectId)
    {
        return await GetProjectViewQuery(ProjectsQuery
                .Where(p => p.OwnerId == teacherId && p.Id == projectId))
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<ProjectView>> GetProjectViewsByOwnerId(int teacherId)
    {
        return await GetProjectViewQuery(ProjectsQuery
                .Where(p => p.OwnerId == teacherId))
            .ToListAsync();
    }

    IQueryable<ProjectView> GetProjectViewQuery(IQueryable<Project> projects) =>
        from p in projects
        let registeredTeams = p.Teams.Count
        let teamsWithSubmissions = p.Teams.Count(t => t.Solutions!.Any())
        select new ProjectView(p, registeredTeams, teamsWithSubmissions);

    public async Task<IEnumerable<Project>> GetProjectsByOwnerId(int id)
    {
        return await ProjectsQuery
            .Where(p => p.OwnerId == id)
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

public record ProjectView(Project Project, int RegisteredTeams, int TeamsWithSubmissions);
