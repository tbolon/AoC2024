﻿namespace AoC2024;

public static class Day08
{
    public static int Solve()
    {
        var grid = Input.GetLines(false).AsGridOfChars();
        var antennas = grid.Where(c => c.Value != '.').GroupBy(c => c.Value, c => c.Point).Select(g => g.ToArray());
        HashSet<Point> antinotes = [];

        foreach (var freqAnts in antennas)
        {
            foreach (var (first, second) in freqAnts.CrossJoin(freqAnts).Where(x => x.First != x.Second))
            {
                var offset = second - first;
                for (var p2 = second; grid.Contains(p2); p2 = p2 += offset)
                {
                    antinotes.Add(p2);
                }
            }
        }

        return antinotes.Count;
    }

    public static int Solve_Part1()
    {
        var grid = Input.GetLines(false).AsGridOfChars();

        HashSet<Point> antinotes = [];

        var antennas = grid.Where(c => c.Value != '.').GroupBy(c => c.Value, c => c.Point).ToDictionary(g => g.Key, g => g.ToArray());

        foreach (var group in antennas)
        {
            var freq = group.Key;
            var ants = group.Value;

            for (var i = 0; i < ants.Length; i++)
            {
                var first = ants[i];
                for (int j = 0; j < ants.Length; j++)
                {
                    if (i == j)
                        continue;

                    var second = ants[j];

                    var x2 = second.X + second.X - first.X;
                    var y2 = second.Y + second.Y - first.Y;
                    var p2 = new Point(x2, y2);
                    if (grid.Contains(p2))
                        antinotes.Add(p2);
                }
            }
        }

        return antinotes.Count;
    }
}
