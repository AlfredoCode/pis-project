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

    public async Task<ProjectTeacherView?> GetProjectTeacherView(int teacherId, int projectId)
    {
        return await GetProjectTeacherViewQuery(ProjectsQuery
                .Where(p => p.OwnerId == teacherId && p.Id == projectId))
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<ProjectTeacherView>> GetProjectTeacherViews(int teacherId)
    {
        return await GetProjectTeacherViewQuery(ProjectsQuery
                .Where(p => p.OwnerId == teacherId))
            .ToListAsync();
    }

    IQueryable<ProjectTeacherView> GetProjectTeacherViewQuery(IQueryable<Project> projects) =>
        from p in projects
            .Include(p => p.Teams).ThenInclude(t => t.Solution).ThenInclude(s => s!.Evaluation).ThenInclude(e => e!.Teacher)
        let registeredTeams = db.Teams.Count(t => t.ProjectId == p.Id)
        let teamsWithSubmissions = db.Teams.Count(t => t.ProjectId == p.Id && t.Solution != null)
        select new ProjectTeacherView(p, registeredTeams, teamsWithSubmissions);

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

public record ProjectTeacherView(Project Project, int RegisteredTeams, int TeamsWithSubmissions);
