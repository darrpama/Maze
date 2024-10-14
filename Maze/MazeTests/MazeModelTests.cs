using MazeModel.Maze;
using MazeModel.MazeGenerator;
using Common.NumbersGenerator;

namespace MazeTests;

public class MazeModelTests
{
    [Fact]
    public void Given_Generate_When_FourthRowsAndColsAsParameter_Then_ReturnRightMaze()
    {
        var numbers = new[]
        {
            0, 0, 0, 1,
            1, 0, 1, 1,
            0, 1, 0, 1,
            0, 0, 0, 1,
            1, 0, 1, 0,
            0, 0, 1, 0,
            1, 1, 0, 1,
            1, 1, 1, 1
        };
        
        IGenerator generator = new SequenceGenerator(numbers);
        var mazeGenerator = new MazeGenerator(generator);
        var result = mazeGenerator.Generate(4, 4);

        var expected = new MazePoint[,]
        {
            {new(true,false,0,0),new(false,false,0,1),  new(true,false,0,2), new(false,true,0,3)},
            {new(false,true,1,0), new(false,false,1,1),new(true,true,1,2 ),new(false,true,1,3)},
            {new(true,false,2,0), new(true,true,2,1), new(false,false,2,2),new(true,true,2,3)},
            {new(true,false,3,0), new(true,false,3,1 ),new(true,false,3,2), new(true,true,3,3)},
        };
        
        Assert.Equal(4, result.GetLength(0));
        Assert.Equal(4, result.GetLength(1));
        Assert.Equal(expected, result);
        
    //     for (var i = 0; i < result.GetLength(0); i++)
    //     {
    //         for (var j = 0; j < result.GetLength(1); j++)
    //         {
    //             Console.Write(result[i, j].F);
    //             Console.Write(" ");
    //         }
    //         Console.WriteLine();
    //     }
    }
}