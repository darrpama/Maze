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
            { new(false, false, 0, 0), new(true, true, 0, 1), new(true, false, 0, 2), new(false, true, 0, 3) },
            { new(false, true, 1, 0), new(false, false, 1, 1), new(true, false, 1, 2), new(true, true, 1, 3) },
            { new(true, false, 2, 0), new(false, true, 2, 1), new(false, false, 2, 2), new(true, true, 2, 3) },
            { new(true, false, 3, 0), new(true, false, 3, 1), new(true, false, 3, 2), new(true, true, 3, 3) },
        };

        Assert.Equal(4, result.GetLength(0));
        Assert.Equal(4, result.GetLength(1));
        for (var row = 0; row < result.GetLength(0); row++)
        {
            for (var col = 0; col < result.GetLength(1); col++)
            {
                Assert.Equal(
                    expected[row, col],
                    result[row, col],
                    (c1, c2) => c1.Down.Equals(c2.Down) &&
                                c1.Right.Equals(c2.Right) &&
                                c1.Row.Equals(c2.Row) &&
                                c1.Col.Equals(c2.Col));
            }
        }
    }
}