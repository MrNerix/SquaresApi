using Xunit;
using SquaresApi.Core;
using SquaresApi.Models;

namespace SquaresApi.Tests.Core;

public class SquareFinderTests
{
    [Fact]
    public void Detects_Simple_AxisAligned_Square()
    {
        var points = new[]
        {
            new Point(-1, -1),
            new Point(1, -1),
            new Point(1, 1),
            new Point(-1, 1)
        };

        var squares = SquareFinder.FindSquares(points);

        Assert.Single(squares);
        var square = squares.First();
        Assert.Equal(4, square.Count);
    }

    [Fact]
    public void Handles_Duplicate_Points_Gracefully()
    {
        var points = new[]
        {
            new Point(-1, -1),
            new Point(1, -1),
            new Point(1, 1),
            new Point(-1, 1),
            new Point(-1, -1)
        };

        var squares = SquareFinder.FindSquares(points);

        Assert.Single(squares);
    }

    [Fact]
    public void Returns_No_Squares_When_Not_Enough_Points()
    {
        var points = new[]
        {
            new Point(0, 0),
            new Point(1, 1),
            new Point(2, 3)
        };

        var squares = SquareFinder.FindSquares(points);

        Assert.Empty(squares);
    }
    
}
