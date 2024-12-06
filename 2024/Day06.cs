namespace AoC2024;

static class Day06
{
    public static int Solve()
    {
        var grid = Input.GetLines(false).AsGridOfChars('\0');

        (Point guard, char symbol) = grid.FirstOrDefault(((Point Point, char Symbol) p) => p.Symbol == '^');
        var dir = Grid8Direction.Up;

        var count = 0;

        while (grid.Contains(guard))
        {
            var next = guard.Move(dir);
            var nextSymbol = grid[next];
            if (nextSymbol == '#')
            {
                MarkupLine($"RotateRight");
                dir = dir.Rotate90(1);
            }
            else if (nextSymbol == '.' || nextSymbol == '^')
            {
                MarkupLine($"Move");
                grid[next] = 'X';
                count++;
                guard = next;
            }
            else if (nextSymbol == 'X')
            {
                MarkupLine($"Move X");
                guard = next;
            }
            else if(nextSymbol == '\0')
            {
                break;
            }
        }

        grid.VisitConsole();

        return count;
    }
}
