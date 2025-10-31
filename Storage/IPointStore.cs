using SquaresApi.Models;

namespace SquaresApi.Storage;

// in-memory storage
public interface IPointStore
{
    IEnumerable<Point> GetAll();
    void AddMany(IEnumerable<Point> points);
    bool Add(Point point); 
    bool Remove(Point point);
}
