using MazeModel.Maze;
using MazeModel.MazeGenerator;

namespace MazeModel.Exporters;

public interface IMazeExport
{
    public void Export(MazePoint[,] maze);
}