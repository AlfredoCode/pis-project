﻿using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Repositories;

public class EvaluationRepository(PregsysDbContext db)
{
    public async Task<Evaluation?> GetEvaluationBySolutionId(int solutionId)
        => await db.Solutions
            .Include(e => e.Evaluation!.Teacher)
            .Where(s => s.Id == solutionId)
            .Select(s => s.Evaluation!)
            .FirstOrDefaultAsync();

    public async Task<Evaluation?> GetEvaluationById(int id)
        => await db.Evaluations
            .Include(e => e.Teacher)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Evaluation> CreateEvaluation(Evaluation evaluation, int solutionId)
    {
        db.Evaluations.Add(evaluation);

        // add the evaluation to the solution
        if (await db.Solutions.FindAsync(solutionId) is Solution solution)
            solution.Evaluation = evaluation;

        await db.SaveChangesAsync();
        return (await GetEvaluationById(evaluation.Id))!;
    }

    public async Task DeleteEvaluation(int id)
    {
        await db.Evaluations.Where(e => e.Id == id).ExecuteDeleteAsync();
    }
}
