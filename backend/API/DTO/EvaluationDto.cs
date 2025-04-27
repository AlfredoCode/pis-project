using PRegSys.DAL.Entities;

namespace PRegSys.API.DTO;

public class EvaluationReadDto(Evaluation evaluation) : IReadDto<EvaluationReadDto, Evaluation>
{
    public int Id { get; } = evaluation.Id;
    public int Points { get; } = evaluation.Points;
    public string Comment { get; } = evaluation.Comment;
    public TeacherReadDto Teacher { get; } = evaluation.Teacher.ToDto();

    public static EvaluationReadDto FromEntity(Evaluation evaluation) => new(evaluation);
}

public class EvaluationWriteDto : IWriteDto<EvaluationWriteDto, Evaluation>
{
    public required int Points { get; set; }
    public required string Comment { get; set; }
    public required int TeacherId { get; set; }
    public required int SolutionId { get; set; }

    public Evaluation ToEntity() => new() {
        Id = default,
        Points = Points,
        Comment = Comment,
        TeacherId = TeacherId,
        Teacher = null!
    };
}

[GenerateDtoExtensions<Evaluation, EvaluationReadDto, EvaluationWriteDto>]
public static partial class EvaluationDtoExtensions;
