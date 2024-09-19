using Model.MazeGenerator;
using Model.NumbersGenerator;

namespace MazeTests;

public class MazeModelTests
{
    [Fact]
    public void Given_Generate_When_FourthRowsAndColsAsParameter_Then_ReturnRightMaze()
    {
        var numbers = new[]
        {
            0, 1, 0, 0, 1, 1, 0, 1, 0, 0,
            0, 0, 1, 1, 0, 1, 0, 1, 1, 0,
            1, 0, 1, 1, 0, 0, 0, 0, 0, 1,
            0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
            0, 0, 1, 0, 1, 1, 1, 0
        };
        IGenerator generator = new SequenceGenerator(numbers);
        var mazeGenerator = new MazeGenerator(generator);
        var result = mazeGenerator.Generate(4, 4);

        var expected = new MazePoint[,]
        {
            {new(false, false), new(true, true), new(true, false), new(false, true)},
            {new(false, true), new(false, false), new(true, false), new(true, true)},
            {new(true, false), new(false, true), new(false, false), new(true, true)},
            {new(true, false), new(true, false), new(true, false), new(true, true)},
        };
        Assert.Equal(result, expected);

    }
}