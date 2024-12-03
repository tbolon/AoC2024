﻿

namespace AoC2021;

internal static class Day09
{
    public static void Part2()
    {
        var grid = Input.GetLines().AsGridOfBytes(byte.MaxValue);

        // détection bassins
        List<Point> points = new();
        foreach (var (point, value) in grid)
        {
            if (grid[point.Right] > value && grid[point.Left] > value && grid[point.Up] > value && grid[point.Down] > value)
            {
                points.Add(point);
            }
        }

        // grille avec un 0 pour les zones vides, 1 pour les zones déjà découvertes d'un bassin, 0xff pour les bords (hauteur 9)
        var fill = grid.Copy((point, value) => value == 9 ? byte.MaxValue : 0);

        // examen du bassin
        int[] sizes = new int[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            sizes[i] = Visit(points[i], fill);
        }

        // calcul du produit des 3 plus grandes
        WriteLine(sizes.OrderByDescending(s => s).Take(3).Aggregate(1, (agg, current) => agg * current));
    }

    public static void Part1()
    {
        var grid = Input.GetLines().AsGridOfBytes(byte.MaxValue);

        var score = grid.Aggregate<int>((point, cumul, value) =>
        {
            if (grid[point.Right] > value
                && grid[point.Left] > value
                && grid[point.Up] > value
                && grid[point.Down] > value)
            {
                return cumul + value + 1;
            }

            return cumul;
        });

        WriteLine(score);
    }

    private static int Visit(Point point, Grid<int> fill)
    {
        if (!fill.Contains(point) || fill[point] != 0)
            return 0; // out of bounds or already visited

        fill[point] = 1; // marquée comme visitée

        return 1 + Visit(point.Left, fill) + Visit(point.Right, fill) + Visit(point.Up, fill) + Visit(point.Down, fill);
    }
}
