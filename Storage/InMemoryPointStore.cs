using SquaresApi.Models;

namespace SquaresApi.Storage;

// Simple HashSet-based store
public class InMemoryPointStore : IPointStore
{
    private readonly HashSet<Point> _points = new();

    public IEnumerable<Point> GetAll() => _points;

    public void AddMany(IEnumerable<Point> points)
    {
        foreach (var p in points)
            _points.Add(p);
    }

    public bool Add(Point point) => _points.Add(point);

    public bool Remove(Point point) => _points.Remove(point);
}
