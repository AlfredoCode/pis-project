using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PRegSys.DAL.Entities;

public abstract class User : IEntity
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}

public class Teacher : User;

public class Student : User;

file class Configuration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasDiscriminator<string>("user_type");
        builder.Property(u => u.Username).UseCollation("case_insensitive");
        builder.HasIndex(u => u.Username).IsUnique();
    }
}
