using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRegSys.DAL.Repositories
{
    public class SolutionRepository(PregsysDbContext db)
    {
        public async Task<IEnumerable<Solution>> GetSolutionsByProjectId(int projectId)
            => await db.Solutions.Where(s => s.ProjectId == projectId).ToListAsync();

        public async Task<IEnumerable<Solution>> GetSolutionsByTeamId(int teamId)
            => await db.Solutions.Where(s => s.TeamId == teamId).ToListAsync();

        public async Task<Solution?> GetSolutionById(int id)
            => await db.Solutions.FindAsync(id);

        public async Task<Solution> SubmitSolution(Solution solution)
        {
            db.Solutions.Add(solution);
            await db.SaveChangesAsync();
            return solution;
        }

        public async Task DeleteSolution(int id)
        {
            await db.Solutions.Where(s => s.Id == id).ExecuteDeleteAsync();
        }
    }
}
