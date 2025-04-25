using Microsoft.EntityFrameworkCore;
using NodaTime;
using PRegSys.DAL.Entities;
using PRegSys.DAL.Enums;

namespace PRegSys.DAL.Seeds;

public static class SignUpRequestSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SignUpRequest>().HasData(
            new
            {
                Id = 1,
                CreationDate = Instant.FromUtc(2023, 9, 1, 8, 0, 0),
                State = StudentSignUpState.Created,
                StudentId = 3,
                TeamId = 1
            }
        );
    }
}