using Spectre.Console;
using System.Runtime.Intrinsics.X86;

namespace AoC2024;

public static class Day14
{
    static int width = 101;
    static int height = 103;

    static int halfWidthIdx;
    static int halfHeightIdx;

    [NoFancy]
    public static long Solve()
    {
        var sample = false;

        if (sample)
        {
            width = 11;
            height = 7;
        }

        halfWidthIdx = width / 2;
        halfHeightIdx = height / 2;

        var robots = Input
            .GetLines(sample: sample)
            .Select(l => Regex.Match(l, @"^p=(\d+),(\d+) v=(-?\d+),(-?\d+)$"))
            .Select(m => new Robot(new Point(m.Groups[1].Value.AsInt(), m.Groups[2].Value.AsInt()), new Point(m.Groups[3].Value.AsInt(), m.Groups[4].Value.AsInt())))
            .ToList();

        var count = robots.Count;
        var maxInCount = 0;

        for (int i = 0; i < int.MaxValue; i++)
        {
            foreach (var robot in robots)
            {
                robot.Move();
            }

            //WriteGrid(robots);
            //Console.ReadKey();

            var countIn = robots.Count(r => r.IsInChristmasTree());

            if (countIn > maxInCount)
            {
                WriteGrid(robots);
                WriteLine(countIn);
                maxInCount = countIn;
                if (countIn == 385)
                {
                    Console.ReadKey();
                }

            }

            //if (robots.All(r => r.IsInChristmasTree()))
            //    return i;

            //if (i % 10000 == 0)
            //{
            //    AnsiConsole.WriteLine(i);
            //}
        }

        return -1;
    }

    private static void WriteGrid(List<Robot> robots)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var p = new Point(x, y);
                var isInTree = IsInChristmasTree(p);
                var color = isInTree ? ConsoleColor.Green : ConsoleColor.Red;
                var locs = robots.Where(r => r.Position == p);
                if (!locs.Any())
                {

                    Write(isInTree ? '.' : '.', color);
                }
                else
                {
                    var count = locs.Count();
                    var countChar = (char)(count > 9 ? '+' : '0' + count);
                    Write(countChar, color);
                }
            }

            WriteLine();
        }
    }

    public static long Solve_Part1()
    {
        var sample = false;

        if (sample)
        {
            width = 11;
            height = 7;
        }

        halfWidthIdx = width / 2;
        halfHeightIdx = height / 2;

        var robots = Input
            .GetLines(sample: sample)
            .Select(l => Regex.Match(l, @"^p=(\d+),(\d+) v=(-?\d+),(-?\d+)$"))
            .Select(m => new Robot(new Point(m.Groups[1].Value.AsInt(), m.Groups[2].Value.AsInt()), new Point(m.Groups[3].Value.AsInt(), m.Groups[4].Value.AsInt())))
            .ToList();

        var count = robots.Count;

        for (int i = 0; i < 100; i++)
        {
            foreach (var robot in robots)
            {
                robot.Move();
            }
        }

        var quadrants = new int[] { 0, 0, 0, 0 };

        foreach (var robot in robots)
        {
            var x = robot.Position.X;
            var y = robot.Position.Y;
            if (x < halfWidthIdx)
            {
                if (y < halfHeightIdx)
                {
                    quadrants[0]++;
                }
                else if (y > halfHeightIdx)
                {
                    quadrants[1]++;
                }
            }
            else if (x > halfWidthIdx)
            {

                if (y < halfHeightIdx)
                {
                    quadrants[2]++;
                }
                else if (y > halfHeightIdx)
                {
                    quadrants[3]++;
                }
            }
        }

        return quadrants.Aggregate(1, (x, y) => x * y);
    }

    private static bool IsInChristmasTree(Point p)
    {
        long x = p.X, y = p.Y;
        if (y >= 100) return false;
        if (x == halfWidthIdx) return true;
        var minX = halfWidthIdx - (y / 2);
        var maxX = halfWidthIdx + (y / 2);
        return x >= minX && x <= maxX;
    }

    record Robot(Point Position, Point Speed)
    {
        public Point Position { get; private set; } = Position;

        public void Move()
        {
            Position = Modulo(Position.Add(Speed));
        }

        public bool IsInChristmasTree() => Day14.IsInChristmasTree(Position);

        private static Point Modulo(Point p)
        {
            return new Point((p.X + width) % width, (p.Y + height) % height);
        }
    }
}
