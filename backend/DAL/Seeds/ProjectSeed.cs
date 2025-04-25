using Microsoft.EntityFrameworkCore;
using NodaTime;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Seeds;

public static class ProjectSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>().HasData(
            new
            {
                Id = 1,
                Name = "IZP Projekt 1",
                Course = "IZP - Zaklady Programovani",
                Description = "Prime number applications using bit fields.",
                MaxTeamSize = 3,
                Capacity = 5,
                Deadline = Instant.FromDateTimeUtc(new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)),
                OwnerId = 1
            }
        );
    }
}
