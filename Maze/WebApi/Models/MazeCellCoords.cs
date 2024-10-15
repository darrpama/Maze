namespace WebApi.Models;

public class MazeCellCoords
{
    public MazeCellCoords()
    {
        
    }
    public MazeCellCoords(int row, int col)
    {
        Row = row;
        Col = col;
    }
    public int Row { get; set; }
    public int Col { get; set; }
}