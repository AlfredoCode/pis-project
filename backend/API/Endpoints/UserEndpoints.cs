using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

public class UserEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/users", async Task<IEnumerable<User>> (UserService users) =>
        {
            return await users.GetAllUsers();
        }).WithName("GetAllUsers");

        group.MapGet("/students", async Task<IEnumerable<Student>> (UserService users) =>
        {
            return await users.GetAllStudents();
        }).WithName("GetAllStudents");

        group.MapGet("/teachers", async Task<IEnumerable<Teacher>> (UserService users) =>
        {
            return await users.GetAllTeachers();
        }).WithName("GetAllTeachers");

        group.MapGet("/users/{id:int}", async Task<Results<Ok<User>, NotFound>> (UserService users, int id) =>
        {
            return (await users.GetUserById(id) is User user)
                ? TypedResults.Ok(user)
                : TypedResults.NotFound();
        }).WithName("GetUserById");

        group.MapGet("/users/{username}", async Task<Results<Ok<User>, NotFound>> (UserService users, string username) =>
        {
            return (await users.GetUserByUsername(username) is User user)
                ? TypedResults.Ok(user)
                : TypedResults.NotFound();
        }).WithName("GetUserByUsername");

        group.MapPost("/users", async Task<Results<Created<User>, BadRequest<string>>> (UserService users, User user) =>
        {
            try
            {
                var createdUser = await users.CreateUser(user);
                return TypedResults.Created($"/users/{createdUser.Id}", createdUser);
            }
            catch (DbUpdateException ex)
            {
                return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }).WithName("CreateUser");

        group.MapPut("/users/{id:int}", async Task<Results<Ok<User>, NotFound, BadRequest>> (UserService users, int id, User user) =>
        {
            if (id != user.Id)
            {
                return TypedResults.BadRequest();
            }

            var existingUser = await users.GetUserById(id);
            if (existingUser is null)
            {
                return TypedResults.NotFound();
            }

            var updatedUser = await users.UpdateUser(user);
            return TypedResults.Ok(updatedUser);
        }).WithName("UpdateUser");

        group.MapDelete("/users/{id:int}", async Task<Results<NoContent, NotFound>> (UserService users, int id) =>
        {
            var existingUser = await users.GetUserById(id);
            if (existingUser is null)
            {
                return TypedResults.NotFound();
            }

            await users.DeleteUser(id);
            return TypedResults.NoContent();
        }).WithName("DeleteUser");
    }
}