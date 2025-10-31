using SquaresApi.Data;
using SquaresApi.Models;

namespace SquaresApi.Storage;

public class SqlitePointStore : IPointStore
{
    private readonly AppDb _db;

    public SqlitePointStore(AppDb db)
    {
        _db = db;
    }

    public IEnumerable<Point> GetAll()
    {
        // get all points
        var points = new List<Point>();
        foreach (var p in _db.Points)
        {
            points.Add(new Point(p.X, p.Y));
        }
        return points;
    }

    public void AddMany(IEnumerable<Point> points)
    {
        // Loop through each point and add if not already in DB
        foreach (var p in points)
        {
            bool exists = _db.Points.Any(x => x.X == p.X && x.Y == p.Y);
            if (!exists)
            {
                _db.Points.Add(new PointEntity { X = p.X, Y = p.Y });
            }
        }
        _db.SaveChanges();
    }

    public bool Add(Point point)
    {
        // Add only if it doesn’t already exist
        bool exists = _db.Points.Any(x => x.X == point.X && x.Y == point.Y);
        if (exists)
        {
            return false;
        }

        _db.Points.Add(new PointEntity { X = point.X, Y = point.Y });
        _db.SaveChanges();
        return true;
    }

    public bool Remove(Point point)
    {
        // Try to find and remove the matching point
        var item = _db.Points.FirstOrDefault(x => x.X == point.X && x.Y == point.Y);
        if (item == null)
        {
            return false;
        }

        _db.Points.Remove(item);
        _db.SaveChanges();
        return true;
    }
}
