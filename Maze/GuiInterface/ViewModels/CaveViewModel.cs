using System;
using CaveModel;
using Common.NumbersGenerator;

namespace GuiInterface.ViewModels;

using CaveModel;

public class CaveViewModel : ViewModelBase
{
    private Cave _cave;
    public Cave Cave
    {
        get => _cave;
        private set => _cave= value;
    }

    public CaveViewModel()
    {
        _cave = new Cave();
        var random = new Random();
        var randomGenerator = new RandomGenerator(random);
        _cave.GenerateInitial(randomGenerator);
        
    }
    
}