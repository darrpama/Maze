using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Common.NumbersGenerator;
using ReactiveUI;

namespace GuiInterface.ViewModels;

using System.Threading;
using CaveModel;

public class CaveViewModel : ViewModelBase
{
    private Cave _cave;
    public Cave Cave
    {
        get;
        private set;
    }

    private CaveCell[,]? _caveCells;
    public CaveCell[,]? CaveCells
    {
        get => _caveCells;
        set => this.RaiseAndSetIfChanged(ref _caveCells, value);
    }
    
    public int LifeLimit { get; set; }
    public int DeathLimit { get; set; }
    public int MaxSize { get; } = 50;
    public int Size { get; set; }
    public int TimeStep
    {
        get => _timeStep;
        set => this.RaiseAndSetIfChanged(ref _timeStep, value);
    }
    private int _timeStep = 100;
    private bool _isAutoModeEnabled;
    
    
    public ReactiveCommand<Unit, Unit> GenerateCaveCommand { get; }
    public ReactiveCommand<Unit, Unit> NextStepCommand { get; }
    public ReactiveCommand<Unit, Unit> AutoCommand { get; }
    
    public ICommand ImportCaveFromFileCommand { get; }
    public ICommand ExportCaveToFileCommand { get; }

    public CaveViewModel()
    {
        var random = new Random();
        var randomGenerator = new RandomGenerator(random);
        var cave = new Cave(randomGenerator);
        Cave = cave;
        
        Cave.ChangeCave += _onCaveChanged;
        
        GenerateCaveCommand = ReactiveCommand.Create(GenerateCave);
        NextStepCommand = ReactiveCommand.Create(NextStep);
        AutoCommand = ReactiveCommand.Create(AutoMod);
        ImportCaveFromFileCommand = ReactiveCommand.CreateFromTask(ImportCave);
        ExportCaveToFileCommand = ReactiveCommand.CreateFromTask(ExportCave);
        Size = MaxSize;
        LifeLimit = 3;
        DeathLimit = 3;
    }

    private void _onCaveChanged(object? cave, CaveCell[,] cells)
    {
        CaveCells = cells;
    }

    private void GenerateCave()
    {
        Cave.Cols = Size;
        Cave.Rows = Size;
        Cave.LifeLimit = LifeLimit;
        Cave.DeathLimit = DeathLimit;
        Cave.GenerateInitial(new RandomGenerator(new Random()));
    }

    private void NextStep()
    {
        Cave.Step();
    }
    
    private async Task NextStepDelayed()
    {
        while (_isAutoModeEnabled)
        {
            Cave.Step();
            await Task.Delay(_timeStep);
        }
    }

    private void AutoMod()
    {
        if (!_isAutoModeEnabled)
        {
            _isAutoModeEnabled = true;
            Dispatcher.UIThread.Post(() => NextStepDelayed(), DispatcherPriority.Background);
        }
        else
        {
            _isAutoModeEnabled = false;
        }
    }

    private readonly Interaction<string?, string?> _importCaveInteraction;
    
    private readonly Interaction<string?, string?> _exportCaveInteraction;

    public Interaction<string?, string?> ImportCaveInteraction => _importCaveInteraction;
    
    public Interaction<string?, string?> ExportCaveInteraction => _exportCaveInteraction;

    private async Task ImportCave()
    {
        var importString = await _importCaveInteraction.Handle(null);
        if (importString == null)
            return;
        Cave.ImportString(importString);
    }

    private async Task ExportCave()
    {
        if (Cave.Cells is null) return;

        var caveString = Cave.ExportString();
    }
    
}