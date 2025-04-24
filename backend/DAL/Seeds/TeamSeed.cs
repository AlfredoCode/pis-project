using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Seeds;

public static class TeamSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Team>().HasData(
            new
            {
                Id = 1,
                Description = "Team Alpha",
                Size = 3,
                LeaderId = 2,
                ProjectId = 1
            },
            new
            {
                Id = 2,
                Description = "Team Beta",
                Size = 3,
                LeaderId = 4,
                ProjectId = 1
            }
        );

        modelBuilder.Entity("StudentTeam").HasData(
            new { StudentsId = 2, TeamId = 1 },
            new { StudentsId = 3, TeamId = 1 },
            new { StudentsId = 4, TeamId = 2 },
            new { StudentsId = 5, TeamId = 2 },
            new { StudentsId = 6, TeamId = 2 }
        );
    }
}
