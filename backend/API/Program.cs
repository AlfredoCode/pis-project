using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi;

using Scalar.AspNetCore;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

using PRegSys.DAL;
using PRegSys.BL;
using PRegSys.API;
using PRegSys.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("PisDB")
    ?? throw new InvalidOperationException("Connection string 'PisDB' is not set");

// Add services to the container.
builder.Services.AddSingleton<IClock>(SystemClock.Instance);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddBLServices();
builder.Services.AddDALServices(connectionString);

builder.Services.Configure<JsonOptions>(o => {
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    o.SerializerOptions.ConfigureForNodaTime(
        new NodaJsonSettings(DateTimeZoneProviders.Tzdb));
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(o => {
    o.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
    o.AddSchemaTransformer<OpenApiSchemaTransformer>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.MapOpenApi();
    app.MapScalarApiReference(o => {
        o.CustomCss = TryReadAllText("scalar-customize.css");
    });
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World");

app.RegisterEndpointDefinitions();

app.Run();

string? TryReadAllText(string fileName)
{
    try
    {
        return File.ReadAllText(fileName);
    }
    catch (Exception ex)
    {
        app.Logger.LogError("Failed to read '{fileName}': {message}", fileName, ex.Message);
        return null;
    }
}
