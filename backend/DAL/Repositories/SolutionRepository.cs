using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Repositories;

public class SolutionRepository(PregsysDbContext db)
{
    IQueryable<Solution> SolutionsQuery => db.Solutions
        .Include(s => s.Team)
        .Include(s => s.Project).ThenInclude(p => p.Owner)
        .Include(s => s.Evaluation).ThenInclude(e => e!.Teacher);

    public async Task<IEnumerable<Solution>> GetSolutionsByProjectId(int projectId)
        => await SolutionsQuery
            .Where(s => s.ProjectId == projectId)
            .ToListAsync();

    public async Task<IEnumerable<Solution>> GetSolutionsByTeamId(int teamId)
        => await SolutionsQuery
            .Where(s => s.TeamId == teamId)
            .ToListAsync();

    public async Task<Solution?> GetSolutionById(int id)
        => await SolutionsQuery
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Solution> SubmitSolution(Solution solution)
    {
        db.Solutions.Add(solution);
        await db.SaveChangesAsync();
        return (await GetSolutionById(solution.Id))!;
    }

    public async Task DeleteSolution(int id)
    {
        await db.Solutions.Where(s => s.Id == id).ExecuteDeleteAsync();
    }
}
