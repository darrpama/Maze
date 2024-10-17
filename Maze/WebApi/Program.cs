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
builder.Services.AddCors();

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
        var exportMaze = new List<List<MazeCellWalls>>();
        for (var i = 0; i < rows; i++)
        {
            var row = new List<MazeCellWalls>();
            for (var j = 0; j < cols; j++)
            {
                var point = maze.Points[i, j];

                row.Add(new MazeCellWalls(point.Right, point.Down));
            }

            exportMaze.Add(row);
        }

        return exportMaze;
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
if (app.Environment.IsDevelopment())
{
    app.UseCors(corsPolicyBuilder => corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
}

app.Run();