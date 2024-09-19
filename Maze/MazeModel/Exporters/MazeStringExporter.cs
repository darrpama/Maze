using System.Text;
using MazeModel.Maze;
using MazeModel.MazeGenerator;

namespace MazeModel.Exporters;

public class MazeStringExporter
{
    public string Export(MazePoint[,] maze)
    {
        var sizeSb = new StringBuilder();
        sizeSb.Append($"{maze.GetLength(1)} {maze.GetLength(0)}\n");

        var rightSb = new StringBuilder();
        var downSb = new StringBuilder();
        for (var i = 0; i < maze.GetLength(0); i++)
        {
            for (var j = 0; j < maze.GetLength(1); j++)
            {
                var point = maze[i, j];
                
                var right = Convert.ToInt32(point.Right);
                var down = Convert.ToInt32(point.Down);
                
                rightSb.Append(right);
                downSb.Append(down);
                
                if (j == maze.GetLength(0) - 1) continue;
                
                rightSb.Append(' ');
                downSb.Append(' ');
            }
            
            if (i == maze.GetLength(1) - 1) continue;
 
            rightSb.Append('\n');
            downSb.Append('\n');
        }

        return sizeSb.Append(rightSb).Append("\n\n").Append(downSb).ToString();
    }
}