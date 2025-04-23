using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PRegSys.DAL.Entities;

public class Team : IEntity
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; } = string.Empty;

    public required int LeaderId { get; set; }
    public required Student Leader { get; set; }

    public required int ProjectId { get; set; }
    public required Project Project { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<SignUpRequest> SignUpRequests { get; set; } = new List<SignUpRequest>();
}

file class Configuration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasOne(t => t.Leader)
            .WithMany()
            .HasForeignKey(t => t.LeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Project)
            .WithMany(p => p.Teams)
            .HasForeignKey(t => t.ProjectId);

        builder.HasMany(t => t.Students)
            .WithMany()
            .UsingEntity(j => j.ToTable("TeamMembers"));
    }
}