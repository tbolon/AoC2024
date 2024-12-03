static class Day01
{
    public static void Solve_Part1()
    {
        var lines = Input.GetLines(1)
            .Select(l => l.SplitSpace())
            .Select(l => new { Left = l[0].AsInt(), Right = l[1].AsInt() })
            .ToArray();

        var left = lines.Select(l => l.Left).Order().ToArray();
        var right = lines.Select(l => l.Right).Order().ToArray();

        var dist = left.Select((l, i) => Math.Abs(right.ElementAt(i) - l)).Sum();

        WriteLine(dist);
    }

    public static void Solve()
    {
        var lines = Input.GetLines(1)
            .Select(l => l.SplitSpace())
            .Select(l => new { Left = l[0].AsInt(), Right = l[1].AsInt() })
            .ToArray();

        var left = lines.Select(l => l.Left).Order().ToArray();
        var right = lines.Select(l => l.Right).Order().ToArray();

        var sim = left.Select((l) => l * right.Where(r => r == l).Count()).Sum();

        WriteLine(sim);
    }
}
