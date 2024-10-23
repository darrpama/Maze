using MazeModel.Maze;

namespace MazeModel.Exporters;

public class MazeListExporter
{
    public List<List<MazePointWalls>> Export(MazePoint[,] points)
    {
        var exportMaze = new List<List<MazePointWalls>>();
        var rows = points.GetLength(0);
        var cols = points.GetLength(1);
            
        for (var i = 0; i < rows; i++)
        {
            var row = new List<MazePointWalls>();
            for (var j = 0; j < cols; j++)
            {
                var point = points[i, j];

                row.Add(new MazePointWalls(point.Right, point.Down));
            }

            exportMaze.Add(row);
        }
        return exportMaze;
    }
}