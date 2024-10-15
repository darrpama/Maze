using Common.NumbersGenerator;
using MazeModel.Exceptions;
using MazeModel.Maze;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IGenerator, RandomGenerator>(_ => new RandomGenerator(new Random()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/maze/generate", (int rows, int cols, IGenerator generator) =>
    {
        var maze = new Maze(generator);
        maze.Generate(rows, cols);
        return maze.ExportString();
    })
    .WithName("GenerateMaze")
    .WithOpenApi();

app.MapPost("/maze/findPath",
        ([FromBody] string mazeString, int sourceX, int sourceY, int destX, int destY, IGenerator generator) =>
        {
            var maze = new Maze(generator);
            try
            {
                maze.ImportString(mazeString);

            }
            catch (ImportMazeError e)
            {
                var errorDictionary = new Dictionary<string, string[]>();
                errorDictionary["mazeString"] = [e.Message];
                return Results.ValidationProblem(errorDictionary);
            }
            maze.StartPoint = maze.Points[sourceX, sourceY];
            maze.EndPoint = maze.Points[destX, destY];
            maze.BuildPath();
            var result = maze.Path.Select(item => new MazeCellCoords(item.Row, item.Col));
            return Results.Ok(result);

        })
    .WithName("MazeFindPath")
    .WithOpenApi();

app.Run();
