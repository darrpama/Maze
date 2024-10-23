using System.Text;
using Common.NumbersGenerator;
using MazeModel.Exceptions;
using MazeModel.Maze;
using Microsoft.AspNetCore.Mvc;
using WebApi;
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
        try
        {
            maze.Generate(rows, cols);
        }
        catch (ArgumentOutOfRangeException e)
        {
            return Results.BadRequest(e.Message);
        }
        return Results.Ok(maze.ExportList());
    })
    .WithName("GenerateMaze")
    .WithOpenApi();

app.MapPost("/maze/findPath",
        ([FromBody] List<List<MazePointWalls>> mazeList, int sourceX, int sourceY, int destX, int destY, IGenerator generator) =>
        {
            var maze = new Maze(generator);
            maze.ImportList(mazeList);

            maze.StartPoint = maze.Points![sourceX, sourceY];
            maze.EndPoint = maze.Points[destX, destY];
            maze.BuildPath();
            var result = maze.Path!.Select(item => new MazeCellCoords(item.Row, item.Col));
            return Results.Ok(result);
        })
    .WithName("MazeFindPath")
    .WithOpenApi();

app.MapPost("/maze/fromString", ([FromBody] string mazeString, IGenerator generator) =>
        {

            var maze = new Maze(generator);
            try
            {
                maze.ImportString(mazeString);
            }
            catch (ImportMazeError e)
            {
                return Results.BadRequest(e.Message);
            }

            return Results.Ok(maze.ExportList());
        })
    .WithName("MazeFromString")
    .WithOpenApi();

app.MapPost("/maze/toString", ([FromBody] List<List<MazePointWalls>> mazeList, IGenerator generator) =>
        {
            var maze = new Maze(generator);
            maze.ImportList(mazeList);
            return Results.Ok(maze.ExportString());
        })
    .WithName("MazeToString")
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