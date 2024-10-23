using System.Collections.Immutable;
using System.Drawing;
using Common.NumbersGenerator;
using MazeModel.Exporters;
using MazeModel.Importers;
using MazeModel.MazeGenerator;


namespace MazeModel.Maze;

public class Maze
{
    public int Rows => Points?.GetLength(0) ?? 0;
    public int Cols => Points?.GetLength(1) ?? 0;
    public event EventHandler<MazePoint[,]>? ChangeMaze;
    public event EventHandler<IList<MazePoint>> ChangePath;

    public Maze(IGenerator numberGenerator )
    {
        Generator = new MazeGenerator.MazeGenerator(numberGenerator);
    }

    private MazeGenerator.MazeGenerator Generator { get; }


    private MazePoint[,]? _mazePoints;

    public MazePoint? StartPoint { get; set; }

    public MazePoint? EndPoint { get; set; }
    
    public bool CanBuildPath => StartPoint is not null && EndPoint is not null;

    public MazePoint[,]? Points
    {
        get => _mazePoints;
        private set
        {
            _mazePoints = value;
            ChainPoints();
            if (_mazePoints != null) OnChangeMaze(_mazePoints);
        }
    }

    private List<MazePoint> _path;

    public ImmutableList<MazePoint>? Path
    {
        get { return _path?.ToImmutableList() ?? null; }
        set
        {
            if (value != null) _path = value.ToList();

            OnChangePath(_path);

        }
    }


    public void Generate(int rows, int cols)
    {
        Points = Generator.Generate(rows, cols);
    }

    private void ChainPoints()
    {
        if (Points is null) throw new ArgumentNullException(nameof(Points));

        var rows = Points.GetLength(0);
        var cols = Points.GetLength(1);

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                if (!Points[row, col].Right)
                {
                    var point = Points[row, col];
                    var neighborPoint = Points[row, col + 1];
                    point.Neighbors.Add(neighborPoint);
                    neighborPoint.Neighbors.Add(point);
                }

                if (!Points[row, col].Down)
                {
                    var point = Points[row, col];
                    var neighborPoint = Points[row + 1, col];
                    point.Neighbors.Add(neighborPoint);
                    neighborPoint.Neighbors.Add(point);
                }
            }
        }
    }

    public void BuildPath()
    {
        if (StartPoint is null) throw new ArgumentNullException(nameof(StartPoint));
        if (EndPoint is null) throw new ArgumentNullException(nameof(EndPoint));

        var openQueue = new PriorityQueue<MazePoint, MazePoint>(new MazePointComparer());

        var closedSet = new HashSet<MazePoint>();

        openQueue.Enqueue(StartPoint, StartPoint);

        while (openQueue.Count != 0)
        {
            var currentNode = openQueue.Dequeue();

            if (currentNode == EndPoint)
            {
                var path = new List<MazePoint>();
                while (currentNode is not null)
                {
                    path.Add(currentNode);
                    if (currentNode == StartPoint)
                        break;
                    currentNode = currentNode.Previous;
                }

                path.Reverse();
                Path = path.ToImmutableList();
                return;
            }

            closedSet.Add(currentNode);

            foreach (var neighbor in currentNode.Neighbors)
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                var newG = currentNode.G + 1;

                var nfo = openQueue.UnorderedItems.Select(item => item.Element)
                    .FirstOrDefault(point => point == neighbor);
                if (nfo is not null)
                {
                    if (newG >= nfo.G) continue;

                    nfo.G = newG;
                    nfo.H = Math.Abs(nfo.Row - EndPoint.Row) + Math.Abs(nfo.Col - EndPoint.Col);
                    nfo.Previous = currentNode;
                }
                else
                {
                    neighbor.G = newG;
                    neighbor.H = GetEuristic(neighbor, EndPoint);
                    neighbor.Previous = currentNode;
                    openQueue.Enqueue(neighbor, neighbor);
                }
            }
        }
    }

    private static int GetEuristic(MazePoint startPoint, MazePoint endPoint)
    {
        return Math.Abs(startPoint.Row - endPoint.Row) + Math.Abs(startPoint.Col - endPoint.Col);
    }

    private void OnChangeMaze(MazePoint[,] points)
    {
        ChangeMaze?.Invoke(this, points);
    }
    private void OnChangePath(List<MazePoint> path)
    {
        ChangePath?.Invoke(this, path);
    }

    public void ImportString(string importString)
    {
        var importer = new StringMazeImporter(importString);
        Points = importer.Import();
    }
    
    public void ImportList(List<List<MazePointWalls>> importList)
    {
        var importer = new ListMazeImporter(importList);
        Points = importer.Import();
    }

    public string ExportString()
    {
        if (Points is null) throw new ArgumentNullException(nameof(Points));
        var exporter = new MazeStringExporter();
        
        return exporter.Export(Points);
    }
    
    public List<List<MazePointWalls>> ExportList()
    {
        if (Points is null) throw new ArgumentNullException(nameof(Points));
        var exporter = new MazeListExporter();
        
        return exporter.Export(Points);
    }
}