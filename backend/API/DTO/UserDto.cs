using PRegSys.DAL.Entities;

namespace PRegSys.API.DTO;

public enum UserRole { Student, Teacher }

public abstract class UserReadDto(User user) : IReadDto<UserReadDto, User>
{
    public int Id { get; } = user.Id;
    public string FirstName { get; } = user.FirstName;
    public string LastName { get; } = user.LastName;
    public string FullName { get; } = user.FullName;
    public string Username { get; } = user.Username;

    public UserRole Role { get; } = user switch
    {
        Teacher => UserRole.Teacher,
        Student => UserRole.Student,
    };

    public static UserReadDto FromEntity(User user) => user switch
    {
        Teacher t => new TeacherReadDto(t),
        Student s => new StudentReadDto(s),
    };
}

public class StudentReadDto(Student student) : UserReadDto(student)
{
}

public class TeacherReadDto(Teacher teacher) : UserReadDto(teacher)
{
}

public class UserWriteDto : IWriteDto<UserWriteDto, User>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required UserRole Role { get; set; }

    public User ToEntity(int id) => Role switch
    {
        UserRole.Student => new Student()
        {
            Id = id,
            FirstName = FirstName,
            LastName = LastName,
            Username = Username,
        },
        UserRole.Teacher => new Teacher()
        {
            Id = id,
            FirstName = FirstName,
            LastName = LastName,
            Username = Username,
        },
    };
}

[GenerateDtoExtensions<User, UserReadDto, UserWriteDto>]
public static partial class UserDtoExtensions
{
    public static StudentReadDto ToDto(this Student user) => new(user);
    public static IEnumerable<StudentReadDto> ToDto(this IEnumerable<Student> students)
        => students.Select(s => new StudentReadDto(s));

    public static TeacherReadDto ToDto(this Teacher user) => new(user);
    public static IEnumerable<TeacherReadDto> ToDto(this IEnumerable<Teacher> teachers)
        => teachers.Select(t => new TeacherReadDto(t));
}
