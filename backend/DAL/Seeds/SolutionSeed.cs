using Microsoft.EntityFrameworkCore;
using NodaTime;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Seeds;

public static class SolutionSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Solution>().HasData(
            new
            {
                Id = 1,
                File = "old"u8.ToArray(),
                FileExtension = "txt",
                ProjectId = 1,
                SubmissionDate = Instant.FromUtc(2023, 10, 8, 7, 0, 0),
                TeamId = 1
            },
            new
            {
                Id = 2,
                File = "new"u8.ToArray(),
                FileExtension = "txt",
                ProjectId = 1,
                SubmissionDate = Instant.FromUtc(2023, 10, 10, 12, 0, 0),
                TeamId = 1,
                EvaluationId = 1
            }
        );
    }
}