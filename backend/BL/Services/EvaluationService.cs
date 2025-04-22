using PRegSys.DAL.Entities;
using PRegSys.DAL.Repositories;

namespace PRegSys.BL.Services;

public class EvaluationService(EvaluationRepository evaluations)
{
    public async Task<Evaluation?> GetEvaluationById(int id)
    {
        return await evaluations.GetEvaluationById(id);
    }

    public async Task<Evaluation?> GetEvaluationBySolutionId(int solutionId)
    {
        return await evaluations.GetEvaluationBySolutionId(solutionId);
    }

    public async Task<Evaluation> CreateEvaluation(Evaluation evaluation)
    {
        return await evaluations.CreateEvaluation(evaluation);
    }

    public async Task DeleteEvaluation(int id)
    {
        await evaluations.DeleteEvaluation(id);
    }
}