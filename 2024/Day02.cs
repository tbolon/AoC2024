namespace AoC2024;

static class Day02
{
    public static int Solve()
    {
        var safeCount = Input.GetLines()
            .Select(l => l.Split(' ').Select(x => x.AsInt()).ToArray())
            .Count(r => IsSafe(r));

        return safeCount;
    }

    static bool IsSafe(int[] rapport)
    {
        if (ComputeIsSafeDiff(rapport))
            return true;

        // on brute force les combinaisons possibles
        for (int i = 0; i < rapport.Length; i++)
        {
            var rapport2 = rapport.Where((r, x) => x != i).ToArray();
            if (ComputeIsSafeDiff(rapport2))
                return true;
        }

        return false;
    }

    static bool ComputeIsSafeDiff(IEnumerable<int> rapport)
    {
        var diffs = rapport.SkipLast(1).Select((x, i) => rapport.ElementAt(i + 1) - x).ToArray();
        return (diffs.All(i => i > 0) || diffs.All(i => i < 0)) && diffs.All(i => Math.Abs(i) >= 1 && Math.Abs(i) <= 3);
    }
}
