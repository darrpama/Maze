using MazeModel.Exceptions;
using MazeModel.Maze;
using MazeModel.MazeGenerator;

namespace MazeModel.Importers;

public class StringMazeImporter(string stringMaze) : IMazeImporter
{
    public MazePoint[,] Import()
    {
        var linesEnumerator = stringMaze.Split("\n").ToList().GetEnumerator();
        linesEnumerator.MoveNext();

        var sizeLine = linesEnumerator.Current;
        var size = GetSize(sizeLine);
        if (size.Item1 > 50 || size.Item2 > 50)
        {
            throw new ImportMazeError();
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
            throw new ImportMazeError();


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
        var numbers = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        if (maze.GetLength(1) != numbers.Length)
            throw new ImportMazeError();
        for (var i = 0; i < maze.GetLength(1); i++)
        {
            if (!(numbers[i] == 0 || numbers[i] == 1))
                throw new ImportMazeError();
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
            throw new ImportMazeError();
        return (int.Parse(values[0]), int.Parse(values[1]));
    }
}