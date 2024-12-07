using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json.Linq;

namespace AoC2024;

static class Day07
{
    [NoFancy]
    public static ulong Solve()
    {
        var sample = false;
        var formulas = Input.GetLines(sample: sample).Select(x => x.Split(':').Transform<string, (long, int[])>(x => (long.Parse(x[0]), [.. x[1].Trim().Split(' ').Select(r => int.Parse(r))])));

        foreach ((long result, int[] values) in formulas)
        {

        }
    }

    [NoFancy]
    public static ulong Solve_Part1()
    {
        var sample = false;
        var output = false;
        var formulas = Input.GetLines(sample: sample).Select(x => x.Split(':').Transform<string, (long, int[])>(x => (long.Parse(x[0]), [.. x[1].Trim().Split(' ').Select(r => int.Parse(r))])));

        ulong score = 0;

        foreach ((long result, int[] values) in formulas)
        {
            Markup($"[cyan]{result}[/]: [cyan]{string.Join("[/] [cyan]", values)}[/]");
            if (output) WriteLine();

            var opl = values.Length - 1;
            var mask = (int)Math.Pow(2, opl);

            var previous = score;
            long computed = 0;
            int i;

            // try all combinations
            for (i = 0; i < mask; i++)
            {
                if (output)
                {
                    Markup($"{i,4} {Convert.ToString(i, 2).PadLeft(opl, '0')} | ");
                    Markup($"{values[0]}");
                }

                computed = values[0];

                for (long j = 0; j <= opl - 1; j++)
                {
                    var value = (long)values[j + 1];
                    var opMask = (long)Math.Pow(2, j);

                    if ((i & opMask) == 0)
                    {
                        computed *= value;
                        if (output)
                        {
                            Markup($" * {value}");
                        }
                    }
                    else
                    {
                        computed += value;
                        if (output)
                        {
                            Markup($" + {value}");
                        }
                    }

                    if (computed > result)
                    {
                        break;
                    }
                }

                if (computed == result)
                {
                    score += (ulong)result;
                    break;
                }

                if (computed > result)
                {
                    if (output)
                    {
                        MarkupLine($" = [red]{computed}[/] > {result}");
                    }
                }
            }

            WriteFormula(result, values, i, opl, computed);

        }

        return score;
    }

    private static void WriteFormula(long result, int[] values, int i, int opl, long computed)
    {
        if (computed != result)
        {
            MarkupLine($" => [red]FAIL[/]");
            return;
        }

        Markup($" => {values[0]}");

        for (long j = 0; j <= opl - 1; j++)
        {
            var value = (long)values[j + 1];
            var opMask = (long)Math.Pow(2, j);

            if ((i & opMask) == 0)
            {
                Markup($" * {value}");
            }
            else
            {
                Markup($" + {value}");
            }
        }

        if (computed == result)
        {
            MarkupLine($" = [lime]{computed}[/]");
        }
        else
        {
            MarkupLine($" = [red]{computed}[/]");
        }
    }

    private record Formula(ulong Result, int[] Values);
}
