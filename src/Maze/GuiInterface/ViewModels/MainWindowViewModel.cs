using ReactiveUI;

namespace GuiInterface.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    private ViewModelBase _mazeContentViewModel;
    private ViewModelBase _caveContentViewModel;
    
    public MainWindowViewModel()
    {
        Maze = new MazeViewModel();
        Cave = new CaveViewModel();
        _mazeContentViewModel = Maze;
        _caveContentViewModel = Cave;
    }
    public ViewModelBase MazeContentViewModel
    {
        get => _mazeContentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _mazeContentViewModel, value);
    }
    
    public ViewModelBase CaveContentViewModel
    {
        get => _caveContentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _caveContentViewModel, value);
    }
    public MazeViewModel Maze { get; }
    public CaveViewModel Cave { get; }
    
#pragma warning restore CA1822 // Mark members as static
}