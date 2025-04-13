using PRegSys.DAL.Repositories;
using PRegSys.DAL.Entities;

namespace PRegSys.BL.Services;

public class UserService(UserRepository users)
{
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await users.GetAllUsers();
    }

    public async Task<IEnumerable<Student>> GetAllStudents()
    {
        return await users.GetAllStudents();
    }

    public async Task<IEnumerable<Teacher>> GetAllTeachers()
    {
        return await users.GetAllTeachers();
    }

    public async Task<User?> GetUserById(int id)
    {
        return await users.GetUserById(id);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await users.GetUserByUsername(username);
    }

    public async Task<User> CreateUser(User user)
    {
        return await users.CreateUser(user);
    }

    public async Task<User> UpdateUser(User user)
    {
        return await users.UpdateUser(user);
    }

    public async Task DeleteUser(int id)
    {
        await users.DeleteUser(id);
    }
}
