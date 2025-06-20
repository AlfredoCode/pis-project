using PRegSys.DAL.Entities;
using PRegSys.DAL.Enums;
using PRegSys.DAL.Repositories;

namespace PRegSys.BL.Services;

public class SignUpRequestService(
    SignUpRequestRepository signUpRequests,
    UserRepository users,
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
        if ((await teams.GetTeamById(request.TeamId)) is not Team team)
            throw new InvalidOperationException("The specified team does not exist");

        if (team.Students.Any(s => s.Id == request.StudentId))
        {
            throw new InvalidOperationException("The student is already in this team");
        }

        if (await teams.GetStudentTeamForProject(request.StudentId, team.ProjectId) is Team existingTeam)
        {
            throw new InvalidOperationException(
                $"This student is already in a team ('{existingTeam.Name}') for this project");
        }

        if (team.SignUpRequests.Any(r => r.StudentId == request.StudentId
                                    && r.State == StudentSignUpState.Created))
        {
            throw new InvalidOperationException("The student has already requested to join this team");
        }

        if (await users.GetUserById(request.StudentId) is not Student student)
        {
            throw new InvalidOperationException("The given user does not exist or is not a student");
        }

        request.State = StudentSignUpState.Created;
        return await signUpRequests.CreateRequest(request);
    }

    public async Task<bool> UpdateRequestState(int requestId, StudentSignUpState newState)
    {
        if (await signUpRequests.GetRequestById(requestId) is not SignUpRequest request)
            return false;

        var students = await teams.GetStudentsInTeam(request.TeamId);

        // check if there is enough space in the team for the student
        if (students?.Count() >= request.Team.Project.MaxTeamSize)
        {
            throw new InvalidOperationException($"The team ('{request.Team.Name}') is already full");
        }

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

            var otherSignUpRequestsForProject =
                (await signUpRequests.GetRequestsByStudent(request.StudentId)).Where(x =>
                    x.Team.ProjectId == request.Team.ProjectId && x.Id != requestId);

            // reject all other requests for the same project
            foreach (var otherRequest in otherSignUpRequestsForProject)
            {
                await signUpRequests.UpdateRequestState(otherRequest.Id, StudentSignUpState.Rejected);
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