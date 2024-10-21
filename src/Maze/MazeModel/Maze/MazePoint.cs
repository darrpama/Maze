namespace MazeModel.Maze;

public enum Direction
{
    Top,
    Down,
    Right,
    Left
}

public class MazePoint
{
    public int Row { get; }
    public int Col { get; }
    public int G = 0;
    public int H = 0;
    public int F => G + H;
    

    public Direction? PathFrom { get; set; }
    public Direction? PathTo { get; set; }

    public List<MazePoint> Neighbors = [];

    public MazePoint? Previous = null;

    public MazePoint(bool down, bool right, int row, int col)
    {
        Down = down;
        Right = right;
        Row = row;
        Col = col;
    }

    public bool Down { get; set; }
    public bool Right { get; set; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Col);
    }
};