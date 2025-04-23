using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;
using PRegSys.DAL.Enums;

namespace PRegSys.DAL.Repositories;

public class SignUpRequestRepository(PregsysDbContext db)
{
    public async Task<IEnumerable<SignUpRequest>> GetRequestsByTeam(int teamId)
        => await db.SignUpRequests.Where(r => r.TeamId == teamId).ToListAsync();

    public async Task<IEnumerable<SignUpRequest>> GetRequestsByStudent(int studentId)
        => await db.SignUpRequests.Where(r => r.StudentId == studentId).ToListAsync();

    public async Task<SignUpRequest?> GetRequestById(int id)
        => await db.SignUpRequests.FindAsync(id);

    public async Task<SignUpRequest> CreateRequest(SignUpRequest upRequest)
    {
        db.SignUpRequests.Add(upRequest);
        await db.SaveChangesAsync();
        return upRequest;
    }

    public async Task UpdateRequestState(int requestId, StudentSignUpState newState)
    {
        var request = await db.SignUpRequests.FindAsync(requestId);
        if (request == null) return;
        request.State = newState;
        await db.SaveChangesAsync();
    }

    public async Task DeleteRequest(int id)
    {
        await db.SignUpRequests.Where(r => r.Id == id).ExecuteDeleteAsync();
    }
}