using Common.NumbersGenerator;

namespace CaveModel;

public class Cave
{
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
    
    public CaveCell[,]? Cells { get; private set; }

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
    }

    public void Step()
    {
        ArgumentNullException.ThrowIfNull(Cells, "Cells is null");
        for (var row = 0; row < Rows; row++)
        {
            for (var col = 0; col < Cols; col++)
            {
                if (Cells[row, col].IsAlive)
                {
                    if (GetDeathCount(row, col) > DeathLimit)
                        Cells[row, col].MakeDeath();
                }
                else
                {
                    if (GetAliveCount(row, col) > LifeLimit)
                        Cells[row, col].MakeAlive();
                }
            }
        }
        
    }
    
    private int GetDeathCount(int row, int col)
    {
        const int cellsAroundCell = 8;
        
        return cellsAroundCell - GetAliveCount(row, col);
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
}