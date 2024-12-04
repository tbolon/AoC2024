/// <summary>
/// Represents a point in a space where X move from left to right and Y move from top to bottom.
/// </summary>
internal readonly struct Point
{
    public static readonly Point Empty = new(0, 0);

    public Point(in long x, in long y)
    {
        X = x;
        Y = y;
    }

    /// <summary>Horizontal coordinate from left to right.</summary>
    public long X { get; }

    /// <summary>Vertical coordinate from top to bottom.</summary>
    public long Y { get; }

    public Point West => new(X - 1, Y);
    public Point East => new(X + 1, Y);
    public Point North => new(X, Y - 1);
    public Point South => new(X, Y + 1);
    public Point SouthEast => new(X + 1, Y + 1);
    public Point NorthEast => new(X + 1, Y - 1);
    public Point SouthWest => new(X - 1, Y + 1);
    public Point NorthWest => new(X - 1, Y - 1);

    public Point Move(Grid8Direction dir, int offset = 1)
    {
        if (offset == 0) return this;
        if (offset < 0) { offset = -offset; dir = dir.Invert(); }

        return dir switch
        {
            Grid8Direction.N => new(X, Y - offset),
            Grid8Direction.NW => new(X - offset, Y - offset),
            Grid8Direction.W => new(X - offset, Y),
            Grid8Direction.SW => new(X - offset, Y + offset),
            Grid8Direction.S => new(X, Y + offset),
            Grid8Direction.SE => new(X + offset, Y + offset),
            Grid8Direction.E => new(X + offset, Y),
            Grid8Direction.NE => new(X + offset, Y - offset),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, "Not supported")
        };
    }

    public static Point operator +(in Point p1, in Point p2) => p1.Add(p2);
    public static Point operator -(in Point p1, in Point p2) => p1.Subtract(p2);
    public static bool operator ==(in Point p1, in Point p2) => p1.X == p2.X && p1.Y == p2.Y;
    public static bool operator !=(in Point p1, in Point p2) => p1.X != p2.X || p1.Y != p2.Y;

    public Point Add(in Point other) => new(X + other.X, Y + other.Y);
    public Point Subtract(in Point other) => new(X - other.X, Y - other.Y);

    public void Deconstruct(out long x, out long y)
    {
        x = X;
        y = Y;
    }

    public IEnumerable<Point> Neighbors()
    {
        yield return North;
        yield return West;
        yield return South;
        yield return East;
    }

    public IEnumerable<Point> Neighbors(IGrid grid)
    {
        if (grid.Contains(North))
            yield return North;
        if (grid.Contains(West))
            yield return West;
        if (grid.Contains(South))
            yield return South;
        if (grid.Contains(East))
            yield return East;
    }


    public override string ToString() => $"({X},{Y})";

    public override bool Equals(object? obj)
    {
        if (obj is Point p2) return this == p2;
        return base.Equals(obj);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);
}


internal enum Grid8Direction : byte { N, W, S, E, NW, SW, SE, NE }

internal enum Grid4Direction : byte { N, W, S, E }