namespace CaveModel.Exporters;

public interface ICaveExporter
{
    public string Export(CaveCell[,] cave);
}