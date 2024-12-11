namespace AoC2024;

static class Day05
{
    public static int Solve()
    {
        var lines = Input.GetLinesArray(sample: false);
        var rules = lines.TakeWhile(x => x.Contains('|')).Select(l => l.Split('|').Transform(x => new { First = x[0].AsInt(), Second = x[1].AsInt() })).ToArray();
        var updates = lines.Skip(rules.Length).Select(r => r.Split(',').Select(r => r.AsInt()).ToArray()).ToArray();

        var ascRules = rules.GroupBy(x => x.First, x => x.Second).ToDictionary(x => x.Key, y => y.ToHashSet());
        var descRules = rules.GroupBy(x => x.Second, x => x.First).ToDictionary(x => x.Key, y => y.ToHashSet());

        var score = 0;

        foreach (var update in updates.Where(u => !IsValid(u, ascRules, descRules)))
        {
            MarkupLine($"[red]{string.Join(",", update)}[/]");

            List<int> @fixed = new(update.Length);

            for (int i = 0; i < update.Length; i++)
            {
                var page = update[i];

                if (i == 0)
                {
                    @fixed.Add(page);
                    MarkupLine($"{@fixed.StringJoin()}");
                    continue;
                }

                var fixedCount = @fixed.Count;

                for (int j = 0; j < fixedCount; j++)
                {
                    var fixedPage = @fixed[j];

                    if (ascRules[page].Contains(fixedPage))
                    {
                        @fixed.Insert(j, page);
                        MarkupLine($"{@fixed.StringJoin()} : [lime]{page} < {fixedPage}[/]");
                        break;
                    }
                    else if (ascRules[fixedPage].Contains(page))
                    {
                        MarkupLine($"{@fixed.StringJoin()} : [purple]{fixedPage} < {page}[/]");
                    }
                    else
                    {
                        MarkupLine($"{@fixed.StringJoin()}");
                    }
                }

                if (fixedCount == @fixed.Count)
                {
                    @fixed.Add(page);
                }

                Assert(fixedCount != @fixed.Count);

            }

            if (!IsValid([.. @fixed], ascRules, descRules, out var broken))
            {
                MarkupLine($"[red]{@fixed.StringJoin()}[/] : [red]{broken.first}[/] < [red]{broken.second}[/]");
            }
            else
            {
                MarkupLine($"[lime]{@fixed.StringJoin()}[/]");
                score += @fixed.Middle();
            }
            continue;
        }

        return score;
    }

    public static int Solve_01()
    {
        var lines = Input.GetLinesArray(sample: false);
        var rules = lines.TakeWhile(x => x.Contains('|')).Select(l => l.Split('|').Transform(x => new { First = x[0].AsInt(), Second = x[1].AsInt() })).ToArray();
        var updates = lines.Skip(rules.Length).Select(r => r.Split(',').Select(r => r.AsInt()).ToArray()).ToArray();

        var ascRules = rules.GroupBy(x => x.First, x => x.Second).ToDictionary(x => x.Key, y => y.ToHashSet());
        var descRules = rules.GroupBy(x => x.Second, x => x.First).ToDictionary(x => x.Key, y => y.ToHashSet());

        var score = 0;

        foreach (var update in updates)
        {
            if (IsValid(update, ascRules, descRules))
            {
                score += update.Middle();
                MarkupLine($"[lime]{update.StringJoin()}[/] = {update.Middle()}");
            }
            else
            {
                MarkupLine($"[red]{update.StringJoin()}[/]");
            }
        }

        return score;
    }

    private static bool IsValid(int[] update, Dictionary<int, HashSet<int>> ascRules, Dictionary<int, HashSet<int>> descRules)
        => IsValid(update, ascRules, descRules, out _);

    private static bool IsValid(int[] update, Dictionary<int, HashSet<int>> ascRules, Dictionary<int, HashSet<int>> descRules, out (int first, int second) broken)
    {
        for (int i = 0; i < update.Length; i++)
        {
            var page1 = update[i];

            for (int j = i + 1; j < update.Length; j++)
            {
                var page2 = update[j];

                if (descRules.TryGetValue(page1, out var pageRules) && pageRules.Contains(page2))
                {
                    broken = (page2, page1);
                    return false;
                }
            }
        }

        broken = default;
        return true;
    }
}
