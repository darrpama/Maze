using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CaveModel;
using Common.NumbersGenerator;
using ReactiveUI;

namespace GuiInterface.ViewModels;

using CaveModel;

public class CaveViewModel : ViewModelBase
{
    private Cave _cave;
    public Cave Cave
    {
        get => _cave;
        private set => _cave = value;
    }

    private CaveCell[,]? _caveCells;
    public CaveCell[,]? CaveCells
    {
        get => _caveCells;
        set => this.RaiseAndSetIfChanged(ref _caveCells, value);
    }

    public int MaxSize { get; } = 50;
    public int Size { get; set; }
    
    public ReactiveCommand<Unit, Unit> GenerateCaveCommand { get; }
    
    public ICommand ImportCaveFromFileCommand { get; }
    public ICommand ExportCaveToFileCommand { get; }

    public CaveViewModel()
    {
        Cave = new Cave();
        var random = new Random();
        var randomGenerator = new RandomGenerator(random);
        Cave.GenerateInitial(randomGenerator);

        Cave.ChangeCave += _onCaveChanged;
        
        GenerateCaveCommand = ReactiveCommand.Create(GenerateCave);
        ImportCaveFromFileCommand = ReactiveCommand.CreateFromTask(ImportCave);
        ExportCaveToFileCommand = ReactiveCommand.CreateFromTask(ExportCave);
        
        Size = MaxSize;
    }

    private void _onCaveChanged(object? cave, CaveCell[,] cells)
    {
        CaveCells = cells;
    }

    private void GenerateCave()
    {
        var random = new Random();
        var randomGenerator = new RandomGenerator(random);
        Cave.GenerateInitial(randomGenerator);
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