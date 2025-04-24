using Microsoft.AspNetCore.Http.HttpResults;

using NodaTime;

using PRegSys.API.DTO;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;
using PRegSys.DAL.Enums;

namespace PRegSys.API.Endpoints;

public class SignUpRequestEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/signuprequests/{id}",
            async Task<Results<Ok<SignUpRequestReadDto>, NotFound>> (SignUpRequestService requests, int id) =>
            {
                return (await requests.GetRequestById(id) is SignUpRequest request)
                    ? TypedResults.Ok(request.ToDto())
                    : TypedResults.NotFound();
            })
            .WithName("GetSignUpRequestById");

        group
            .MapGet("/students/{studentId}/signuprequests",
                async Task<IEnumerable<SignUpRequestReadDto>> (SignUpRequestService requests, int studentId) =>
                {
                    return (await requests.GetRequestsByStudent(studentId)).ToDto();
                })
            .WithName("GetRequestsByStudent");

        group.MapGet("/teams/{teamId}/signuprequests",
            async Task<IEnumerable<SignUpRequestReadDto>> (SignUpRequestService requests, int teamId) =>
            {
                return (await requests.GetRequestsByTeam(teamId)).ToDto();
            })
            .WithName("GetRequestsByTeam");

        group.MapPost("/signuprequests",
            async Task<Created<SignUpRequestReadDto>> (
                SignUpRequestService requests, SignUpRequestWriteDto request, IClock clock) =>
            {
                var created = await requests.CreateRequest(request.ToEntity(clock));
                return TypedResults.Created($"/signuprequests/{created.Id}", created.ToDto());
            })
            .WithName("SubmitSignUpRequest");

        group.MapPut("/signuprequests/{id}/state",
            async (SignUpRequestService requests, int id, StudentSignUpState newState) =>
            {
                await requests.UpdateRequestState(id, newState);
                return TypedResults.NoContent();
            })
            .WithName("UpdateSignUpRequestState");

        group.MapDelete("/signuprequests/{id}",
            async (SignUpRequestService requests, int id) =>
            {
                await requests.DeleteRequest(id);
                return TypedResults.NoContent();
            })
            .WithName("DeleteSignUpRequest");
    }
}