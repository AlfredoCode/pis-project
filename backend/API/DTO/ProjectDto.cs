using NodaTime;

using PRegSys.DAL.Entities;
using PRegSys.DAL.Repositories;

namespace PRegSys.API.DTO;

public class ProjectReadDto(Project project) : IReadDto<ProjectReadDto, Project>
{
    public int Id { get; } = project.Id;
    public string Name { get; } = project.Name;
    public string Course { get; } = project.Course;
    public string Description { get; } = project.Description;
    public int MaxTeamSize { get; } = project.MaxTeamSize;
    public int Capacity { get; } = project.Capacity;
    public Instant Deadline { get; } = project.Deadline;
    public UserReadDto Owner { get; } = project.Owner.ToDto();

    public static ProjectReadDto FromEntity(Project project) => new(project);
}

public class ProjectStudentViewDto(Project project, Team? team, Student student) : ProjectReadDto(project)
{
    public TeamReadDto? Team { get; } = team?.ToDto();
    public StudentReadDto Student { get; } = student.ToDto();
}

public class ProjectTeacherViewDto(ProjectTeacherView ptv) : ProjectReadDto(ptv.Project)
{
    public int RegisteredTeams { get; } = ptv.RegisteredTeams;
    public int TeamsWithSubmissions { get; } = ptv.TeamsWithSubmissions;
}

public class ProjectWriteDto : IWriteDto<ProjectWriteDto, Project>
{
    public required string Name { get; set; }
    public required string Course { get; set; }
    public required string Description { get; set; }
    public required int MaxTeamSize { get; set; }
    public required int Capacity { get; set; }
    public required Instant Deadline { get; set; }
    public required int OwnerId { get; set; }

    public Project ToEntity(int id) => new()
    {
        Id = id,
        Name = Name,
        Course = Course,
        Description = Description,
        MaxTeamSize = MaxTeamSize,
        Capacity = Capacity,
        Deadline = Deadline,
        OwnerId = OwnerId,
        Owner = null!,
        Teams = null!,
    };
}

[GenerateDtoExtensions<Project, ProjectReadDto, ProjectWriteDto>]
public static partial class ProjectDtoExtensions;
