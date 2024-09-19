namespace MazeModel.Maze;

public class MazePointComparer: IComparer<MazePoint>
{
    public int Compare(MazePoint? x, MazePoint? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (y is null) return 1;
        if (x is null) return -1;
        if (x.F == y.F) return 0;
        if (x.F > y.F) return -1;
        
        return 1;
    }
}