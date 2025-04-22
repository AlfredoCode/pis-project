using Microsoft.AspNetCore.Http.HttpResults;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;
using PRegSys.DAL.Enums;

namespace PRegSys.API.Endpoints;

public class SignUpRequestEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/signuprequests/{id}",
            async Task<Results<Ok<SignUpRequest>, NotFound>> (SignUpRequestService requests, int id) =>
            {
                return (await requests.GetRequestById(id) is SignUpRequest request)
                    ? TypedResults.Ok(request)
                    : TypedResults.NotFound();
            }).WithName("GetSignRequestById");

        group
            .MapGet("/students/{studentId}/signuprequests",
                async Task<IEnumerable<SignUpRequest>> (SignUpRequestService requests, int studentId) =>
                {
                    return await requests.GetRequestsByStudent(studentId);
                })
            .WithName("GetRequestsByStudent")
            .WithDescription("xxx");

        group.MapGet("/teams/{teamId}/signuprequests",
            async Task<IEnumerable<SignUpRequest>> (SignUpRequestService requests, int teamId) =>
            {
                return await requests.GetRequestsByTeam(teamId);
            }).WithName("GetRequestsByTeam");

        group.MapPost("/signuprequests",
            async Task<Created<SignUpRequest>> (SignUpRequestService requests, SignUpRequest request) =>
            {
                var created = await requests.CreateRequest(request);
                return TypedResults.Created($"/signrequests/{created.Id}", created);
            }).WithName("SubmitSignRequest");

        group.MapPut("/signuprequests/{id}/state",
            async (SignUpRequestService requests, int id, StudentSignUpState newState) =>
            {
                await requests.UpdateRequestState(id, newState);
                return TypedResults.NoContent();
            }).WithName("UpdateSignRequestState");

        group.MapDelete("/signuprequests/{id}", async (SignUpRequestService requests, int id) =>
        {
            await requests.DeleteRequest(id);
            return TypedResults.NoContent();
        }).WithName("DeleteSignRequest");
    }
}