using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Repositories;

public class EvaluationRepository(PregsysDbContext db)
{
    public async Task<Evaluation?> GetEvaluationBySolutionId(int solutionId)
        => await db.Solutions
            .Where(s => s.Id == solutionId)
            .Select(s => s.Evaluation!)
            .Include(e => e.Teacher)
            .FirstOrDefaultAsync();

    public async Task<Evaluation?> GetEvaluationById(int id)
        => await db.Evaluations
            .Include(e => e.Teacher)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Evaluation> CreateEvaluation(Evaluation evaluation)
    {
        db.Evaluations.Add(evaluation);
        await db.SaveChangesAsync();
        return (await GetEvaluationById(evaluation.Id))!;
    }

    public async Task DeleteEvaluation(int id)
    {
        await db.Evaluations.Where(e => e.Id == id).ExecuteDeleteAsync();
    }
}
