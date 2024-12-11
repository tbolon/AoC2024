namespace AoC2024;

public static class Day07
{
    [NoFancy]
    public static long Solve() => Solve_Iteratif(LoadFormulas());

    public static List<(long result, int[] values)> LoadFormulas() => [.. Input
            .GetLines(sample: false)
            .Select(x => x.Split(':')
            .Transform<string, (long, int[])>(x => (long.Parse(x[0]), [.. x[1].Trim().Split(' ').Select(r => int.Parse(r))])))];


    public static long Solve_Iteratif(List<(long result, int[] values)> formulas)
    {
        long score = 0;

        //Progress().Start(ctx =>
        {
            //var task = ctx.AddTask("🤖 Solving", maxValue: formulas.Count);
            foreach ((long result, int[] values) in formulas)
            {
                //task.Increment(1);

                // nombre d'opérations
                var opl = values.Length - 1;

                // génère une table combinatoire avec toutes les valeurs possibles sur une base 3
                var combTable = GetCombTable(opl, 3);

                long computed;

                // parcours de la table des combinaisons
                for (var i = 0; i < combTable.Length; i++)
                {
                    // opérations à exécuter pour cette combinaison
                    var ops = combTable[i];

                    // calcul
                    computed = values[0];
                    for (int j = 0; j < opl; j++)
                    {
                        var value = (long)values[j + 1];
                        computed = ops[j] switch
                        {
                            0 => computed * value,
                            1 => computed + value,
                            2 => long.Parse(computed.ToString() + value.ToString(), System.Globalization.NumberStyles.None),
                            _ => throw new NotSupportedException()
                        };

                        // short-circuit si le nombre dépasse la valeur recherchée en cours de route
                        if (computed > result)
                        {
                            break;
                        }
                    }

                    if (computed == result)
                    {
                        score += result;
                        break;
                    }
                }
            }
        }
        //);

        return score;
    }

    static readonly Dictionary<byte, Dictionary<int, byte[][]>> _cache = [];

    static byte[][] GetCombTable(int length, byte radix)
    {
        if (!_cache.TryGetValue(radix, out var table))
            _cache[radix] = table = [];

        if (!table.TryGetValue(length, out var combTable))
            table[length] = combTable = CreateCombTable(length, radix);

        return combTable;
    }

    /// <summary>
    /// Créé une table avec toutes les combinaisons de valeurs possibles pour une base donnée.
    /// </summary>
    static byte[][] CreateCombTable(int length, byte radix)
    {
        var max = (int)Math.Pow(radix, length);
        var result = new byte[max][];

        for (int i = 0; i < max; i++)
        {
            var val = i;
            var comb = new byte[length];
            var rank = length - 1;
            while (val > 0)
            {
                var rem = val % radix;
                comb[rank--] = (byte)rem;
                val /= radix;
            }

            result[i] = comb;
        }

        return result;
    }

    /// <summary>
    /// Méthode récursive.
    /// </summary>
    public static long Solve_Recursive(List<(long result, int[] values)> formulas)
    {
        long score = 0;

        //Progress().Start(ctx =>
        {
            //var task = ctx.AddTask("🤖 Solving", maxValue: formulas.Count);
            foreach ((long result, int[] values) in formulas)
            {
                //task.Increment(1);
                if (Calc(0, values, result, 0xff))
                    score += result;
            }
        }
        //);

        return score;
    }

    private static bool Calc(long left, IEnumerable<int> values, long result, byte op)
    {
        var right = values.First();

        var next = op switch
        {
            0 => left * right,
            1 => left + right,
            2 => long.Parse(left.ToString() + right.ToString(), System.Globalization.NumberStyles.None),
            0xff => left,
            _ => throw new NotSupportedException()
        };

        if (next > result) return false;

        if (op != 0xff) values = values.Skip(1);

        if (!values.Any()) return next == result;

        return Calc(next, values, result, 0)
            || Calc(next, values, result, 1)
            || Calc(next, values, result, 2);
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
