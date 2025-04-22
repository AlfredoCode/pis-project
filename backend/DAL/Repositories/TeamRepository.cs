using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Repositories;

public class TeamRepository(PregsysDbContext db)
{
    public async Task<IEnumerable<Team>> GetAllTeams() => await db.Teams.ToListAsync();

    public async Task<IEnumerable<Team>> GetTeamsByProjectId(int projectId)
        => await db.Teams.Where(t => t.ProjectId == projectId).ToListAsync();

    public async Task<IEnumerable<Team>> GetTeamsByStudentId(int studentId)
        => await db.Teams.Where(t => t.Students.Any(s => s.Id == studentId)).ToListAsync();

    public async Task<Team?> GetTeamById(int id) => await db.Teams.FindAsync(id);

    public async Task<Team> CreateTeam(Team team)
    {
        db.Teams.Add(team);
        await db.SaveChangesAsync();
        return team;
    }

    public async Task<Team> UpdateTeam(Team team)
    {
        db.Teams.Update(team);
        await db.SaveChangesAsync();
        return team;
    }

    public async Task DeleteTeam(int id)
    {
        await db.Teams.Where(t => t.Id == id).ExecuteDeleteAsync();
    }
}