using NodaTime;

using PRegSys.DAL.Entities;

namespace PRegSys.API.DTO;

public class SolutionReadDto(Solution solution) : IReadDto<SolutionReadDto, Solution>
{
    public int Id { get; } = solution.Id;
    public byte[] File { get; } = solution.File;
    public string FileExtension { get; } = solution.FileExtension;
    public Instant SubmissionDate { get; } = solution.SubmissionDate;
    public int TeamId { get; } = solution.TeamId;
    public string TeamName { get; } = solution.Team.Name;
    public int ProjectId { get; } = solution.ProjectId;
    public string ProjectName { get; } = solution.Project.Name;
    public Instant ProjectDeadline { get; } = solution.Project.Deadline;
    public string ProjectCourse { get; } = solution.Project.Course;
    public int? EvaluationId { get; } = solution.EvaluationId;
    public string? EvaluatedBy { get; } = solution.Evaluation?.Teacher.FullName;
    public int? EvaluationPoints { get; } = solution.Evaluation?.Points;
    public string? EvaluationComment { get; } = solution.Evaluation?.Comment;

    public static SolutionReadDto FromEntity(Solution solution) => new(solution);
}

public class SolutionWriteDto : IWriteDto<SolutionWriteDto, Solution>
{
    public required byte[] File { get; set; }
    public required string FileExtension { get; set; }
    public required int TeamId { get; set; }
    public required int ProjectId { get; set; }

    public Solution ToEntity(IClock clock) => new() {
        Id = default,
        File = File,
        FileExtension = FileExtension,
        SubmissionDate = clock.GetCurrentInstant(),
        ProjectId = ProjectId,
        Project = null!,
        TeamId = TeamId,
        Team = null!,
        EvaluationId = null,
        Evaluation = null!,
    };
}

[GenerateDtoExtensions<Solution, SolutionReadDto, SolutionWriteDto>]
public static partial class SolutionDtoExtensions;
