using PRegSys.DAL.Entities;
using PRegSys.DAL.Enums;
using PRegSys.DAL.Repositories;

namespace PRegSys.BL.Services;

public class SignUpRequestService(
    SignUpRequestRepository signUpRequests,
    TeamRepository teams)
{
    public async Task<SignUpRequest?> GetRequestById(int id)
    {
        return await signUpRequests.GetRequestById(id);
    }

    public async Task<IEnumerable<SignUpRequest>> GetRequestsByStudent(int studentId)
    {
        return await signUpRequests.GetRequestsByStudent(studentId);
    }

    public async Task<IEnumerable<SignUpRequest>> GetRequestsByTeam(int teamId)
    {
        return await signUpRequests.GetRequestsByTeam(teamId);
    }

    public async Task<SignUpRequest> CreateRequest(SignUpRequest signUpRequest)
    {
        return await signUpRequests.CreateRequest(signUpRequest);
    }

    public async Task UpdateRequestState(int requestId, StudentSignUpState newState)
    {
        await signUpRequests.UpdateRequestState(requestId, newState);

        // add the student to the team if the request is accepted
        if (newState == StudentSignUpState.Approved
            && await signUpRequests.GetRequestById(requestId) is SignUpRequest request)
        {
            await teams.AddMember(request.TeamId, request.StudentId);
        }
    }

    public async Task DeleteRequest(int id)
    {
        await signUpRequests.DeleteRequest(id);
    }
}