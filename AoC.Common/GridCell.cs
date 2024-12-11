
namespace AoC;

/// <summary>
/// Represents a grid where values are organized in multiple rows in vertical order from top to bottom, and where each row is constituted of values ordered from left to right.
/// </summary>
public record struct GridCell<T>(Point Point, T Value)
{
    public static implicit operator (Point point, T value)(GridCell<T> value)
    {
        return (value.Point, value.Value);
    }

    public static implicit operator GridCell<T>((Point point, T value) value)
    {
        return new GridCell<T>(value.point, value.value);
    }
}