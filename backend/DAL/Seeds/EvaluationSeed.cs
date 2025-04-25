using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Seeds;

public static class EvaluationSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Evaluation>().HasData(
            new
            {
                Id = 1,
                Comment = "Good work overall.",
                Points = 85,
                TeacherId = 1
            }
        );
    }
}