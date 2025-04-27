﻿using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Repositories;

public class TeamRepository(PregsysDbContext db)
{
    IQueryable<Team> TeamsQuery(bool includeSolutions = false) =>
        (includeSolutions
            ? db.Teams.Include(t => t.Solutions!).ThenInclude(s => s!.Evaluation)
            : db.Teams.AsQueryable())
        .Include(t => t.Leader)
        .Include(t => t.Project).ThenInclude(t => t.Owner);

    public async Task<IEnumerable<Team>> GetAllTeams()
        => await TeamsQuery()
            .ToListAsync();

    public async Task<IEnumerable<Team>> GetTeamsByProjectId(int projectId)
        => await TeamsQuery()
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();

    public async Task<IEnumerable<Team>> GetTeamsByStudentId(int studentId, bool includeSolutions = false)
        => await TeamsQuery(includeSolutions)
            .Where(t => t.Students.Any(s => s.Id == studentId))
            .ToListAsync();

    public async Task<Team?> GetStudentTeamForProject(int studentId, int projectId, bool includeSolutions = false)
    {
        return await TeamsQuery(includeSolutions)
            .Where(t => t.ProjectId == projectId && t.Students.Any(s => s.Id == studentId))
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Student>?> GetStudentsInTeam(int teamId)
    {
        if (await db.Teams
            .Include(t => t.Students)
            .SingleOrDefaultAsync(t => t.Id == teamId) is Team team)
        {
            return team.Students;
        }
        return null;
    }

    public async Task<Team?> GetTeamById(int id, bool includeSolutions = false)
        => await TeamsQuery(includeSolutions)
            .Include(t => t.Students)
            .Include(t => t.SignUpRequests)
            .SingleOrDefaultAsync(t => t.Id == id);

    public async Task<Team> CreateTeam(Team team)
    {
        db.Teams.Add(team);
        await db.SaveChangesAsync();
        return (await GetTeamById(team.Id))!;
    }

    public async Task<bool> AddMember(Team team, int studentId)
    {
        if (await db.Students.FindAsync(studentId) is Student student)
        {
            team.Students.Add(student);
            await db.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> RemoveMember(Team team, int studentId)
    {
        if (team.Students.FirstOrDefault(s => s.Id == studentId) is Student student)
        {
            bool removed = team.Students.Remove(student);
            await db.SaveChangesAsync();
            return removed;
        }
        return false;
    }

    public async Task<Team> UpdateTeam(Team team)
    {
        var existingTeam = await db.Teams.FindAsync(team.Id);
        if (existingTeam is null)
            throw new InvalidOperationException("The given team does not exist");

        existingTeam.Name = team.Name;
        existingTeam.Description = team.Description;
        existingTeam.LeaderId = team.LeaderId;
        existingTeam.ProjectId = team.ProjectId;

        db.Teams.Update(existingTeam);
        await db.SaveChangesAsync();
        return (await GetTeamById(team.Id))!;
    }

    public async Task DeleteTeam(int id)
    {
        await db.Teams.Where(t => t.Id == id).ExecuteDeleteAsync();
    }
}