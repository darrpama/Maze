using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using MazeModel.Maze;

namespace GuiInterface.Controls;

[TemplatePart("PART_Grid", typeof(Grid), IsRequired = true)]
public class MazeControl : TemplatedControl
{
    private Grid? MazeGrid { get; set; }


    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        MazeGrid = e.NameScope.Find<Grid>("PART_Grid");
        Maze.ChangeMaze += OnMazeChanged;
        Maze.ChangePath += OnMazePathChanged;
        Console.WriteLine("MazeControl.OnApplyTemplate");
    }

    private void OnMazePathChanged(object? sender, IList<MazePoint> path)
    {
        foreach (var child in MazeGrid!.Children)
        {
            var cell = (MazeCellControl)child;
            cell.IsCircleVisible = false;
        }

        // TODO: вложенный цикл выглядит некрасиво, переделать!
        for (var i = 0; i < path.Count; i++)
        {
            var pathPoint = path[i];


            foreach (var child in MazeGrid!.Children)
            {
                var cell = (MazeCellControl)child;
                if (cell.Col != pathPoint.Col || cell.Row != pathPoint.Row) continue;


                if (i == 0)
                {
                    cell.ShowPathPart(PathPartTypeEnum.Start);
                    continue;
                }

                if (i == path.Count - 1)
                {
                    cell.ShowPathPart(PathPartTypeEnum.End);
                    continue;
                }

                cell.ShowPathPart(PathPartTypeEnum.PathPart);
            }
        }
    }

    private void OnMazeChanged(object? sender, MazePoint[,] mazePoints)
    {
        if (Maze.Points is null) return;

        _clearMazeUi();

        _defineRowsAndColsUi();
        var rectangles = _GetRectangles();

        _drawRectanglesUi(rectangles);
    }

    private void _drawRectanglesUi(List<Control> rechtangles)
    {
        Dispatcher.UIThread.Post(() => { MazeGrid!.Children.AddRange(rechtangles); });
    }

    private List<Control> _GetRectangles()
    {
        var rechtangles = new List<Control>();

        for (var row = 0; row < Maze.Points!.GetLength(0); row++)
        {
            for (var col = 0; col < Maze.Points.GetLength(1); col++)
            {
                var cell = Maze.Points[row, col];

                var rect = new MazeCellControl(row, col, cell.Right, cell.Down);

                rect.RightClick += OnMazeCellRightClick;
                rect.LeftClick += OnMazeCellLeftClick;

                Grid.SetRow(rect, row);
                Grid.SetColumn(rect, col);
                rechtangles.Add(rect);
            }
        }

        return rechtangles;
    }

    private void _defineRowsAndColsUi()
    {
        var rowDefinitions = new RowDefinitions();
        var columnDefinitions = new ColumnDefinitions();
        for (var row = 0; row < Maze.Points!.GetLength(0); row++)
        {
            var rowDefinition = new RowDefinition();

            rowDefinitions.Add(rowDefinition);
        }

        for (var col = 0; col < Maze.Points.GetLength(1); col++)
        {
            var colDefinition = new ColumnDefinition();

            columnDefinitions.Add(colDefinition);
        }

        Dispatcher.UIThread.Post(() =>
        {
            MazeGrid!.RowDefinitions = rowDefinitions;
            MazeGrid!.ColumnDefinitions = columnDefinitions;
        });
    }

    private void _clearMazeUi()
    {
        Dispatcher.UIThread.Post(() =>
        {
            foreach (var child in MazeGrid!.Children)
            {
                ((MazeCellControl)child).RightClick -= OnMazeCellRightClick;
                ((MazeCellControl)child).LeftClick -= OnMazeCellLeftClick;
            }

            MazeGrid!.Children.Clear();
            MazeGrid!.RowDefinitions.Clear();
            MazeGrid!.ColumnDefinitions.Clear();
        });
    }

    private void OnMazeCellRightClick(object? sender, RoutedEventArgs e)
    {
        var mazeCell = (MazeCellControl)sender!;
        mazeCell.ShowPathPart(PathPartTypeEnum.Start);
        Maze.StartPoint = Maze.Points![mazeCell.Row, mazeCell.Col];

        if (Maze.CanBuildPath)
        {
            Maze.BuildPath();
        }
    }

    private void OnMazeCellLeftClick(object? sender, RoutedEventArgs e)
    {
        var mazeCell = (MazeCellControl)sender!;
        mazeCell.ShowPathPart(PathPartTypeEnum.End);
        Maze.EndPoint = Maze.Points![mazeCell.Row, mazeCell.Col];

        if (Maze.CanBuildPath)
        {
            Maze.BuildPath();
        }
    }


    public Maze Maze
    {
        get => GetValue(MazeProperty);
        set => SetValue(MazeProperty, value);
    }

    public static readonly StyledProperty<Maze> MazeProperty =
        AvaloniaProperty.Register<MazeControl, Maze>(nameof(Maze));
}