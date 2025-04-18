using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PRegSys.DAL.Entities;

public class Evaluation : IEntity
{
    public required int Id { get; set; }

    public required int Points { get; set; }

    public required string Comment { get; set; }

    public required int TeacherId { get; set; }
    public required Teacher Teacher { get; set; }
}

file class Configuration : IEntityTypeConfiguration<Evaluation>
{
    public void Configure(EntityTypeBuilder<Evaluation> builder)
    {
    }
}