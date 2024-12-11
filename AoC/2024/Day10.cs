namespace AoC2024;

static class Day10
{
    public static int Solve()
    {
        var grid = Input.GetLines(sample: false).AsGridOfChars('\0');

        var apikes = new HashSet<Point>();

        var base0 = grid.Where(x => x.Value == '0');

        return base0.Sum(c => Hike(c.Point, c.Value, new List<Point>()));

        int Hike(Point p, char expected, List<Point> pikes)
        {
            if (!grid.Contains(p))
                return 0;

            if (grid[p] != expected)
                return 0;

            if (expected == '9')
            {
                return 1;// pikes.Add(p) ? 1 : 0;
            }

            var next = (char)(expected + 1);

            var score = 0;
            score += Hike(p.Left, next, pikes);
            score += Hike(p.Right, next, pikes);
            score += Hike(p.Up, next, pikes);
            score += Hike(p.Down, next, pikes);
            return score;

        }

    }

    public static int Solve_Part1()
    {
        var grid = Input.GetLines(sample: false).AsGridOfChars('\0');

        var apikes = new HashSet<Point>();

        var base0 = grid.Where(x => x.Value == '0');

        return base0.Sum(c => Hike(c.Point, c.Value, new HashSet<Point>()));

        int Hike(Point p, char expected, HashSet<Point> pikes)
        {
            if (!grid.Contains(p))
                return 0;

            if (grid[p] != expected)
                return 0;

            if (expected == '9')
            {
                return pikes.Add(p) ? 1 : 0;
            }

            var next = (char)(expected + 1);

            var score = 0;
            score += Hike(p.Left, next, pikes);
            score += Hike(p.Right, next, pikes);
            score += Hike(p.Up, next, pikes);
            score += Hike(p.Down, next, pikes);
            return score;

        }

    }
}
