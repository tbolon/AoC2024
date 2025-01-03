namespace AoC2024;

public static class Day11
{
    public static long Solve()
    {
        // key: valeur sur la pierre
        // value: nb de pierres avec cette valeur
        var stones = new Dictionary<long, long>();
        var nextStones = new Dictionary<long, long>();

        stones.AddRange(Input.GetInput(sample: false).Split(' ').Select(x => long.Parse(x)).GroupBy(x => x).Select(x => (x.Key, (long)x.Count())));
        MarkupLine($"Generation [purple]{0}[/] : [lime]{stones.Values.Sum()}[/] ([cyan]{stones.Count}[/] slots)");

        for (var i = 0; i < 75; i++)
        {
            if (i < 6) MarkupLine(string.Join(' ', stones.Select(p => $"[[[purple]{p.Key}[/]x[lime]{p.Value}[/]]]")));
            nextStones.Clear();
            foreach ((var number, var count) in stones)
            {
                (var left, var right) = Blink(number);
                nextStones.AddOrIncrement(left, count);
                if (right != null)
                    nextStones.AddOrIncrement(right.Value, count);
            }

            (nextStones, stones) = (stones, nextStones);
            MarkupLine($"Generation [purple]{i + 1}[/] : [lime]{stones.Values.Sum()}[/] ([cyan]{stones.Count}[/] slots)");
        }

        return stones.Values.Sum();
    }

    private static (long left, long? right) Blink(long stone)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(stone);

        // case 1 : si 0 alors 1
        if (stone == 0) return (1, null);

        // cas 3 : si nb de chiffres impair, on multiple par 2024
        var digits = DigitCount(stone);
        if (digits % 2 != 0) return (stone * 2024, null);

        // cas 2 : si nb pair, on renvoie 2 nombres avec la moitié gauche et droite
        var divider = (int)Math.Pow(10, digits / 2);
        var left = stone / divider;
        var right = stone % divider;
        Assert(left * divider + right == stone, "Bad split");
        return (left, right);
    }

    public static long Solve_Part1()
    {
        var stones = Input.GetInput(sample: false).Split(' ').Select(x => long.Parse(x)).ToList();

        WriteLine(string.Join(' ', stones));

        for (int round = 0; round < 25; round++)
        {
            for (int x = 0; x < stones.Count; x++)
            {
                var stone = stones[x];
                if (stone == 0)
                {
                    stones[x] = 1;
                }
                else
                {
                    var digits = DigitCount(stone);
                    if (digits % 2 == 0)
                    {
                        var divider = (int)Math.Pow(10, digits / 2);
                        var left = stone / divider;
                        var right = stone % divider;
                        Assert(left * divider + right == stone, "Bad split");
                        stones[x++] = left;
                        stones.Insert(x, right);
                    }
                    else
                    {
                        stones[x] *= 2024;
                    }
                }
            }

            if (round < 6)
            {
                WriteLine(string.Join(' ', stones));
            }
            else
            {
                WriteLine($"{round} : {stones.Count}");
            }
        }

        return stones.Count;
    }

    private static int DigitCount(long value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        if (value < 10) return 1; // shortcut
        var digits = 1;
        while (value / 10 != 0)
        {
            digits++;
            value /= 10;
        }

        return digits;
    }
}