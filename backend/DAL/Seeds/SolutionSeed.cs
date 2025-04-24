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
                EvaluationId = 1,
                File = new byte[] { 0x0 }, // Dummy file content
                ProjectId = 1,
                SubmissionDate = Instant.FromDateTimeUtc(new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc)),
                TeamId = 1
            }
        );
    }
}