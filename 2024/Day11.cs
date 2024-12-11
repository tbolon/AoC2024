namespace AoC2024;

public static class Day11
{
    record StoneGen(long Number, int Generation);

    record StoneResult(long Left, long? Right);

    public static long Solve()
    {
        var stones = Input.GetInput(sample: false).Split(' ').Select(x => long.Parse(x)).GroupBy(x => x).ToDictionary(x => x.Key, x => (long)x.Count()).ToSortedDictionary();

        WriteLine(string.Join(' ', stones));

        for (var i = 0; i < 75; i++)
        {
            WriteLine($"Generation {i} : {stones.Values.Sum()} ({stones.Count} slots)");
            if (i < 6) WriteLine(string.Join(' ', stones));

            var nextStones = new SortedDictionary<long, long>();
            foreach ((var number, var count) in stones)
            {
                (var left, var right) = Blink(number);
                nextStones.AddOrIncrement(left, count);
                if (right != null)
                    nextStones.AddOrIncrement(right.Value, count);
            }

            stones = nextStones;
        }

        return stones.Values.Sum();
    }

    private static (long left, long? right) Blink(long stone)
    {
        // case 1 : si 0 alors 1
        if (stone == 0)
            return (1, null);

        var digits = CountDigits(stone);

        // cas 3 : si nb impair, on multiple par 2024
        if (digits % 2 != 0)
            return (stone * 2024, null);

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
                    var digits = CountDigits(stone);
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

    private static int CountDigits(long value)
    {
        var digits = 1;
        while (value / 10 > 0)
        {
            digits++;
            value /= 10;
        }

        return digits;
    }
}