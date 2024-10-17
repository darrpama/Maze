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
        IsCellVisible = isAlive;
    }
    
    public bool IsCellVisible
    {
        get => GetValue(IsCellVisibleProperty);
        set => SetValue(IsCellVisibleProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsCellVisibleProperty =
        AvaloniaProperty.Register<CaveCellControl, bool>(nameof(IsCellVisible), false);
}