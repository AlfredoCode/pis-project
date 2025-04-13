using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Repositories
{
    public class SignRequestRepository(PregsysDbContext db)
    {
        public async Task<IEnumerable<SignRequest>> GetRequestsByTeam(int teamId)
            => await db.SignRequests.Where(r => r.TeamId == teamId).ToListAsync();

        public async Task<IEnumerable<SignRequest>> GetRequestsByStudent(int studentId)
            => await db.SignRequests.Where(r => r.StudentId == studentId).ToListAsync();

        public async Task<SignRequest?> GetRequestById(int id)
            => await db.SignRequests.FindAsync(id);

        public async Task<SignRequest> CreateRequest(SignRequest request)
        {
            db.SignRequests.Add(request);
            await db.SaveChangesAsync();
            return request;
        }

        public async Task UpdateRequestState(int requestId, string newState)
        {
            var request = await db.SignRequests.FindAsync(requestId);
            if (request == null) return;
            request.State = newState;
            await db.SaveChangesAsync();
        }

        public async Task DeleteRequest(int id)
        {
            await db.SignRequests.Where(r => r.Id == id).ExecuteDeleteAsync();
        }
    }
}