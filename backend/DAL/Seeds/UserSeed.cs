using Microsoft.EntityFrameworkCore;
using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Seeds;

public static class UserSeed
{
    private const string HashedPassword = "GuExk1o8330FmzAc3LNRRjqutOq+kfcH6QfDFEyfiCWPdcbe";
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new { Id = 1, FirstName = "John", LastName = "Doe", Username = "jdoe", Password = HashedPassword, user_type = "Teacher" },
            new { Id = 2, FirstName = "Alice", LastName = "Wonder", Username = "alice", Password = HashedPassword, user_type = "Student" },
            new { Id = 3, FirstName = "Bob", LastName = "Builder", Username = "bob", Password = HashedPassword, user_type = "Student" },
            new { Id = 4, FirstName = "Martin", LastName = "Novak", Username = "mnovak", Password = HashedPassword, user_type = "Teacher" },
            new { Id = 5, FirstName = "Jana", LastName = "Svobodova", Username = "jsvobodova", Password = HashedPassword, user_type = "Student" },
            new { Id = 6, FirstName = "Pavel", LastName = "Kucera", Username = "pkucera", Password = HashedPassword, user_type = "Student" },
            new { Id = 7, FirstName = "Eva", LastName = "Horakova", Username = "ehorakova", Password = HashedPassword, user_type = "Student" },
            new { Id = 8, FirstName = "Tomas", LastName = "Marek", Username = "tmarek", Password = HashedPassword, user_type = "Student" }
        );
    }
}
