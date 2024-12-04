namespace AoC2024;

static partial class Day04
{
    public static int Solve()
    {
        var grid = Input.GetLines(sample: false).AsGridOfChars(outOfBoundsValue: '\0');

        return grid.CountMatch(Match);

        bool Match(Point p, char c)
        {
            if (c != 'A')
            {
                return false;
            }

            switch (grid[p.NorthWest])
            {
                case 'M':
                    if (grid[p.SouthEast] != 'S') return false;
                    break;

                case 'S':
                    if (grid[p.SouthEast] != 'M') return false;
                    break;

                default:
                    return false;
            }

            switch (grid[p.NorthEast])
            {
                case 'M':
                    if (grid[p.SouthWest] != 'S') return false;
                    break;

                case 'S':
                    if (grid[p.SouthWest] != 'M') return false;
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

        return grid.Visit(Visit, 0);

        int Visit(Point p, char c, int count)
        {
            if (c != 'X')
            {
                return count;
            }

            count += grid.Scan(p, Grid8Direction.N, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.NW, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.W, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.SW, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.S, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.SE, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.E, ScanXmas) ? 1 : 0;
            count += grid.Scan(p, Grid8Direction.NE, ScanXmas) ? 1 : 0;

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

    enum ScanState : byte { Continue, Success, Fail };
}
