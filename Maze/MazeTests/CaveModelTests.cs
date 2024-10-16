using CaveModel;
using Common.NumbersGenerator;

namespace MazeTests;

public class CaveModelTests
{
    [Fact]
    public void Given_Generate_When_FourthRowsAndColsAsParameter_Then_CallGenerateInitialReturnsRightCave()
    {
        var numbers = new[]
        {
            0, 1, 0, 1,
            1, 0, 0, 1,
            0, 1, 0, 0,
            0, 0, 1, 1
        };
        IGenerator generator = new SequenceGenerator(numbers);
        var cave = new Cave { Rows = 4, Cols = 4 };
        cave.GenerateInitial(generator);
        var expected = new CaveCell[,]
        {
            { new(false), new(true), new(false), new(true) },
            { new(true), new(false), new(false), new(true) },
            { new(false), new(true), new(false), new(false) },
            { new(false), new(false), new(true), new(true) }
        };
        Assert.Equal(cave.Cells, expected);
    }

    [Fact]
    public void Given_Cave_When_StepCalled_Then_StepChangingCaveCorrectly()
    {
        const string importString = """
        10 10
        1 0 1 0 0 0 0 1 1 0 
        0 0 1 1 0 0 0 0 0 1 
        0 0 1 0 1 0 1 1 0 1 
        0 1 1 1 1 1 1 0 0 0 
        0 0 0 1 1 0 0 1 1 1 
        0 1 0 1 0 1 0 0 0 0 
        1 1 0 0 0 0 0 1 0 0 
        0 0 0 0 0 0 1 0 1 1 
        1 0 0 0 0 1 1 0 0 0 
        0 1 1 0 0 1 1 0 0 0 
        """;
        var cave = new Cave
        {
            DeathLimit = 3,
            LifeLimit = 4
        };
        cave.ImportString(importString);
        for (var i = 0; i < 6; i++)
            cave.Step();

        var expectedString = """
        10 10
        1 1 1 1 1 1 1 1 1 1 
        1 1 1 1 1 1 1 1 1 1 
        1 1 1 1 1 1 1 1 1 1 
        1 1 1 1 1 1 1 1 1 1 
        1 1 1 1 1 1 0 0 1 1 
        1 1 0 0 0 0 0 0 0 1 
        1 0 0 0 0 0 0 0 0 1 
        1 0 0 0 0 0 0 0 1 1 
        1 1 0 0 0 1 1 1 1 1 
        1 1 1 1 1 1 1 1 1 1
        """;
        var expectedCave = Cave.FromString(expectedString);

        Assert.Equal(cave.Cells, expectedCave.Cells);
    }
    
    [Fact]
    public void Given_RandomGenerate_When_FourthRowsAndColsAsParameter_Then_CallGenerateInitialReturnsRightCave()
    {
        IGenerator generator = new RandomGenerator(random:new Random());
        var cave = new Cave { Rows = 4, Cols = 4 };
        cave.GenerateInitial(generator);
        Assert.Equal(4, cave.Cells?.GetLength(0));
        Assert.Equal(4, cave.Cells?.GetLength(1));
        // for (var i = 0; i < cave.Cells?.GetLength(0); i++)
        // {
        //     for (var j = 0; j < cave.Cells?.GetLength(1); j++)
        //     {
        //         Console.Write($"{cave.Cells[i, j].IsAlive}");
        //     }
        //     Console.WriteLine();
        // }
    }
}