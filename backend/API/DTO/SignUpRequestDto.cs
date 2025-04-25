using NodaTime;

using PRegSys.DAL.Entities;
using PRegSys.DAL.Enums;

namespace PRegSys.API.DTO;

public class SignUpRequestReadDto(SignUpRequest signUpRequest) : IReadDto<SignUpRequestReadDto, SignUpRequest>
{
    public int Id { get; } = signUpRequest.Id;
    public Instant CreationDate { get; } = signUpRequest.CreationDate;
    public StudentSignUpState State { get; } = signUpRequest.State;
    public UserReadDto Student { get; } = signUpRequest.Student.ToDto();
    public TeamReadDto Team { get; } = signUpRequest.Team.ToDto();

    public static SignUpRequestReadDto FromEntity(SignUpRequest signUpRequest) => new(signUpRequest);
}

public class SignUpRequestWriteDto : IWriteDto<SignUpRequestWriteDto, SignUpRequest>
{
    public required int StudentId { get; set; }
    public required int TeamId { get; set; }

    public SignUpRequest ToEntity(IClock clock) => new()
    {
        Id = default,
        CreationDate = clock.GetCurrentInstant(),
        State = StudentSignUpState.Created,
        StudentId = StudentId,
        Student = null!,
        TeamId = TeamId,
        Team = null!,
    };
}

[GenerateDtoExtensions<SignUpRequest, SignUpRequestReadDto, SignUpRequestWriteDto>]
public static partial class SignUpRequestDtoExtensions;
