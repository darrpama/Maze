using MazeModel.Maze;
using MazeModel.MazeGenerator;

namespace MazeModel.Exporters;

public interface IMazeExporter
{
    public string Export(MazePoint[,] maze);
}