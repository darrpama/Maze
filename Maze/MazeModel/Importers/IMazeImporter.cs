using MazeModel.Maze;
using MazeModel.MazeGenerator;

namespace MazeModel.Importers;

public interface IMazeImporter
{
    public MazePoint[,] Import();
}