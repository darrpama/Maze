namespace CaveModel.Importers;

public interface ICaveImporter
{
    public CaveCell[,] Import();
}