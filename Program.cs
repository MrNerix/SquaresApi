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


app.Run();