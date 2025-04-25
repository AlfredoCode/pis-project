using PRegSys.DAL.Entities;

namespace PRegSys.API.DTO;

public class TeamReadDto(Team team) : IReadDto<TeamReadDto, Team>
{
    public int Id { get; } = team.Id;
    public string Name { get; } = team.Name;
    public string Description { get; } = team.Description;
    public UserReadDto Leader { get; } = team.Leader.ToDto();
    public ProjectReadDto Project { get; } = team.Project.ToDto();

    public static TeamReadDto FromEntity(Team team) => new(team);
}

public class TeamWriteDto : IWriteDto<TeamWriteDto, Team>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int LeaderId { get; set; }
    public required int ProjectId { get; set; }

    public Team ToEntity(int id) => new()
    {
        Id = id,
        Name = Name,
        Description = Description,
        LeaderId = LeaderId,
        ProjectId = ProjectId,
        Leader = null!,
        Project = null!,
    };
}

[GenerateDtoExtensions<Team, TeamReadDto, TeamWriteDto>]
public static partial class TeamDtoExtensions;
