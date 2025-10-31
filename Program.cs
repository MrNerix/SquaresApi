using Microsoft.AspNetCore.Mvc;
using SquaresApi.Models;
using SquaresApi.Storage;
using SquaresApi.Core;
using Microsoft.EntityFrameworkCore;
using SquaresApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQLite
var conn = builder.Configuration.GetConnectionString("Default") ?? "Data Source=squares.db";
builder.Services.AddDbContext<AppDb>(opt => opt.UseSqlite(conn));

// Swap to SQLite-backed store
builder.Services.AddScoped<IPointStore, SqlitePointStore>();

var app = builder.Build();

//Ensure the db exists on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    db.Database.EnsureCreated();
}


app.UseSwagger();
app.UseSwaggerUI();


// Import a list of points. Duplicates are ignored.
app.MapPost("/points/import", ([FromBody] ImportPointsDto dto, IPointStore store) =>
{
    List<Point> points = new List<Point>();

    if (dto.Points != null)
    {
        foreach (var p in dto.Points)
        {
            points.Add(new Point(p.X, p.Y));
        }
    }

    store.AddMany(points);

    return Results.Accepted();
});

// Add a single point. 201 if new, 409 if it already exists.
app.MapPost("/points", ([FromBody] CreatePointDto dto, IPointStore store) =>
{
    var point = new Point(dto.X, dto.Y);
    var added = store.Add(point);

    if (added)
    {
        return Results.Created($"/points/{dto.X},{dto.Y}", dto);
    }
    else
    {
        return Results.Conflict("Point already exists.");
    }
});

// Delete point by coordinate. returns 204 even if it wasn't there.
app.MapDelete("/points/{x:int},{y:int}", (int x, int y, IPointStore store) =>
{
    store.Remove(new Point(x, y)); // Ignore result for idempotency
    return Results.NoContent();
});


// Get all squares and return how many there are
app.MapGet("/squares", (IPointStore store) =>
{
    // Get all points
    var points = store.GetAll();

    // Find all sets of 4 points that form squares
    var foundSquares = SquareFinder.FindSquares(points);

    // Convert each found square into a SquareDto
    var squareDtos = new List<SquareDto>();
    foreach (var square in foundSquares)
    {
        var pointDtos = new List<CreatePointDto>();
        foreach (var p in square)
        {
            pointDtos.Add(new CreatePointDto(p.X, p.Y));
        }

        squareDtos.Add(new SquareDto(pointDtos));
    }

    var response = new SquaresResponse(squareDtos.Count, squareDtos);

    // Return a 200 OK result with the response data
    return Results.Ok(response);
});


app.MapGet("/", () => Results.Redirect("/swagger"));


app.Run();