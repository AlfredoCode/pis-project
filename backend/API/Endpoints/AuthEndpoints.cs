using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRegSys.DAL.Helpers;
using PRegSys.BL.Services;
using PRegSys.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRegSys.API.Endpoints;

public class AuthEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/login", LoginHandler)
            .WithName("Login");

        group.MapPost("/register", RegisterHandler)
            .WithName("Register");
    }

    private static async Task<IResult> LoginHandler(
        [FromServices] UserService userService,
        [FromBody] LoginRequest login,
        [FromServices] IConfiguration config)
    {
        var user = await userService.GetUserByUsername(login.Username);
        if (user == null || !PasswordHelper.VerifyPassword(login.Password, user.Password))
        {
            return TypedResults.Unauthorized();
        }

        var token = GenerateJwtToken(user, config);
        return TypedResults.Ok<object>(new { token });
    }

    private static async Task<IResult> RegisterHandler(
        [FromServices] UserService userService,
        [FromBody] RegisterRequest register,
        [FromServices] IConfiguration config)
    {
        var hashedPassword = PasswordHelper.HashPassword(register.Password);
        User newUser;

        if (register.UserType.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
        {
            newUser = new Teacher
            {
                Id = 0,
                Username = register.Username,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Password = hashedPassword
            };
        }
        else if (register.UserType.Equals("Student", StringComparison.OrdinalIgnoreCase))
        {
            newUser = new Student
            {
                Id = 0,
                Username = register.Username,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Password = hashedPassword
            };
        }
        else
        {
            return TypedResults.BadRequest("Invalid user type. Valid types are 'Teacher' or 'Student'.");
        }

        newUser = await userService.CreateUser(newUser);
        var token = GenerateJwtToken(newUser, config);
        return TypedResults.Ok<object>(new { token });
    }

    private static string GenerateJwtToken(User user, IConfiguration config)
    {
        var jwtSettings = config.GetSection("JwtSettings");

        var secret = jwtSettings["Secret"]
            ?? throw new InvalidOperationException("JWT Secret is not configured.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
        };

        if (user is Teacher)
        {
            claims.Add(new Claim("UserType", "Teacher"));
        }
        else if (user is Student)
        {
            claims.Add(new Claim("UserType", "Student"));
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record LoginRequest(string Username, string Password);

public record RegisterRequest(string Username, string Password, string FirstName, string LastName, string UserType);