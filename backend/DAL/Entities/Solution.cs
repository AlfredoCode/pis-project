using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace PRegSys.DAL.Entities;

public class Solution : IEntity
{
    public required int Id { get; set; }
    public required byte[] File { get; set; }
    public required string FileExtension { get; set; }
    public required Instant SubmissionDate { get; set; }

    public required int ProjectId { get; set; }
    public required Project Project { get; set; }

    public required int TeamId { get; set; }
    public required Team Team { get; set; }

    public required int? EvaluationId { get; set; }
    public required Evaluation? Evaluation { get; set; }
}

file class Configuration : IEntityTypeConfiguration<Solution>
{
    public void Configure(EntityTypeBuilder<Solution> builder)
    {
    }
}