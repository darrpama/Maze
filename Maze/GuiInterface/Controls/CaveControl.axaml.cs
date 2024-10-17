using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using Avalonia.Threading;
using CaveModel;

namespace GuiInterface.Controls;

[TemplatePart("PartGrid", typeof(Grid), IsRequired = true)]
public class CaveControl : TemplatedControl
{
    private Grid? CaveGrid { get; set; }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        CaveGrid = e.NameScope.Find<Grid>("PartGrid");
        Cave.ChangeCave += OnCaveChanged;
    }
    
    private void OnCaveChanged(object? sender, CaveCell[,] caveCells)
    {
        if (Cave.Cells is null) return;
        _clearCaveUi();
        _defineRowsAndColsUi();
        
        var rectangles = _getRectangles(caveCells);
        _drawRectanglesUi(rectangles);
    }

    private void _drawRectanglesUi(List<Control> rectangles)
    {
        Dispatcher.UIThread.Post(() => { CaveGrid!.Children.AddRange(rectangles); });
    }

    private List<Control> _getRectangles(CaveCell[,] cells)
    {
        Console.WriteLine("GetRectangles");
        var rectangles = new List<Control>();
        Console.WriteLine(cells.GetLength(0));
        Console.WriteLine(cells.GetLength(1));
        for (var row = 0; row < cells.GetLength(0); row++)
        {
            for (var col = 0; col < cells.GetLength(1); col++)
            {
                var cell = cells[row, col];
                var rect = new CaveCellControl(row, col, cell);
                Grid.SetRow(rect, row);
                Grid.SetColumn(rect, col);
                rectangles.Add(rect);
                Console.WriteLine("Rectangle " + row + ", " + col);
            }
        }
        
        Console.WriteLine("GetRectangles2");
        return rectangles;
    }

    private void _defineRowsAndColsUi()
    {
        var rowDefinitions = new RowDefinitions();
        var columnDefinitions = new ColumnDefinitions();
        for (var row = 0; row < Cave.Cells!.GetLength(0); row++)
        {
            var rowDefinition = new RowDefinition();
            rowDefinitions.Add(rowDefinition);
        }

        for (var col = 0; col < Cave.Cells.GetLength(1); col++)
        {
            var colDefinition = new ColumnDefinition();
            columnDefinitions.Add(colDefinition);
        }

        Dispatcher.UIThread.Post(() =>
        {
            CaveGrid!.RowDefinitions = rowDefinitions;
            CaveGrid!.ColumnDefinitions = columnDefinitions;
        });
    }

    private void _clearCaveUi()
    {
        Dispatcher.UIThread.Post(() =>
        {
            CaveGrid!.Children.Clear();
            CaveGrid!.RowDefinitions.Clear();
            CaveGrid!.ColumnDefinitions.Clear();
        });
    }
    
    public Cave Cave
    {
        get => GetValue(CaveProperty);
        set => SetValue(CaveProperty, value);
    }
    
    public static readonly StyledProperty<Cave> CaveProperty =
        AvaloniaProperty.Register<CaveControl, Cave>(nameof(Cave));
}