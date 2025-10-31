using Microsoft.AspNetCore.Mvc;
using SquaresApi.Models;
using SquaresApi.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IPointStore, InMemoryPointStore>();

var app = builder.Build();
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



app.Run();