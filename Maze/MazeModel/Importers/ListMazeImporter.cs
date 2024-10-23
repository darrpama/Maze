using MazeModel.Exceptions;
using MazeModel.Maze;

namespace MazeModel.Importers;

public class ListMazeImporter : IMazeImporter
{
    public List<List<MazePointWalls>> Walls { get; }
    
    public ListMazeImporter(List<List<MazePointWalls>> walls)
    {
        Walls = walls;
    }


    public MazePoint[,] Import()
    {
        Validate();
        var rowsCount = Walls.Count;
        var colsCount = Walls[0].Count;
        var points = new MazePoint[rowsCount, colsCount];

        for (var row = 0; row < rowsCount; row++)
        {
            for (var col = 0; col < colsCount; col++)
            {
                points[row, col] = new MazePoint(Walls[row][col].Down, Walls[row][col].Right, row, col);
            }
        }

        return points;
    }

    private void Validate()
    {
        var rows = Walls.Count;
        if (rows < 1)
        {
            throw new ImportMazeError("Maze size should be between 1 and 50.");
        }
        
        var cols = Walls[0].Count;
        
        if (
            rows > 50 || cols > 50 || cols < 1
        )
        {
            throw new ImportMazeError("Maze size should be between 1 and 50.");
        }

        if (Walls.Any(col => col.Count != cols))
        {
            throw new ImportMazeError("Maze should be square.");
        }
    }
}