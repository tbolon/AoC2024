static class Extensions
{
    public static string? EmptyAsNull(this string @this) => string.IsNullOrEmpty(@this) ? null : @this;

    public static int AsInt(this string @this) => int.Parse(@this, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture);

    public static int? AsIntN(this string? @this) => string.IsNullOrEmpty(@this) ? null : int.Parse(@this, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture);

    public static string[] SplitSpace(this string @this, bool removeEmptyEntries = true) => @this.Split(' ', removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

    public static int IndexOf<T>(this T[] array, T value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (Equals(array[i], value))
                return i;
        }

        return -1;
    }

    public static T Middle<T>(this T[] @this)
    {
        Assert(@this.Length % 2 == 1);
        return @this[@this.Length / 2];
    }

    public static T Middle<T>(this IEnumerable<T> @this)
    {
        Assert(@this.Count() % 2 == 1);
        return @this.ElementAt(@this.Count() / 2);
    }

    public static TResult Transform<T, TResult>(this T[] @this, Func<T[], TResult> func) => func(@this);

    /// <summary>
    /// Assumes that each line is composed of the same amount of characters and returns a grid with all lines.
    /// </summary>
    public static Grid<char> AsGridOfChars(this IEnumerable<string> lines, char? outOfBoundsValue = default) => new(lines, outOfBoundsValue);

    /// <summary>
    /// Assumes that each line is composed of characters '0' to '9', convert them to an array of bytes and returns a grid with all lines.
    /// </summary>
    public static Grid<byte> AsGridOfBytes(this IEnumerable<string> lines, byte? outOfBoundsValue = default) => AsGrid(lines, l => l.Select(c => (byte)(c - '0')), outOfBoundsValue);

    /// <summary>
    /// Converts all lines to a grid, assuming the with of the grid will be based on the number of values returned by the first line.
    /// </summary>
    /// <param name="lines">Lines to convert to grid.</param>
    /// <param name="transform">Function to use to transform each line of text into a collection of values.</param>
    /// <param name="outOfBoundsValue">
    /// A specific value to return when out of bounds coordinates are used when calling <see cref="Grid{T}.Item(long,long)"/>.
    /// Use <see langword="null" /> to raise an <see cref="ArgumentOutOfRangeException"/> when and out of bounds index is used.
    /// </param>
    public static Grid<T> AsGrid<T>(this IEnumerable<string> lines, Func<string, IEnumerable<T>> transform, T? outOfBoundsValue = default) where T : struct => new(lines.Select(l => transform(l)), outOfBoundsValue);

    public static IEnumerable<char> AsChars(this string @this) => @this;

    public static Grid8Direction Invert(this Grid8Direction @this) => @this switch
    {
        Grid8Direction.Up => Grid8Direction.Down,
        Grid8Direction.Left => Grid8Direction.Right,
        Grid8Direction.Down => Grid8Direction.Up,
        Grid8Direction.Right => Grid8Direction.Left,
        Grid8Direction.LeftUp => Grid8Direction.RightDown,
        Grid8Direction.LeftDown => Grid8Direction.RightUp,
        Grid8Direction.RightDown => Grid8Direction.LeftUp,
        Grid8Direction.RightUp => Grid8Direction.LeftDown,
        _ => throw new ArgumentOutOfRangeException(nameof(@this), @this, "Not supported")
    };
}