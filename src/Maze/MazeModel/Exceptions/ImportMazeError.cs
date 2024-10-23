namespace MazeModel.Exceptions;

public class ImportMazeError: Exception
{
    public ImportMazeError()
    {
    }

    public ImportMazeError(string message) : base(message)
    {
    }

    public ImportMazeError(string message, Exception inner) : base(message, inner)
    {
    }
}