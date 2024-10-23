using CaveModel.Exceptions;
using CaveModel.Exporters;

namespace CaveModel.Importers;

public class CaveImporter(string stringCave) : ICaveImporter
{
    public CaveCell[,] Import()
    {
        var linesEnumerator = stringCave.Split("\n").ToList().GetEnumerator();
        linesEnumerator.MoveNext();

        var sizeLine = linesEnumerator.Current;
        var size = GetSize(sizeLine);
        if (size.Item1 > 50 || size.Item2 > 50)
        {
            throw new ImportCaveError();
        }

        var cave = new CaveCell[size.Item1, size.Item2];
        InitCave(cave);

        var lineCounter = 0;
        while (linesEnumerator.MoveNext())
        {
            var line = linesEnumerator.Current;
            ParseLine(cave, line, lineCounter);
            lineCounter++;
        }

        return cave;
    }

    private void InitCave(CaveCell[,] cave)
    {
        for (var i = 0; i < cave.GetLength(0); i++)
        {
            for (var j = 0; j < cave.GetLength(1); j++)
            {
                cave[i, j] = new CaveCell();
            }
        }
    }

    private void ParseLine(CaveCell[,] cave, string line, int lineIndex)
    {
        var numbers = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        if (cave.GetLength(1) != numbers.Length)
            throw new ImportCaveError();
        for (var i = 0; i < cave.GetLength(1); i++)
        {
            if (!(numbers[i] == 0 || numbers[i] == 1))
                throw new ImportCaveError();
            
            var boolNumber = numbers[i] == 1;
            cave[lineIndex, i].SetAlive(boolNumber);
        }
    }

    private (int, int) GetSize(string sizeLine)
    {
        var values = sizeLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (values.Length != 2)
            throw new ImportCaveError();
        return (int.Parse(values[0]), int.Parse(values[1]));
    }
}