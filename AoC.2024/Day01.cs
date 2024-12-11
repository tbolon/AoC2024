namespace AoC2024;

public static class Day01
{
    public static int Solve_Part1()
    {
        var lines = Input.GetLines()
            .Select(l => l.SplitSpace())
            .Select(l => new { Left = l[0].AsInt(), Right = l[1].AsInt() })
            .ToArray();

        var left = lines.Select(l => l.Left).Order().ToArray();
        var right = lines.Select(l => l.Right).Order().ToArray();

        return left.Select((l, i) => Math.Abs(right.ElementAt(i) - l)).Sum();
    }

    public static int Solve()
    {
        var lines = Input.GetLines()
            .Select(l => l.SplitSpace())
            .Select(l => new { Left = l[0].AsInt(), Right = l[1].AsInt() })
            .ToArray();

        var left = lines.Select(l => l.Left).Order().ToArray();
        var right = lines.Select(l => l.Right).Order().ToArray();

        return left.Select((l) => l * right.Where(r => r == l).Count()).Sum();
    }
}
