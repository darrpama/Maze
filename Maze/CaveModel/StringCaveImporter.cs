using CaveModel.Exceptions;

namespace CaveModel;

public class StringCaveImporter
{
    private string _stringCave;

    public StringCaveImporter(string stringCave)
    {
        _stringCave = stringCave;
    }

    public CaveCell[,] Import()
    {
        var linesEnumerator = _stringCave.Split("\n").ToList().GetEnumerator();
        linesEnumerator.MoveNext();
        while (linesEnumerator.Current.Trim() == "")
        {
            linesEnumerator.MoveNext();
        }

        var sizeLine = linesEnumerator.Current;
        var size = GetSize(sizeLine);
        if (
            size.Item1 > 50 || size.Item2 > 50 ||
            size.Item1 < 1 || size.Item2 < 1
            )
        {
            throw new ImportCaveError("The file size is incorrect (must be from 1 to 50).");
        }

        var cave = new CaveCell[size.Item1, size.Item2];
        InitCave(cave);

        var lineCounter = 0;
        while (linesEnumerator.MoveNext())
        {
            var line = linesEnumerator.Current;
            if (line.Trim() == "")
            {
                continue;
            }

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
            throw new ImportCaveError("Length of line is incorrect.");
        for (var i = 0; i < cave.GetLength(1); i++)
        {
            if (!(numbers[i] == 0 || numbers[i] == 1))
                throw new ImportCaveError("Value must be 0 or 1.");
            var boolNumber = numbers[i] == 1;
            
            cave[lineIndex, i].SetAlive(boolNumber);
        }
    }

    private (int, int) GetSize(string sizeLine)
    {
        var values = sizeLine.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (values.Length != 2)
            throw new ImportCaveError("String cave size is invalid.");
        return (int.Parse(values[0]), int.Parse(values[1]));
    }

}