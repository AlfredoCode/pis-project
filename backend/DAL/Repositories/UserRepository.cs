using Microsoft.EntityFrameworkCore;

using PRegSys.DAL.Entities;

namespace PRegSys.DAL.Repositories;

public class UserRepository(PregsysDbContext db)
{
    public async Task<IEnumerable<User>> GetAllUsers() => await db.Users.ToListAsync();
    public async Task<IEnumerable<Student>> GetAllStudents() => await db.Students.ToListAsync();
    public async Task<IEnumerable<Teacher>> GetAllTeachers() => await db.Teachers.ToListAsync();
    public async Task<User?> GetUserById(int id) => await db.Users.FindAsync(id);

    public async Task<User?> GetUserByUsername(string username)
        => await db.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User> CreateUser(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return (await GetUserById(user.Id))!;
    }

    public async Task<User> UpdateUser(User user)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync();
        return (await GetUserById(user.Id))!;
    }

    public async Task DeleteUser(int id)
    {
        await db.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
    }
}
