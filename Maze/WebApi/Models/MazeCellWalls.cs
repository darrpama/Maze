namespace WebApi.Models;

public class MazeCellWalls
{
    public MazeCellWalls()
    {
        
    }
    public MazeCellWalls(bool right, bool down)
    {
        Right = right;
        Down = down;
    }
    public bool Right { get; set; }
    public bool Down { get; set; }
}