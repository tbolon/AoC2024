namespace AoC2024;

static class Day06
{
    public static int Solve()
    {
        var grid = Input.GetLines().AsGridOfChars('\0');

        (Point guard, char symbol) = grid.FirstOrDefault(((Point Point, char Symbol) p) => p.Symbol == '^');
        var dir = Grid8Direction.Up;

        var count = 0;

        while (grid.Contains(guard))
        {
            var next = guard.Move(dir);
            if (grid[next] == '#')
            {
                dir = dir.Rotate90(1);
            }
            else if (grid[next] == '.')
            {
                grid[next] = 'X';
                count++;
                guard = next;
            }
            else if (grid[next] == 'X')
            {
                guard = next;
            }
        }

        return count;
    }
}
