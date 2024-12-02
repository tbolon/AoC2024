static class Day01
{
    public static void Solve_Part1()
    {
        var lines = Input.GetLines(1)
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(l => new { Left = int.Parse(l[0]), Right = int.Parse(l[1]) })
            .ToArray();

        var left = lines.Select(l => l.Left).Order().ToArray();
        var right = lines.Select(l => l.Right).Order().ToArray();

        var dist = left.Select((l, i) => Math.Abs(right.ElementAt(i) - l)).Sum();

        WriteLine(dist);
    }

    public static void Solve_Part2()
    {
        var lines = Input.GetLines(1)
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(l => new { Left = int.Parse(l[0]), Right = int.Parse(l[1]) })
            .ToArray();

        var left = lines.Select(l => l.Left).Order().ToArray();
        var right = lines.Select(l => l.Right).Order().ToArray();

        var sim = left.Select((l) => l * right.Where(r => r == l).Count()).Sum();

        WriteLine(sim);
    }
}
