using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/users", async Task<IEnumerable<User>> (UserService users) => {
            return await users.GetAllUsers();
        })
            .WithName("GetAllUsers");

        group.MapGet("/students", async Task<IEnumerable<Student>> (UserService users) => {
            return await users.GetAllStudents();
        })
            .WithName("GetAllStudents");

        group.MapGet("/teachers", async Task<IEnumerable<Teacher>> (UserService users) => {
            return await users.GetAllTeachers();
        })
            .WithName("GetAllTeachers");

        group.MapGet("/users/{id:int}", async Task<Results<Ok<User>, NotFound>>
            (UserService users, int id) => {
                return (await users.GetUserById(id) is User user)
                    ? TypedResults.Ok(user)
                    : TypedResults.NotFound();
            })
            .WithName("GetUserById");

        group.MapGet("/users/{username}", async Task<Results<Ok<User>, NotFound>>
            (UserService users, string username) => {
                return (await users.GetUserByUsername(username) is User user)
                    ? TypedResults.Ok(user)
                    : TypedResults.NotFound();
            })
            .WithName("GetUserByUsername");

        return group;
    }
}
