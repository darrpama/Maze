using System.Text;

namespace CaveModel.Exporters;

public class CaveStringExporter : ICaveExporter
{
    public string Export(CaveCell[,] cave)
    {
        var sizeSb = new StringBuilder();
        sizeSb.Append($"{cave.GetLength(1)} {cave.GetLength(0)}\n");

        var sb = new StringBuilder();
        for (var i = 0; i < cave.GetLength(0); i++)
        {
            for (var j = 0; j < cave.GetLength(1); j++)
            {
                var cell = cave[i, j];
                
                var isAlive = Convert.ToInt32(cell.IsAlive);
                
                sb.Append(isAlive);
                
                if (j == cave.GetLength(0) - 1) continue;
                
                sb.Append(' ');
            }
            
            if (i == cave.GetLength(1) - 1) continue;
 
            sb.Append('\n');
        }

        return sizeSb.Append(sb).ToString();
    }
}