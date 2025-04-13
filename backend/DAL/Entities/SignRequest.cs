using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PRegSys.DAL.Entities
{
    public class SignRequest : IEntity
    {
        public required int Id { get; set; }
        public required DateTime CreationDate { get; set; }
        public required string State { get; set; }

        public required int StudentId { get; set; }
        public required Student Student { get; set; } = null!;

        public required int TeamId { get; set; }
        [JsonIgnore]
        public Team Team { get; set; } = null!;
    }

    file class Configuration : IEntityTypeConfiguration<SignRequest>
    {
        public void Configure(EntityTypeBuilder<SignRequest> builder)
        {
            builder.HasOne(r => r.Student)
                .WithMany()
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Team)
                .WithMany(t => t.SignRequests)
                .HasForeignKey(r => r.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}