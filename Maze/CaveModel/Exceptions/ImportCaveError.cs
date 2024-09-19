namespace CaveModel.Exceptions;

public class ImportCaveError : Exception
{
    public ImportCaveError()
    {
    }

    public ImportCaveError(string message) : base(message)
    {
    }

    public ImportCaveError(string message, Exception inner) : base(message, inner)
    {
    }
}