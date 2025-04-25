using PRegSys.DAL.Entities;
using PRegSys.DAL.Repositories;

namespace PRegSys.BL.Services;

public class TeamService(TeamRepository teams, UserRepository users)
{
    public async Task<IEnumerable<Team>> GetAllTeams()
    {
        return await teams.GetAllTeams();
    }

    public async Task<Team?> GetTeamById(int id)
    {
        return await teams.GetTeamById(id);
    }

    public async Task<IEnumerable<Team>> GetTeamsByProjectId(int projectId)
    {
        return await teams.GetTeamsByProjectId(projectId);
    }

    public async Task<IEnumerable<Team>> GetTeamsByStudentId(int studentId)
    {
        return await teams.GetTeamsByStudentId(studentId);
    }

    public async Task<IEnumerable<Student>?> GetStudentsInTeam(int teamId)
    {
        return await teams.GetStudentsInTeam(teamId);
    }

    public async Task<Team> CreateTeam(Team team)
    {
        if (!team.Students.Any(s => s.Id == team.LeaderId))
        {
            User? leader = team.Leader ?? await users.GetUserById(team.LeaderId);
            if (leader is null)
                throw new InvalidOperationException("The given team leader user does not exist");

            if (leader is not Student leaderStudent)
                throw new InvalidOperationException("The given team leader is not a student");

            team.Students.Add(leaderStudent);
        }

        return await teams.CreateTeam(team);
    }

    public async Task<Team> UpdateTeam(Team team)
    {
        if (!team.Students.Any(s => s.Id == team.LeaderId))
        {
            throw new InvalidOperationException("The new team leader is not a member of the team");
        }

        return await teams.UpdateTeam(team);
    }

    public async Task DeleteProject(int id)
    {
        await teams.DeleteTeam(id);
    }
}
