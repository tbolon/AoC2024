using AoC2021;

namespace AoC2023;

static class Day01
{
    static string[] names = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    static char[] digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public static int Solve()
    {
        var lines = Input.GetLines(sample: false);

        var sum = 0;

        foreach (var line in lines)
        {
            char left = '\0';
            char right = '\0';

            for (int i = 0; i < line.Length; i++)
            {
                var sub = line.Substring(i);

                var x = digits.IndexOf(sub[0]);
                if (x != -1)
                {
                    if (left == '\0') left = digits[x];
                    right = digits[x];
                }

                for (x = 0; x < names.Length; x++)
                {
                    if (sub.StartsWith(names[x]))
                    {
                        if (left == '\0') left = digits[x];
                        right = digits[x];
                        break;
                    }
                }
            }

            var value = $"{left}{right}".AsInt();

            sum += value;
        }

        return sum;
    }

    public static int Solve_01()
    {
        var lines = Input.GetLines();


        var sum = 0;

        foreach (var line in lines)
        {
            var left = line.First(c => char.IsDigit(c));
            var right = line.Last(c => char.IsDigit(c));

            sum += $"{left}{right}".AsInt();
        }

        return sum;
    }
}
