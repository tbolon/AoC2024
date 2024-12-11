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

    public Point Left => new(X - 1, Y);
    public Point Right => new(X + 1, Y);
    public Point Up => new(X, Y - 1);
    public Point Down => new(X, Y + 1);
    public Point RightDown => new(X + 1, Y + 1);
    public Point RightUp => new(X + 1, Y - 1);
    public Point LeftDown => new(X - 1, Y + 1);
    public Point LeftUp => new(X - 1, Y - 1);

    public Point Move(Grid8Direction dir, int offset = 1)
    {
        if (offset == 0) return this;
        if (offset < 0) { offset = -offset; dir = dir.Invert(); }

        return dir switch
        {
            Grid8Direction.Up => new(X, Y - offset),
            Grid8Direction.LeftUp => new(X - offset, Y - offset),
            Grid8Direction.Left => new(X - offset, Y),
            Grid8Direction.LeftDown => new(X - offset, Y + offset),
            Grid8Direction.Down => new(X, Y + offset),
            Grid8Direction.RightDown => new(X + offset, Y + offset),
            Grid8Direction.Right => new(X + offset, Y),
            Grid8Direction.RightUp => new(X + offset, Y - offset),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, "Not supported")
        };
    }

    public static Point operator +(in Point p1, in Point p2) => p1.Add(p2);
    public static Point operator +(in Point p1, in (long x, long y) p2) => p1.Add(p2.x, p2.y);
    public static Point operator -(in Point p1, in Point p2) => p1.Subtract(p2);
    public static bool operator ==(in Point p1, in Point p2) => p1.X == p2.X && p1.Y == p2.Y;
    public static bool operator !=(in Point p1, in Point p2) => p1.X != p2.X || p1.Y != p2.Y;

    public Point Add(in Point other) => new(X + other.X, Y + other.Y);

    public Point Add(long x, long y) => new(X + x, Y + y);

    public Point Subtract(in Point other) => new(X - other.X, Y - other.Y);

    public void Deconstruct(out long x, out long y)
    {
        x = X;
        y = Y;
    }

    public IEnumerable<Point> Neighbors()
    {
        yield return Up;
        yield return Left;
        yield return Down;
        yield return Right;
    }

    public IEnumerable<Point> Neighbors(IGrid grid)
    {
        if (grid.Contains(Up))
            yield return Up;
        if (grid.Contains(Left))
            yield return Left;
        if (grid.Contains(Down))
            yield return Down;
        if (grid.Contains(Right))
            yield return Right;
    }


    public override string ToString() => $"({X},{Y})";

    public override bool Equals(object? obj)
    {
        if (obj is Point p2) return this == p2;
        return base.Equals(obj);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);
}


[Flags]
public enum Grid8Direction : byte
{
    Undefined = 0,
    Up = 1,
    Left = 2,
    Down = 4,
    Right = 8,
    LeftUp = Left | Up,
    LeftDown = Left | Down,
    RightDown = Right | Down,
    RightUp = Right | Up
}