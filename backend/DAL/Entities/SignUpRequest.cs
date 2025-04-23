using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using NodaTime;
using PRegSys.DAL.Enums;

namespace PRegSys.DAL.Entities;

public class SignUpRequest : IEntity
{
    public required int Id { get; set; }
    public required Instant CreationDate { get; set; }
    public required StudentSignUpState State { get; set; }

    public required int StudentId { get; set; }
    public required Student Student { get; set; }

    public required int TeamId { get; set; }
    [JsonIgnore]
    public Team Team { get; set; } = null!;
}

file class Configuration : IEntityTypeConfiguration<SignUpRequest>
{
    public void Configure(EntityTypeBuilder<SignUpRequest> builder)
    {
        builder.HasOne(r => r.Student)
            .WithMany()
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Team)
            .WithMany(t => t.SignUpRequests)
            .HasForeignKey(r => r.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}