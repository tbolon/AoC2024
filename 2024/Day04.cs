namespace AoC2024;

static partial class Day04
{
    public static int Solve()
    {
        var grid = Input.GetLines(sample: false).AsGridOfChars(outOfBoundsValue: '\0');

        return grid.CountMatch(Match);

        bool Match(char c, Point p)
        {
            if (c != 'A')
            {
                return false;
            }

            switch (grid[p.LeftUp])
            {
                case 'M':
                    if (grid[p.RightDown] != 'S') return false;
                    break;

                case 'S':
                    if (grid[p.RightDown] != 'M') return false;
                    break;

                default:
                    return false;
            }

            switch (grid[p.RightUp])
            {
                case 'M':
                    if (grid[p.LeftDown] != 'S') return false;
                    break;

                case 'S':
                    if (grid[p.LeftDown] != 'M') return false;
                    break;

                default:
                    return false;
            }

            return true;
        }
    }

    public static int Solve_01()
    {
        var grid = Input.GetLines(sample: false).AsGridOfChars(outOfBoundsValue: '\0');

        return grid.Aggregate(Visit, 0);

        int Visit(int count, char c, Point p)
        {
            if (c != 'X')
            {
                return count;
            }

            count += grid.Scan(p, Grid8Direction.Up, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.LeftUp, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.Left, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.LeftDown, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.Down, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.RightDown, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.Right, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.RightUp, ScanXmas) ? 1 : 0;

            return count;
        }

        bool? ScanXmas(Point p, char c, int index)
        {
            return index switch
            {
                0 => c == 'X' ? null : false,
                1 => c == 'M' ? null : false,
                2 => c == 'A' ? null : false,
                3 => c == 'S',
                _ => false
            };
        }
    }

    private static TResult Scan<T, TResult>(this Grid<T> @this, Point location, Grid8Direction dir, Func<Point, T, int, TResult?> action, TResult falseValue = default)
        where T : struct
        where TResult : struct
    {
        var i = 0;
        var state = action(location, @this[location], i++);
        if (state != null)
        {
            return state.Value;
        }

        do
        {
            location = location.Move(dir, 1);
            if (!@this.Contains(location))
            {
                return falseValue;
            }

            state = action(location, @this[location], i++);
        } while (state == null);

        return state ?? falseValue;
    }
}
