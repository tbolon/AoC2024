﻿interface IGrid
{
    bool Contains(Point p);
}

/// <summary>
/// Represents a grid where values are organized in multiple rows in vertical order from top to bottom, and where each row is constituted of values ordered from left to right.
/// </summary>
class Grid<T> : IGrid, IEnumerable<GridCell<T>>
{
    private readonly T[] _data;
    private readonly T? _outOfBoundsValue;

    public Grid(long width, long height, T? initialValue = default, T? outOfBoundsValue = default)
    {
        _data = new T[width * height];
        _outOfBoundsValue = outOfBoundsValue;
        Width = width;
        Height = height;

        if (initialValue != null)
        {
            for (int i = 0; i < _data.Length; i++)
                _data[i] = initialValue;
        }
    }

    public Grid(in IEnumerable<IEnumerable<T>> source, T? outOfBoundsValue = default)
    {
        _outOfBoundsValue = outOfBoundsValue;
        var tempItems = new List<T>();
        foreach (var row in source)
        {
            foreach (var cell in row)
            {
                if (Height == 0) Width++;
                tempItems.Add(cell);
            }

            Height++;
        }

        _data = tempItems.ToArray();
    }

    /// <summary>Gets the width of the grid.</summary>
    public long Width { get; }

    /// <summary>Gets the height of the grid.</summary>
    public long Height { get; }

    /// <summary>Gets the max acceptable value for x in <see cref="this[in long, in long]"/>.</summary>
    public long XMax => Width - 1;

    /// <summary>Gets the max acceptable value for y in <see cref="this[in long, in long]"/>.</summary>
    public long YMax => Height - 1;

    /// <summary>Gets the total number of values in the grid.</summary>
    public long Count => Width * Height;

    public T this[in Point p]
    {
        get => this[p.X, p.Y];
        set => this[p.X, p.Y] = value;
    }

    public T this[in long x, in long y]
    {
        get
        {
            if (x < 0 || x > XMax) return _outOfBoundsValue ?? throw new ArgumentOutOfRangeException(nameof(x), x, $"x must be between 0 and {XMax}");
            if (y < 0 || y > YMax) return _outOfBoundsValue ?? throw new ArgumentOutOfRangeException(nameof(y), y, $"y must be between 0 and {YMax}");
            return _data[y * Width + x];
        }
        set
        {
            if (x < 0 || x > XMax) throw new ArgumentOutOfRangeException(nameof(x), x, $"x must be between 0 and {XMax}");
            if (y < 0 || y > YMax) throw new ArgumentOutOfRangeException(nameof(y), y, $"y must be between 0 and {YMax}");
            _data[y * Width + x] = value;
        }
    }

    public IEnumerator<GridCell<T>> GetEnumerator()
    {
        for (var y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                yield return (new Point(x, y), this[x, y]);
            }
        }
    }

    /// <summary>
    /// Visit each value in grid order (left to right then top to bottom) and call the <paramref name="visit"/>.
    /// Calls <see cref="SysConsole.WriteLine"/> at the end of each row.
    /// </summary>
    public void VisitConsole(Action<T> visit) => Visit(visit, WriteLine);

    /// <summary>
    /// Visit each value in grid order (left to right then top to bottom) and call the <paramref name="visit"/>.
    /// Calls <see cref="SysConsole.WriteLine"/> at the end of each row.
    /// </summary>
    public void VisitConsole(Action<Point, T> visit) => Visit(visit, WriteLine);

    public void Visit(Action<T> visit, Action? endOfRow = null) => Visit((p, value) => visit(value), endOfRow);

    public void Visit(Action<Point, T> visit, Action? endOfRow = null)
    {
        for (var y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                visit(new Point(x, y), this[x, y]);
            }

            endOfRow?.Invoke();
        }
    }

    public void ForEach(Func<GridCell<T>, T> func)
    {
        for (var y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                this[x, y] = func(new GridCell<T>(new Point(x, y), this[x, y]));
            }
        }
    }

    public void ForEach(Action<GridCell<T>> action)
    {
        for (var y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                action(new GridCell<T>(new Point(x, y), this[x, y]));
            }
        }
    }

    public int CountMatch(Func<T, Point, bool> predicate)
    {
        return Aggregate(Match, 0);

        int Match(int current, T value, Point p)
        {
            if (predicate(value, p))
                return current + 1;
            return current;
        }
    }

    public TResult Aggregate<TResult>(Func<TResult, T, Point, TResult> visit, TResult seed = default, Action? endOfRow = null) where TResult : struct
    {
        var result = seed;

        for (var y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                result = visit(result, this[x, y], new Point(x, y));
            }

            endOfRow?.Invoke();
        }

        return result;
    }

    public bool Contains(Point p) => p.X >= 0 && p.X <= XMax && p.Y >= 0 && p.Y <= YMax;

    /// <summary>
    /// Effectue une copie de cette grille, en transformant chaque élément.
    /// </summary>
    public Grid<T2> Copy<T2>(Func<Point, T, T2> transform)
    {
        var result = new Grid<T2>(Width, Height);
        foreach (var (point, value) in this)
        {
            result[point] = transform(point, value);
        }

        return result;
    }
    /// <summary>
    /// Effectue une copie de cette grille
    /// </summary>
    public Grid<T> Copy()
    {
        var result = new Grid<T>(Width, Height);
        foreach (var (point, value) in this)
        {
            result[point] = value;
        }

        return result;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

internal record struct GridCell<T>(Point Point, T Value)
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