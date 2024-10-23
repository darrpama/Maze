using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace GuiInterface.Controls;

public enum PathPartTypeEnum
{
    Start,
    End,
    PathPart
}

public class MazeCellControl : TemplatedControl
{
    public int Row { get; }

    public int Col { get; }

    public MazeCellControl(int row, int col, bool rightBorder, bool downBorder)
    {
        Row = row;
        Col = col;
        InternalBorderThickness =
            new Thickness(0, 0, rightBorder ? 2 : 0, downBorder ? 2 : 0);

        _lineBrush = new SolidColorBrush(new Color(255, 0, 0, 0));
        BorderBrush = _lineBrush;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        var point = e.GetCurrentPoint(this);
        if (point.Properties.IsLeftButtonPressed)
        {
            var click = new RoutedEventArgs(LeftClickEvent);
            RaiseEvent(click);
        }

        
        if (point.Properties.IsRightButtonPressed)
        {
            var click = new RoutedEventArgs(RightClickEvent);
            RaiseEvent(click);
        }

        e.Handled = true;
    }

    public static readonly StyledProperty<Thickness> InternalBorderThicknessProperty =
        AvaloniaProperty.Register<MazeCellControl, Thickness>(nameof(InternalBorderThickness));

    public Thickness InternalBorderThickness
    {
        get => GetValue(InternalBorderThicknessProperty);
        set => SetValue(InternalBorderThicknessProperty, value);
    }

    public void ShowPathPart(PathPartTypeEnum pathPartType)
    {
        IsCircleVisible = true;
        
        switch (pathPartType)
        {
            case PathPartTypeEnum.Start:
                PathPartCircleColor = Brush.Parse("blue");
                break;
            case PathPartTypeEnum.End:
                PathPartCircleColor = Brush.Parse("green");
                break; 
            case PathPartTypeEnum.PathPart:
                PathPartCircleColor = Brush.Parse("red");
                break; 
                
        }
    }

    public IBrush PathPartCircleColor
    {
        get => GetValue(PathPartCircleColorProperty);
        set => SetValue(PathPartCircleColorProperty, value);
    }

    public bool IsCircleVisible
    {
        get => GetValue(IsCircleVisibleProperty);
        set => SetValue(IsCircleVisibleProperty, value);
    }

    public static readonly StyledProperty<IBrush> PathPartCircleColorProperty =
        AvaloniaProperty.Register<MazeCellControl, IBrush>(nameof(PathPartCircleColor));

    public static readonly StyledProperty<bool> IsCircleVisibleProperty =
        AvaloniaProperty.Register<MazeCellControl, bool>(nameof(IsCircleVisible), false);


    public event EventHandler<RoutedEventArgs>? RightClick
    {
        add => AddHandler(RightClickEvent, value);
        remove => RemoveHandler(RightClickEvent, value);
    }

    public static readonly RoutedEvent<RoutedEventArgs> RightClickEvent =
        RoutedEvent.Register<MazeCellControl, RoutedEventArgs>(nameof(RightClick), RoutingStrategies.Bubble);
    
    public event EventHandler<RoutedEventArgs>? LeftClick
    {
        add => AddHandler(LeftClickEvent, value);
        remove => RemoveHandler(LeftClickEvent, value);
    }

    public static readonly RoutedEvent<RoutedEventArgs> LeftClickEvent =
        RoutedEvent.Register<MazeCellControl, RoutedEventArgs>(nameof(LeftClick), RoutingStrategies.Bubble);

    private readonly IBrush _lineBrush;
}