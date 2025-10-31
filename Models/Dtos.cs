namespace SquaresApi.Models;


//Data Transfer Objects

// adding one point
public record CreatePointDto(int X, int Y);

// importing many points at once
public record ImportPointsDto(List<CreatePointDto> Points);

// the return for each found square, 4 pointss
public record SquareDto(List<CreatePointDto> Points);

// all squares
public record SquaresResponse(int Count, List<SquareDto> Squares);
