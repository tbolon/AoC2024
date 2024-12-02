static class Day02
{
    public static void Solve()
    {
        var safeCount = Input.GetLines(2)
            .Select(l => l.Split(' ').Select(x => int.Parse(x)).ToArray())
            .Count(r => IsSafe(r));
        //.Count(r => IsSafe_Part2(r));

        WriteLine(safeCount);

        static bool IsSafe(int[] rapport)
        {
            var diff = ComputeDiff(rapport);
            if (IsSafeDiff(diff))
            {
                return true;
            }

            // on brute force les combinaisons possibles
            for (int i = 0; i < rapport.Length; i++)
            {
                var rapport2 = rapport.Where((r, x) => x != i).ToArray();
                var diff2 = ComputeDiff(rapport2);
                if (IsSafeDiff(diff2))
                    return true;
            }

            return false;
        }

        static bool IsSafeDiff(int[] diffs)
        {
            return (diffs.All(i => i > 0) || diffs.All(i => i < 0)) && diffs.All(i => Math.Abs(i) >= 1 && Math.Abs(i) <= 3);
        }

        static int[] ComputeDiff(IEnumerable<int> rapport) => rapport.SkipLast(1).Select((x, i) => rapport.ElementAt(i + 1) - x).ToArray();

    }
}
