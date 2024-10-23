using Common.NumbersGenerator;
using MazeModel.Maze;

namespace MazeModel.MazeGenerator;

public class MazeGenerator
{
    private IGenerator _generator;
    public MazeGenerator(IGenerator generator)
    {
        _generator = generator;
    }

    public MazePoint[,] Generate(int rows, int cols)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rows, 50);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cols, 50);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rows);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cols);
        
        var maze = new MazePoint[rows, cols];
        var row = new (int, MazePoint)[cols];
        var setCounter = cols + 1;
        InitializeMaze(maze);
        InitializeFillingRow(row);
        for (var i = 0; i < rows; i++)
        {
            StepGenerate(row, _generator);
            if (i < rows - 1)
            {
                CopyFillingRowToMaze(row, maze, i);
                PrepareRowForNextStep(row, ref setCounter);
            }
            else
            {
                EndRow(row);
                CopyFillingRowToMaze(row, maze, i);
            }
        }

        MazeFillRightWalls(maze);

        return maze;
    }

    private void MazeFillRightWalls(MazePoint[,] maze)
    {
        for (var i = 0; i < maze.GetLength(0); i++)
        {
            maze[i, maze.GetLength(1) - 1].Right = true;
        }
    }

    private void EndRow((int, MazePoint)[] row)
    {
        for (var i = 0; i < row.Length - 1; i++)
        {
            if (row[i].Item1 != row[i + 1].Item1)
            {
                row[i].Item2.Right = false;
            }
            JoinSets(row, row[i].Item1, row[i+1].Item1);
        }
        for (var i = 0; i < row.Length; i++)
        {
            row[i].Item2.Down = true;
        }
    }

    private void PrepareRowForNextStep((int, MazePoint)[] row, ref int setCounter)
    {
        RemoveRightWalls(row);
        ClearSetWhereDownWalls(row);
        FillEmptySets(row, ref setCounter);
    }

    private void FillEmptySets((int, MazePoint)[] row, ref int setCounter)
    {
        for (var i = 0; i < row.Length; i++)
        {
            if (row[i].Item1 == 0)
            {
                row[i].Item1 = setCounter++;
            }
        }
    }

    private void ClearSetWhereDownWalls((int, MazePoint)[] row)
    {
        for (var i = 0; i < row.Length; i++)
        {
            if (!row[i].Item2.Down) continue;
            row[i].Item2.Down = false;
            row[i].Item1 = 0;
        }
    }

    private void RemoveRightWalls((int, MazePoint)[] row)
    {
        for (var index = 0; index < row.Length; index++)
        {
            row[index].Item2.Right = false;
        }
    }

    private static void CopyFillingRowToMaze((int, MazePoint)[] row, MazePoint[,] maze, int rowNumber)
    {
        for (var i = 0; i < row.Length; i++)
        {
            maze[rowNumber, i].Down = row[i].Item2.Down;
            maze[rowNumber, i].Right = row[i].Item2.Right;
        }
    }

    private void StepGenerate((int, MazePoint)[] row, IGenerator generator)
    {
        RightWallsGenerate(row, generator);

        DownWallsGenerate(row, generator);
    }

    private void DownWallsGenerate((int, MazePoint)[] row, IGenerator generator)
    {
        for (var i = 0; i < row.Length; i++)
        {
            var generatedValue = generator.NextBool();
            if (!generatedValue) continue;
            var countWithoutDownWall = GetCountWithoutDownWallFromSet(row, row[i].Item1);
            if (countWithoutDownWall > 1)
            {
                row[i].Item2.Down = true;
            }
        }
    }

    private int GetCountWithoutDownWallFromSet((int, MazePoint)[] row, int setNumber)
    {
        return row.Count(cell => cell.Item1 == setNumber && !cell.Item2.Down);
    }

    private void RightWallsGenerate((int, MazePoint)[] row, IGenerator generator)
    {
        for (var i = 0; i < row.Length - 1; i++)
        {
            var generatedValue = generator.NextBool();
            if (generatedValue || row[i].Item1 == row[i + 1].Item1)
            {
                row[i].Item2.Right = true;
                continue;
            }

            JoinSets(row, row[i].Item1, row[i + 1].Item1);
        }
    }

    private void JoinSets((int, MazePoint)[] row, int sourceSetValue, int destSetValue)
    {
        for (var i = 0; i < row.Length; i++)
        {
            if (row[i].Item1 == destSetValue)
            {
                row[i].Item1 = sourceSetValue;
            }
        }
    }

    private void InitializeFillingRow((int, MazePoint)[] row)
    {
        for (var i = 0; i < row.Length; i++)
        {
            row[i].Item1 = i + 1;
            row[i].Item2 = new MazePoint(false, false, 0, 0);
        }
    }

    private static void InitializeMaze(MazePoint[,] toGenerate)
    {
        for (var i = 0; i < toGenerate.GetLength(0); i++)
        {
            for (var j = 0; j < toGenerate.GetLength(1); j++)
            {
                toGenerate[i, j] = new MazePoint(false, false, i, j);
            }
        }
    }
}