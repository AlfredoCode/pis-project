using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Seeds;

public static class UserSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new { Id = 1, FirstName = "John", LastName = "Doe", Username = "jdoe", user_type = "Teacher" },
            new { Id = 2, FirstName = "Alice", LastName = "Wonder", Username = "alice", user_type = "Student" },
            new { Id = 3, FirstName = "Bob", LastName = "Builder", Username = "bob", user_type = "Student" },
            new { Id = 4, FirstName = "Martin", LastName = "Novak", Username = "mnovak", user_type = "Teacher" },
            new { Id = 5, FirstName = "Jana", LastName = "Svobodova", Username = "jsvobodova", user_type = "Student" },
            new { Id = 6, FirstName = "Pavel", LastName = "Kucera", Username = "pkucera", user_type = "Student" },
            new { Id = 7, FirstName = "Eva", LastName = "Horakova", Username = "ehorakova", user_type = "Student" },
            new { Id = 8, FirstName = "Tomas", LastName = "Marek", Username = "tmarek", user_type = "Student" }
        );
    }
}