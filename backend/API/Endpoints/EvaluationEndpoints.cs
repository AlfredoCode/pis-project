using Microsoft.AspNetCore.Http.HttpResults;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

public class EvaluationEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/evaluations/{id}", async Task<Results<Ok<Evaluation>, NotFound>> (EvaluationService evaluations, int id) =>
        {
            return (await evaluations.GetEvaluationById(id) is Evaluation eval)
                ? TypedResults.Ok(eval)
                : TypedResults.NotFound();
        }).WithName("GetEvaluationById");

        group.MapGet("/solutions/{solutionId}/evaluations", async Task<Evaluation?> (EvaluationService evaluations, int solutionId) =>
        {
            return await evaluations.GetEvaluationBySolutionId(solutionId);
        }).WithName("GetEvaluationsBySolutionId");

        group.MapPost("/evaluations", async Task<Created<Evaluation>> (EvaluationService evaluations, Evaluation eval) =>
        {
            var created = await evaluations.CreateEvaluation(eval);
            return TypedResults.Created($"/evaluations/{created.Id}", created);
        }).WithName("CreateEvaluation");

        group.MapDelete("/evaluations/{id}", async (EvaluationService evaluations, int id) =>
        {
            await evaluations.DeleteEvaluation(id);
            return TypedResults.NoContent();
        }).WithName("DeleteEvaluation");
    }
}