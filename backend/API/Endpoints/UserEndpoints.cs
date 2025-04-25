using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using PRegSys.API.DTO;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;

namespace PRegSys.API.Endpoints;

public class UserEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/users",
            async Task<IEnumerable<UserReadDto>> (UserService users) =>
            {
                return (await users.GetAllUsers()).ToDto();
            })
            .WithName("GetAllUsers");

        group.MapGet("/students",
            async Task<IEnumerable<StudentReadDto>> (UserService users) =>
            {
                return (await users.GetAllStudents()).ToDto();
            })
            .WithName("GetAllStudents");

        group.MapGet("/teachers",
            async Task<IEnumerable<TeacherReadDto>> (UserService users) =>
            {
                return (await users.GetAllTeachers()).ToDto();
            })
            .WithName("GetAllTeachers");

        group.MapGet("/users/{id:int}",
            async Task<Results<Ok<UserReadDto>, NotFound>> (UserService users, int id) =>
            {
                return (await users.GetUserById(id) is User user)
                    ? TypedResults.Ok(user.ToDto())
                    : TypedResults.NotFound();
            })
            .WithName("GetUserById");

        group.MapGet("/users/{username}",
            async Task<Results<Ok<UserReadDto>, NotFound>> (UserService users, string username) =>
            {
                return (await users.GetUserByUsername(username) is User user)
                    ? TypedResults.Ok(user.ToDto())
                    : TypedResults.NotFound();
            })
            .WithName("GetUserByUsername");

        group.MapPost("/users",
            async Task<Results<Created<UserReadDto>, BadRequest<string>>> (UserService users, UserWriteDto user) =>
            {
                try
                {
                    var createdUser = await users.CreateUser(user.ToEntity(default));
                    return TypedResults.Created($"/users/{createdUser.Id}", createdUser.ToDto());
                }
                catch (DbUpdateException ex)
                {
                    return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            })
            .WithName("CreateUser");

        group.MapPut("/users/{id:int}",
            async Task<Results<Ok<UserReadDto>, BadRequest<string>>> (UserService users, int id, UserWriteDto user) =>
            {
                try
                {
                    var updatedUser = await users.UpdateUser(user.ToEntity(id));
                    return TypedResults.Ok(updatedUser.ToDto());
                }
                catch (DbUpdateException ex)
                {
                    return TypedResults.BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            })
            .WithName("UpdateUser");

        group.MapDelete("/users/{id:int}",
            async (UserService users, int id) =>
            {
                await users.DeleteUser(id);
                return TypedResults.NoContent();
            })
            .WithName("DeleteUser");
    }
}