using ReactiveUI;

namespace GuiInterface.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    private ViewModelBase _contentViewModel;
    
    public MainWindowViewModel()
    {
        Maze = new MazeViewModel();
        _contentViewModel = Maze;
    }
    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }
    public MazeViewModel Maze { get; }
    
#pragma warning restore CA1822 // Mark members as static
}