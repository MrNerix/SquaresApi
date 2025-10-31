using NetTopologySuite.Geometries;
using SquaresApi.Models;

namespace SquaresApi.Core
{
    public static class SquareFinder
    {
        // Small tolerance for comparing doubles (lengths)
        private const double Eps = 0.0001;

        public static List<List<Models.Point>> FindSquares(IEnumerable<Models.Point> points)
        {
            // Remove duplicates
            var unique = new Dictionary<(int X, int Y), Models.Point>();
            foreach (var p in points)
            {
                unique[(p.X, p.Y)] = p;
            }

            var pointList = unique.Values.ToList();
            var result = new List<List<Models.Point>>();

            var geometryFactory = new GeometryFactory();

            // Try every combination of 4 distinct points
            for (int a = 0; a < pointList.Count; a++)
            for (int b = a + 1; b < pointList.Count; b++)
            for (int c = b + 1; c < pointList.Count; c++)
            for (int d = c + 1; d < pointList.Count; d++)
            {
                var pa = pointList[a];
                var pb = pointList[b];
                var pc = pointList[c];
                var pd = pointList[d];

                // Build a closed ring of coordinates (required by NTS polygon)
                var coords = new[]
                {
                    new Coordinate(pa.X, pa.Y),
                    new Coordinate(pb.X, pb.Y),
                    new Coordinate(pc.X, pc.Y),
                    new Coordinate(pd.X, pd.Y),
                    new Coordinate(pa.X, pa.Y)
                };

                var polygon = geometryFactory.CreatePolygon(coords);

                // check if the 4 points form a rectangle
                if (!polygon.IsRectangle)
                    continue;

                // If it is a rectangle, check all 4 sides have (about) the same length
                var sides = GetSideLengths(coords);
                if (IsSquare(sides))
                {
                    result.Add(new List<Models.Point> { pa, pb, pc, pd });
                }
            }

            return result;
        }

        // Compute the 4 side lengths from the closed ring (coords[0..4])
        private static List<double> GetSideLengths(Coordinate[] coords)
        {
            var lengths = new List<double>(capacity: 4);
            for (int i = 0; i < 4; i++)
            {
                var dx = coords[i + 1].X - coords[i].X;
                var dy = coords[i + 1].Y - coords[i].Y;
                lengths.Add(Math.Sqrt(dx * dx + dy * dy));
            }
            return lengths;
        }

        // A square is a rectangle whose 4 sides are (approximately) equal
        private static bool IsSquare(List<double> sides)
        {
            var first = sides[0];
            for (int i = 1; i < sides.Count; i++)
            {
                if (Math.Abs(sides[i] - first) >= Eps)
                    return false;
            }
            return true;
        }
    }
}
