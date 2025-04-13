using PRegSys.DAL.Entities;
using PRegSys.DAL.Repositories;

namespace PRegSys.BL.Services
{
    public class SignRequestService(SignRequestRepository signRequests)
    {
        public async Task<SignRequest?> GetRequestById(int id)
        {
            return await signRequests.GetRequestById(id);
        }

        public async Task<IEnumerable<SignRequest>> GetRequestsByStudent(int studentId)
        {
            return await signRequests.GetRequestsByStudent(studentId);
        }

        public async Task<IEnumerable<SignRequest>> GetRequestsByTeam(int teamId)
        {
            return await signRequests.GetRequestsByTeam(teamId);
        }

        public async Task<SignRequest> SubmitSolution(SignRequest signRequest)
        {
            return await signRequests.CreateRequest(signRequest);
        }

        public async Task UpdateRequestState(int requestId, string newState)
        {
            await signRequests.UpdateRequestState(requestId, newState);
        }

        public async Task DeleteRequest(int id)
        {
            await signRequests.DeleteRequest(id);
        }
    }
}