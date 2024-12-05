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

        foreach (var update in updates)
        {
            for (int i = 0; i < update.Length; i++)
            {
                var page1 = update[i];

                for (int j = i + 1; j < update.Length; j++)
                {
                    var page2 = update[j];

                    if (descRules.TryGetValue(page1, out var pageRules) && pageRules.Contains(page2))
                        goto fix;

                    if (ascRules.TryGetValue(page2, out pageRules) && pageRules.Contains(page1))
                        goto fix;
                }
            }

            // OK: ignored
            continue;

        fix:
            MarkupLine($"[red]{string.Join(",", update)}[/]");
            List<int> @fixed = new(update.Length);

            for (int i = 0; i < update.Length; i++)
            {
                var value = update[i];

                if (i == 0)
                {
                    @fixed.Add(value);
                    MarkupLine($"{string.Join(",", @fixed)}");
                    continue;
                }

                var fixedCount = @fixed.Count;

                for (int j = 0; j < fixedCount; j++)
                {
                    var @fixedValue = @fixed[j];

                    if (descRules[fixedValue].Contains(value))
                    {
                        @fixed.Insert(j, value);
                        MarkupLine($"{string.Join(",", @fixed)} : [purple]{value}[/] < [purple]{fixedValue}[/]");
                        break;
                    }
                    //else if (ascRules[fixedValue].Contains(value))
                    //{
                    //    MarkupLine($"[purple]{fixedValue}[/] < [purple]{value}[/]");
                    //    @fixed.Insert(j + 1, value);
                    //    break;
                    //}
                    else if (ascRules[value].Contains(fixedValue))
                    {
                        @fixed.Insert(j, value);
                        MarkupLine($"{string.Join(",", @fixed)} : [purple]{value}[/] < [purple]{fixedValue}[/]");
                        break;
                    }
                    //else if (descRules[value].Contains(fixedValue))
                    //{
                    //    @fixed.Insert(j, value);
                    //    MarkupLine($"{string.Join(",", @fixed)} : [purple]{fixedValue}[/] < [purple]{value}[/]");
                    //    break;
                    //}
                }

                Assert(fixedCount != @fixed.Count);

            }

            if (!IsValid([.. @fixed], ascRules, descRules, out var broken))
            {
                MarkupLine($"[red]{string.Join(",", @fixed)}[/] : [red]{broken.first}[/] < [red]{broken.second}[/]");
            }
            else
            {
                MarkupLine($"[lime]{string.Join(",", @fixed)}[/]");
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
                MarkupLine($"[lime]{string.Join(",", update)}[/] = {update.Middle()}");
            }
            else
            {
                MarkupLine($"[red]{string.Join(",", update)}[/]");
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

                if (ascRules.TryGetValue(page2, out pageRules) && pageRules.Contains(page1))
                {
                    broken = (page1, page2);
                    return false;
                }
            }
        }

        broken = default;
        return true;
    }
}
