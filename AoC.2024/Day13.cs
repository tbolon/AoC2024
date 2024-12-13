using System.Diagnostics;
using System.Numerics;

namespace AoC2024;

public static class Day13
{
    [NoFancy]
    public static long Solve()
    {
        long score = 0;

        var lines = Input.GetLinesArray(sample: false);
        for (int i = 0; i < lines.Length; i += 3)
        {
            var match = Regex.Match(lines[i], @"^Button ([AB]): X\+(\d+), Y\+(\d+)$", RegexOptions.CultureInvariant);
            var btnA = new Point(match.Groups[2].Value.AsInt(), match.Groups[3].Value.AsInt());
            match = Regex.Match(lines[i + 1], @"^Button ([AB]): X\+(\d+), Y\+(\d+)$", RegexOptions.CultureInvariant);
            var btnB = new Point(match.Groups[2].Value.AsInt(), match.Groups[3].Value.AsInt());
            match = Regex.Match(lines[i + 2], @"^Prize: X=(\d+), Y=(\d+)$", RegexOptions.CultureInvariant);
            var prize = new Point(match.Groups[1].Value.AsInt(), match.Groups[2].Value.AsInt());

            var arc = new Arcade(btnA, btnB, prize);
            arc.BumpPrize();
            (var a, var b) = arc.Solve();
            WriteLine($"BtnA={btnA};BtnB={btnB};Prize={arc.Prize} => a={a};b={b}");
            score += 3 * a + b;
        }

        return score;
    }

    [NoFancy]
    public static long Solve_Part1()
    {
        long score = 0;

        var lines = Input.GetLinesArray(sample: false);
        for (int i = 0; i < lines.Length; i += 3)
        {
            var match = Regex.Match(lines[i], @"^Button ([AB]): X\+(\d+), Y\+(\d+)$", RegexOptions.CultureInvariant);
            var btnA = new Point(match.Groups[2].Value.AsInt(), match.Groups[3].Value.AsInt());
            match = Regex.Match(lines[i + 1], @"^Button ([AB]): X\+(\d+), Y\+(\d+)$", RegexOptions.CultureInvariant);
            var btnB = new Point(match.Groups[2].Value.AsInt(), match.Groups[3].Value.AsInt());
            match = Regex.Match(lines[i + 2], @"^Prize: X=(\d+), Y=(\d+)$", RegexOptions.CultureInvariant);
            var prize = new Point(match.Groups[1].Value.AsInt(), match.Groups[2].Value.AsInt());

            var arc = new Arcade(btnA, btnB, prize);
            (var a, var b) = arc.Solve();
            score += 3 * a + b;
        }

        return score;
    }

    [DebuggerDisplay("BtnA:{BtnA};BtnB:{BtnB};Prize:{Prize}")]
    private class Arcade(Point btnA, Point btnB, Point prize)
    {
        public Point BtnA => btnA;

        public Point BtnB => btnB;

        public Point Prize => prize;

        public void BumpPrize()
        {
            prize = new Point(prize.X + 10000000000000, prize.Y + 10000000000000);
        }

        public (long a, long b) Solve()
        {
            // b = ((Py*Ax) - (Ay*Px)) / ((By*Ax) - (Ay*Bx))
            var t1 = (Prize.Y * BtnA.X) - (BtnA.Y * Prize.X);
            var t2 = (BtnB.Y * BtnA.X) - (BtnA.Y * BtnB.X);

            var b = Math.Round((decimal)t1 / t2, 6);

            if (b != (long)b)
                return (0, 0);


            // a = (Px/Ax) - ((Bx/Ax) * b)
            // a = pa - ba * b ; ba = Bx/Ax ; pa = Px/Ax
            var pa = (decimal)prize.X / btnA.X;
            var ba = (decimal)btnB.X / btnA.X;

            var a = Math.Round(pa - (ba * b), 6);

            if (a != (long)a)
                return (0, 0);


            return ((long)a, (long)b);

        }
    }
}
