using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Common.NumbersGenerator;
using MazeModel.Exporters;
using MazeModel.Importers;
using MazeModel.Maze;
using MazeModel.MazeGenerator;
using ReactiveUI;

namespace GuiInterface.ViewModels;

public class MazeViewModel: ViewModelBase
{
    private Maze _maze;
    public Maze Maze
    {
        get;
        private set;
    }

    private MazePoint[,]? _mazePoints;

    public MazePoint[,]? MazePoints
    {
        get => _mazePoints;
        set => this.RaiseAndSetIfChanged(ref _mazePoints, value);
    }

    public int MaxSize { get; } = 50;

    public int Rows { get; set; }
    public int Columns { get; set; }


    public ReactiveCommand<Unit, Unit> GenerateMazeCommand { get; }
    public ICommand ImportMazeFromFileCommand { get; }
    
    public ICommand ExportMazeToFileCommand { get; }
    

    public MazeViewModel()
    {
        var random = new Random();
        var randomGenerator = new RandomGenerator(random);
        var maze = new Maze(randomGenerator);
        Maze = maze;

        Maze.ChangeMaze += _onMazeChanged;
        Maze.ChangePath += _onPathChanged;
        
        GenerateMazeCommand = ReactiveCommand.Create(GenerateMaze);
        ImportMazeFromFileCommand = ReactiveCommand.CreateFromTask(ImportMaze);
        ExportMazeToFileCommand = ReactiveCommand.CreateFromTask(ExportMaze);
        
        _importMazeInteraction = new Interaction<string?, string?>();
        _exportMazeInteraction = new Interaction<string?, string?>();

        Rows = MaxSize;
        Columns = MaxSize;
    }


    private void _onMazeChanged(object? maze, MazePoint[,] points)
    {
        MazePoints = points;
    }
    private void _onPathChanged(object? sender, IEnumerable<MazePoint> e)
    {
        
    }
    
    private void GenerateMaze()
    {
        Console.WriteLine("Generating maze...");
        Maze.Generate(Rows, Columns);
    }


    private readonly Interaction<string?, string?> _importMazeInteraction;
    
    private readonly Interaction<string?, string?> _exportMazeInteraction;

    public Interaction<string?, string?> ImportMazeInteraction => _importMazeInteraction;
    
    public Interaction<string?, string?> ExportMazeInteraction => _exportMazeInteraction;

    private async Task ImportMaze()
    {
        var importString = await _importMazeInteraction.Handle(null);
        if (importString == null)
            return;
        Maze.ImportString(importString);
    }
    
    private async Task ExportMaze()
    {
        if (Maze.Points is null) return;
        
        var mazeString = Maze.ExportString();
        
        await _exportMazeInteraction.Handle(mazeString);
    }
}