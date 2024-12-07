namespace AoC2024;

static class Day07
{
    [NoFancy]
    public static ulong Solve()
    {
        var sample = false;
        var formulas = Input.GetLines(sample: sample).Select(x => x.Split(':').Transform<string, (long, int[])>(x => (long.Parse(x[0]), [.. x[1].Trim().Split(' ').Select(r => int.Parse(r))])));

        ulong score = 0;

        foreach ((long result, int[] values) in formulas)
        {
            MarkupLine($"[cyan]{result}[/]: [cyan]{string.Join("[/] [cyan]", values)}[/]");
            var opl = values.Length - 1;
            var mask = (int)Math.Pow(3, opl);

            for (int i = 0; i < mask; i++)
            {
                var base3 = DecimalToArbitrarySystem(i, 3).PadLeft(opl, '0');

                long computed = 0;


                computed = values[0];

                for (int j = 0; j <= opl - 1; j++)
                {
                    var value = (long)values[j + 1];
                    var op = base3[j];
                    if (op == '0')
                    {
                        computed *= value;
                    }
                    else if (op == '1')
                    {
                        computed += value;
                    }
                    else
                    {
                        computed = long.Parse(computed.ToString() + value.ToString(), System.Globalization.NumberStyles.None);
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
            }

            //SysConsole.ReadKey();
        }

        return score;
    }

    /// <summary>
    /// Converts the given decimal number to the numeral system with the
    /// specified radix (in the range [2, 36]).
    /// </summary>
    /// <param name="decimalNumber">The number to convert.</param>
    /// <param name="radix">The radix of the destination numeral system
    /// (in the range [2, 36]).</param>
    /// <returns></returns>
    public static string DecimalToArbitrarySystem(long decimalNumber, int radix)
    {
        const int BitsInLong = 64;
        const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        if (radix < 2 || radix > Digits.Length)
            throw new ArgumentException("The radix must be >= 2 and <= " +
                Digits.Length.ToString());

        if (decimalNumber == 0)
            return "0";

        int index = BitsInLong - 1;
        long currentNumber = Math.Abs(decimalNumber);
        char[] charArray = new char[BitsInLong];

        while (currentNumber != 0)
        {
            int remainder = (int)(currentNumber % radix);
            charArray[index--] = Digits[remainder];
            currentNumber = currentNumber / radix;
        }

        string result = new String(charArray, index + 1, BitsInLong - index - 1);
        if (decimalNumber < 0)
        {
            result = "-" + result;
        }

        return result;
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
