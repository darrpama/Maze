using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CaveModel;

namespace GuiInterface.Controls;

public class CaveCellControl : TemplatedControl
{
    public int Row { get; }

    public int Col { get; }
    public CaveCell Cell { get; }
    public CaveCellControl(int row, int col, CaveCell cell)
    {
        Row = row;
        Col = col;
        Cell = cell;
        IsVisible = cell.IsAlive;
        Cell.AliveHasSet += OnAliveHasSet;
    }

    private void OnAliveHasSet(object? sender, bool e)
    {
        var cell = sender as CaveCell;
        IsVisible = cell.IsAlive;
    }

}