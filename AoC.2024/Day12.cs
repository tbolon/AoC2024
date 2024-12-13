using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AoC2024;

public static class Day12
{
    public static long Solve()
    {
        var grid = Input.GetLines(sample: false).AsGridOfChars();

        var gardens = new List<Garden>();

        // remplissage jardins
        foreach (var cell in grid)
        {
            if (!gardens.Any(g => g.Contains(cell.Point)))
            {
                var garden = new Garden(cell.Value, cell.Point);
                gardens.Add(garden);
                garden.Fill(grid);
                //WriteLine($"{garden.Letter} Area = {garden.Area} ; Perimeter = {garden.Perimeter} ; Price = {garden.Price}");
            }
        }

        foreach (var garden in gardens)
        {
            HashSet<Point> horizSegments = [];
            HashSet<Point> vertSegments = [];

            foreach (var point in garden.Points)
            {
                if (!garden.Contains(point.Up))
                    horizSegments.Add(point);
                if (!garden.Contains(point.Left))
                    vertSegments.Add(point);
                if (!garden.Contains(point.Down))
                    horizSegments.Add(point.Down);
                if (!garden.Contains(point.Right))
                    vertSegments.Add(point.Right);
            }

            var segCount = 0;
            Point previous = Point.Empty;
            bool gardenIsDown = false;
            foreach (var point in horizSegments.OrderBy(s => s.Y).ThenBy(s => s.X))
            {
                if (previous.Right != point)
                {
                    // different fence
                    segCount++;
                    gardenIsDown = garden.Contains(point);
                    Assert(gardenIsDown || garden.Contains(point.Up));
                }
                else
                {
                    // same fence, check if the garden is on the same side
                    var thisGardenIsDown = garden.Contains(point);
                    if (gardenIsDown != thisGardenIsDown)
                    {
                        segCount++;
                        gardenIsDown = thisGardenIsDown;
                    }
                }

                previous = point;
            }

            previous = Point.Empty;
            bool gardenIsRight = false;
            foreach (var point in vertSegments.OrderBy(s => s.X).ThenBy(s => s.Y))
            {
                if (previous.Down != point)
                {
                    // different fence
                    segCount++;
                    gardenIsRight = garden.Contains(point);
                    Assert(gardenIsRight || garden.Contains(point.Left));
                }
                else
                {
                    // same fence, check if the garden is on the same side
                    var thisGardenIsRight = garden.Contains(point);
                    if (gardenIsRight != thisGardenIsRight)
                    {
                        segCount++;
                        gardenIsRight = thisGardenIsRight;
                    }
                }

                previous = point;
            }


            garden.FenceCount = segCount;

            WriteLine($"{garden.Letter} Area = {garden.Area} ; Fance Count = {garden.FenceCount} ; Price = {garden.FenceCount * garden.Area}");
        }

        return gardens.Sum(g => g.FenceCount * g.Area);
    }

    public static long Solve_Part1()
    {
        var grid = Input.GetLines(sample: false).AsGridOfChars();

        var gardens = new List<Garden>();

        foreach (var cell in grid)
        {
            if (!gardens.Any(g => g.Contains(cell.Point)))
            {
                var garden = new Garden(cell.Value, cell.Point);
                gardens.Add(garden);
                garden.Fill(grid);
                WriteLine($"{garden.Letter} Area = {garden.Area} ; Perimeter = {garden.Perimeter} ; Price = {garden.Price}");
            }
        }

        Assert(gardens.Sum(g => g.Area) == grid.Count);

        return gardens.Sum(g => g.Price);
    }

    [DebuggerDisplay("[{P1.X},{P1.Y}];[{P2.X},{P2.Y}]")]
    public readonly struct Line(Point p1, Point p2)
    {
        public Point P1 { get; } = p1;

        public Point P2 { get; } = p2;

        public bool IsVertical => P1.X == P2.X;

        public bool IsHorizontal => P1.Y == P2.Y;

        public double Length => IsVertical
            ? Math.Abs(P2.Y - P1.Y)
            : IsHorizontal
            ? Math.Abs(P2.X - P1.X)
            : Math.Sqrt(Math.Pow(P2.Y - P1.Y, 2) + Math.Pow(P2.X - P1.X, 2));


        public override int GetHashCode() => P1.GetHashCode() ^ P2.GetHashCode();

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (!(obj is Line l2)) return false;

            return (P1 == l2.P1 && P2 == l2.P2) || (P1 == l2.P2 && P2 == l2.P1);
        }
    }

    class Garden
    {
        private readonly HashSet<Point> points = new();

        private readonly Point origin;

        public Garden(char letter, Point origin)
        {
            Letter = letter;
            this.origin = origin;
        }

        public IEnumerable<Point> Points => points;

        public char Letter { get; }

        public int Area => points.Count;

        public int Perimeter { get; private set; }

        public int FenceCount { get; set; }

        public int Price => Area * Perimeter;

        public bool Contains(Point point) => points.Contains(point);

        public void Fill(Grid<char> grid)
        {
            Perimeter = Visit(grid, origin);
        }

        private int Visit(Grid<char> grid, Point p)
        {
            if (!grid.Contains(p)) return 1;
            if (grid[p] != Letter) return 1;
            if (!points.Add(p)) return 0;
            return Visit(grid, p.Left) + Visit(grid, p.Up) + Visit(grid, p.Right) + Visit(grid, p.Down);
        }
    }
}