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

    public async Task<SignUpRequest> CreateRequest(SignUpRequest request)
    {
        if ((request.Team ?? await teams.GetTeamById(request.TeamId)) is not Team team)
            throw new InvalidOperationException("the specified team does not exist");

        if (await teams.GetStudentTeamForProject(request.StudentId, team.ProjectId) is Team existingTeam)
        {
            throw new InvalidOperationException(
                $"This student is already in a team ('{existingTeam.Name}') for this project");
        }

        return await signUpRequests.CreateRequest(request);
    }

    public async Task<bool> UpdateRequestState(int requestId, StudentSignUpState newState)
    {
        if (await signUpRequests.GetRequestById(requestId) is not SignUpRequest request)
            return false;

        // TODO: check if the team leader is the one who is accepting/rejecting the request
        // TODO: check if there is enough space in the team for the student

        await signUpRequests.UpdateRequestState(requestId, newState);

        // add the student to the team if the request is accepted
        if (newState == StudentSignUpState.Approved)
        {
            // check if the student is already in some other team for the same project
            if (await teams.GetStudentTeamForProject(request.StudentId, request.Team.ProjectId) is Team existingTeam)
            {
                throw new InvalidOperationException(
                    $"This student is already in a team ('{existingTeam.Name}') for this project");
            }

            await teams.AddMember(request.Team, request.StudentId);
        }

        return true;
    }

    public async Task DeleteRequest(int id)
    {
        await signUpRequests.DeleteRequest(id);
    }
}