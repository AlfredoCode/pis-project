using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using NodaTime;

namespace PRegSys.DAL.Entities;

public class Project : IEntity
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Course { get; set; }
    public required string Description { get; set; }
    public required int MaxTeamSize { get; set; }
    public required int Capacity { get; set; }
    public required Instant Deadline { get; set; }

    public required int OwnerId { get; set; }
    public required Teacher Owner { get; set; } = null!;

    public required ICollection<Team> Teams { get; set; } = new List<Team>();
}

file class Configuration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
    }
}
