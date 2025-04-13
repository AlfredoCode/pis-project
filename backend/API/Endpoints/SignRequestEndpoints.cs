using Microsoft.AspNetCore.Http.HttpResults;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

public class SignRequestEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/signrequests/{id}", async Task<Results<Ok<SignRequest>, NotFound>> (SignRequestService requests, int id) =>
        {
            return (await requests.GetRequestById(id) is SignRequest request)
                ? TypedResults.Ok(request)
                : TypedResults.NotFound();
        }).WithName("GetSignRequestById");

        group.MapGet("/students/{studentId}/signrequests", async Task<IEnumerable<SignRequest>> (SignRequestService requests, int studentId) =>
        {
            return await requests.GetRequestsByStudent(studentId);
        }).WithName("GetRequestsByStudent");

        group.MapGet("/teams/{teamId}/signrequests", async Task<IEnumerable<SignRequest>> (SignRequestService requests, int teamId) =>
        {
            return await requests.GetRequestsByTeam(teamId);
        }).WithName("GetRequestsByTeam");

        group.MapPost("/signrequests", async Task<Created<SignRequest>> (SignRequestService requests, SignRequest request) =>
        {
            var created = await requests.SubmitSolution(request);
            return TypedResults.Created($"/signrequests/{created.Id}", created);
        }).WithName("SubmitSignRequest");

        group.MapPut("/signrequests/{id}/state", async (SignRequestService requests, int id, string newState) =>
        {
            await requests.UpdateRequestState(id, newState);
            return TypedResults.NoContent();
        }).WithName("UpdateSignRequestState");

        group.MapDelete("/signrequests/{id}", async (SignRequestService requests, int id) =>
        {
            await requests.DeleteRequest(id);
            return TypedResults.NoContent();
        }).WithName("DeleteSignRequest");
    }
}