using Common.NumbersGenerator;

namespace CaveModel;

public class Cave
{

    public Cave()
    {
        
    }
    public Cave(IGenerator generator)
    {
        GenerateInitial(generator);
    }
    
    private int _rows = 1;
    public int Rows
    {
        get => _rows;
        set
        {
            RangeValidate(value, 1, 50);
            _rows = value;
        }
    }

    private int _cols = 1;
    public int Cols
    {
        get => _cols;
        set
        {
            RangeValidate(value, 1, 50);
            _cols = value;
        }
    }

    private int _lifeLimit;
    public int LifeLimit
    {
        get => _lifeLimit;
        set
        {
            RangeValidate(value, 0, 7);
            _lifeLimit = value;
        }
    }

    private int _deathLimit;
    public int DeathLimit
    {
        get => _deathLimit;
        set
        {
            RangeValidate(value, 0, 7);
            _deathLimit = value;
        }
    }
    
    private CaveCell[,]? _caveCells;
    public CaveCell[,]? Cells
    {
        get => _caveCells;
        private set
        {
            _caveCells = value;
        }
    }

    private void RangeValidate(int value, int min, int max)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException($"Value must be between {min} and {max}.");
    }

    public void GenerateInitial(IGenerator generator)
    {
        Cells = new CaveCell[Rows,Cols];
        
        for (var row = 0; row < Rows; row++)
        {
            for (var col = 0; col < Cols; col++)
            {
                Cells[row, col] = new CaveCell();
                Cells[row, col].SetAlive(generator.NextBool());
            }
        }
        OnChangeCave(Cells);
    }

    public void Step()
    {
        ArgumentNullException.ThrowIfNull(Cells, "Cells is null");
        var cellsCopy = CopyCells();
        
        for (var row = 0; row < Rows; row++)
        {
            for (var col = 0; col < Cols; col++)
            {
                if (Cells[row, col].IsAlive)
                {
                    if (GetAliveCount(row, col) < DeathLimit)
                        cellsCopy[row, col].MakeDeath();
                }
                else
                {
                    if (GetAliveCount(row, col) > LifeLimit)
                        cellsCopy[row, col].MakeAlive();
                }
            }
        }
        SetCellsByCopy(cellsCopy);
    }

    private void SetCellsByCopy(CaveCell[,] cellsCopy)
    {
        ArgumentNullException.ThrowIfNull(Cells, "Cells is null");

        for (var row = 0; row < Rows; row++)
        {
            for (var col = 0; col < Cols; col++)
            {
                Cells[row, col].SetAlive(cellsCopy[row, col].IsAlive);
            }
        }
    }

    public CaveCell[,] CopyCells()
    {
        ArgumentNullException.ThrowIfNull(Cells, "Cells is null");

        var copy = new CaveCell?[Rows, Cols];
        for (var row = 0; row < Cells.GetLength(0); row++)
        {
            for (var col = 0; col < Cells.GetLength(1); col++)
            {
                copy[row, col] = Cells[row, col].Clone() as CaveCell;
            }
        }

        return copy;
    }
    
    private int GetAliveCount(int row, int col)
    {
        return new List<bool>
        {
            CheckCellAlive(row-1, col-1),
            CheckCellAlive(row-1, col),
            CheckCellAlive(row-1, col+1),
            CheckCellAlive(row, col+1),
            CheckCellAlive(row+1, col+1),
            CheckCellAlive(row+1, col),
            CheckCellAlive(row+1, col-1),
            CheckCellAlive(row, col-1),
        }.Count(val => val);
         

    }

    private bool CheckCellAlive(int row, int col)
    {
        ArgumentNullException.ThrowIfNull(Cells, "Cells is null");
        if (row < 0 || row >= Rows || col < 0 || col >= Cols)
        {
            return true;
        }
        return Cells[row, col].IsAlive;
    }

    public void ImportString(string caveString)
    {
        var importer = new StringCaveImporter(caveString);

        Cells = importer.Import();
        Rows = Cells.GetLength(0);
        Cols = Cells.GetLength(1);
    }

    public string ExportString()
    {
        if (Cells == null) throw new ArgumentNullException(nameof(Cells));
        // var exporter = new CaveStringExporter();
        // return exporter.Export(Cells);
        return "Aboba";
    }

    public static Cave FromString(string caveString)
    {
        var cave = new Cave();
        cave.ImportString(caveString);
        return cave;
    }
    
    public event EventHandler<CaveCell[,]>? ChangeCave;

    private void OnChangeCave(CaveCell[,] cells)
    {
        ChangeCave?.Invoke(this, cells);
    }
}