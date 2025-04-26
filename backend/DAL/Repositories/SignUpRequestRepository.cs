using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;
using PRegSys.DAL.Enums;

namespace PRegSys.DAL.Repositories;

public class SignUpRequestRepository(PregsysDbContext db)
{
    IQueryable<SignUpRequest> SignUpRequestsQuery => db.SignUpRequests
        .Include(r => r.Student)
        .Include(r => r.Team).ThenInclude(t => t.Leader)
        .Include(r => r.Team).ThenInclude(t => t.Project).ThenInclude(p => p.Owner);

    public async Task<IEnumerable<SignUpRequest>> GetRequestsByTeam(int teamId)
        => await SignUpRequestsQuery
            .Where(r => r.TeamId == teamId)
            .ToListAsync();

    public async Task<IEnumerable<SignUpRequest>> GetRequestsByStudent(int studentId)
        => await SignUpRequestsQuery
            .Where(r => r.StudentId == studentId)
            .ToListAsync();

    public async Task<SignUpRequest?> GetRequestById(int id)
        => await SignUpRequestsQuery
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<SignUpRequest> CreateRequest(SignUpRequest upRequest)
    {
        db.SignUpRequests.Add(upRequest);
        await db.SaveChangesAsync();
        return (await GetRequestById(upRequest.Id))!;
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