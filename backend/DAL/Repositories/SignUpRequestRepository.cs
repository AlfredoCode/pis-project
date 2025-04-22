using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;
using PRegSys.DAL.Enums;

namespace PRegSys.DAL.Repositories;

public class SignUpRequestRepository(PregsysDbContext db)
{
    public async Task<IEnumerable<SignUpRequest>> GetRequestsByTeam(int teamId)
        => await db.SignRequests.Where(r => r.TeamId == teamId).ToListAsync();

    public async Task<IEnumerable<SignUpRequest>> GetRequestsByStudent(int studentId)
        => await db.SignRequests.Where(r => r.StudentId == studentId).ToListAsync();

    public async Task<SignUpRequest?> GetRequestById(int id)
        => await db.SignRequests.FindAsync(id);

    public async Task<SignUpRequest> CreateRequest(SignUpRequest upRequest)
    {
        db.SignRequests.Add(upRequest);
        await db.SaveChangesAsync();
        return upRequest;
    }

    public async Task UpdateRequestState(int requestId, StudentSignUpState newState)
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