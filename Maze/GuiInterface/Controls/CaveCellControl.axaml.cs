using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace GuiInterface.Controls;

public class CaveCellControl : TemplatedControl
{
    public int Row { get; }

    public int Col { get; }
    public CaveCellControl(int row, int col, bool isAlive)
    {
        Row = row;
        Col = col;
        IsCircleVisible = isAlive;
    }
    
    public bool IsCircleVisible
    {
        get => GetValue(IsCircleVisibleProperty);
        set => SetValue(IsCircleVisibleProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsCircleVisibleProperty =
        AvaloniaProperty.Register<CaveCellControl, bool>(nameof(IsCircleVisible), false);
}