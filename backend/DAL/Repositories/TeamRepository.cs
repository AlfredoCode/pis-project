using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Repositories;

public class TeamRepository(PregsysDbContext db)
{
    IQueryable<Team> TeamsQuery => db.Teams
        .Include(t => t.Leader)
        .Include(t => t.Project).ThenInclude(t => t.Owner);

    public async Task<IEnumerable<Team>> GetAllTeams()
        => await TeamsQuery
            .ToListAsync();

    public async Task<IEnumerable<Team>> GetTeamsByProjectId(int projectId)
        => await TeamsQuery
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();

    public async Task<IEnumerable<Team>> GetTeamsByStudentId(int studentId)
        => await TeamsQuery
            .Where(t => t.Students.Any(s => s.Id == studentId))
            .ToListAsync();

    public async Task<Team?> GetStudentTeamForProject(int studentId, int projectId)
    {
        return await TeamsQuery
            .Where(t => t.ProjectId == projectId && t.Students.Any(s => s.Id == studentId))
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Student>?> GetStudentsInTeam(int teamId)
    {
        if (await db.Teams
            .Include(t => t.Students)
            .FirstOrDefaultAsync(t => t.Id == teamId) is Team team)
        {
            return team.Students;
        }
        return null;
    }

    public async Task<Team?> GetTeamById(int id)
        => await TeamsQuery
            .Include(t => t.Students)
            .Include(t => t.SignUpRequests)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Team> CreateTeam(Team team)
    {
        db.Teams.Add(team);
        await db.SaveChangesAsync();
        return (await GetTeamById(team.Id))!;
    }

    public async Task AddMember(int teamId, int studentId)
    {
        if (await db.Teams.FindAsync(teamId) is Team team
            && await db.Students.FindAsync(studentId) is Student student)
        {
            team.Students.Add(student);
            db.SaveChanges();
        }
    }

    public async Task<Team> UpdateTeam(Team team)
    {
        db.Teams.Update(team);
        await db.SaveChangesAsync();
        return (await GetTeamById(team.Id))!;
    }

    public async Task DeleteTeam(int id)
    {
        await db.Teams.Where(t => t.Id == id).ExecuteDeleteAsync();
    }
}