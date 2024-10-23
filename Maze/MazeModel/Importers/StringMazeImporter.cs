using MazeModel.Exceptions;
using MazeModel.Maze;
using MazeModel.MazeGenerator;

namespace MazeModel.Importers;

public class StringMazeImporter : IMazeImporter
{
    private string _stringMaze;

    public StringMazeImporter(string stringMaze)
    {
        _stringMaze = stringMaze;
    }

    public MazePoint[,] Import()
    {
        var linesEnumerator = _stringMaze.Split("\n").ToList().GetEnumerator();
        linesEnumerator.MoveNext();

        var sizeLine = linesEnumerator.Current;
        var size = GetSize(sizeLine);
        if (
            size.Item1 > 50 || size.Item2 > 50 ||
            size.Item1 < 1 || size.Item2 < 1
            )
        {
            throw new ImportMazeError("Maze size should be between 1 and 50.");
        }

        var maze = new MazePoint[size.Item1, size.Item2];
        InitMaze(maze);
        var sideFlag = SideFlag.Right;

        var lineCounter = 0;
        while (linesEnumerator.MoveNext())
        {
            var line = linesEnumerator.Current;
            if (line.Trim() == "")
            {
                sideFlag = SideFlag.Down;
                lineCounter = 0;
                continue;
            }

            ParseLine(maze, line, lineCounter, sideFlag);
            lineCounter++;

        }
        if (sideFlag != SideFlag.Down)
            throw new ImportMazeError("Maze should contains two sections with down and righ sides.");


        return maze;
    }

    private void InitMaze(MazePoint[,] maze)
    {
        for (var i = 0; i < maze.GetLength(0); i++)
        {
            for (var j = 0; j < maze.GetLength(1); j++)
            {
                maze[i, j] = new MazePoint(false, false, i, j);
            }
        }
    }

    private void ParseLine(MazePoint[,] maze, string line, int lineIndex, SideFlag sideFlag)
    {
        int[] numbers;
        try
        {
            numbers = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        }
        catch (FormatException e)
        {
            throw new ImportMazeError(e.Message);
        }
        if (maze.GetLength(1) != numbers.Length)
            throw new ImportMazeError("Maze must have the same number of elements with size.");
        for (var i = 0; i < maze.GetLength(1); i++)
        {
            if (!(numbers[i] == 0 || numbers[i] == 1))
                throw new ImportMazeError("Maze must have 0 or 1 as number");
            var boolNumber = numbers[i] == 1;
            
            switch (sideFlag)
            {
                case SideFlag.Down:
                    maze[lineIndex, i].Down = boolNumber;
                    break;
                case SideFlag.Right:
                    maze[lineIndex, i].Right = boolNumber;
                    break;
            }
        }
    }

    private (int, int) GetSize(string sizeLine)
    {
        var values = sizeLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (values.Length != 2)
            throw new ImportMazeError("Maze size should contains only two numbers.");
        return (int.Parse(values[0]), int.Parse(values[1]));
    }
}