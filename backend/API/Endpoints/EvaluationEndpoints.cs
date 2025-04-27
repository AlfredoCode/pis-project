using Microsoft.AspNetCore.Http.HttpResults;

using PRegSys.API.DTO;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

public class EvaluationEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/evaluations/{id}",
            async Task<Results<Ok<EvaluationReadDto>, NotFound>> (EvaluationService evaluations, int id) =>
            {
                return (await evaluations.GetEvaluationById(id) is Evaluation eval)
                    ? TypedResults.Ok(eval.ToDto())
                    : TypedResults.NotFound();
            })
            .WithName("GetEvaluationById");

        group.MapGet("/solutions/{solutionId}/evaluation",
            async Task<EvaluationReadDto?> (EvaluationService evaluations, int solutionId) =>
            {
                return (await evaluations.GetEvaluationBySolutionId(solutionId))?.ToDto();
            })
            .WithName("GetEvaluationBySolutionId");

        group.MapPost("/evaluations",
            async Task<Created<EvaluationReadDto>> (EvaluationService evaluations, EvaluationWriteDto eval) =>
            {
                var created = await evaluations.CreateEvaluation(eval.ToEntity(), eval.SolutionId);
                return TypedResults.Created($"/evaluations/{created.Id}", created.ToDto());
            })
            .WithName("CreateEvaluation");

        group.MapDelete("/evaluations/{id}",
            async (EvaluationService evaluations, int id) =>
            {
                await evaluations.DeleteEvaluation(id);
                return TypedResults.NoContent();
            })
            .WithName("DeleteEvaluation");
    }
}