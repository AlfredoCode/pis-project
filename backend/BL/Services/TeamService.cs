using PRegSys.DAL.Entities;
using PRegSys.DAL.Repositories;

namespace PRegSys.BL.Services
{
    public class TeamService(TeamRepository teams)
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

        public async Task<Team> CreateTeam(Team team)
        {
            return await teams.CreateTeam(team);
        }

        public async Task<Team> UpdateTeam(Team team)
        {
            return await teams.UpdateTeam(team);
        }

        public async Task DeleteProject(int id)
        {
            await teams.DeleteTeam(id);
        }
    }
}
