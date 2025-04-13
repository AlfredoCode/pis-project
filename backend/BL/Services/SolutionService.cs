using PRegSys.DAL.Entities;
using PRegSys.DAL.Repositories;

namespace PRegSys.BL.Services
{
    public class SolutionService(SolutionRepository solutions)
    {
        public async Task<Solution?> GetSolutionById(int id)
        {
            return await solutions.GetSolutionById(id);
        }

        public async Task<IEnumerable<Solution>> GetSolutionsByProjectId(int projectId)
        {
            return await solutions.GetSolutionsByProjectId(projectId);
        }

        public async Task<IEnumerable<Solution>> GetSolutionsByTeamId(int teamId)
        {
            return await solutions.GetSolutionsByTeamId(teamId);
        }

        public async Task<Solution> SubmitSolution(Solution solution)
        {
            return await solutions.SubmitSolution(solution);
        }

        public async Task DeleteSolution(int id)
        {
            await solutions.DeleteSolution(id);
        }
    }
}