namespace AoC2024;

public static class Day12
{
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

    class Garden
    {
        private readonly HashSet<Point> points = new();

        private readonly Point origin;

        public Garden(char letter, Point origin)
        {
            Letter = letter;
            this.origin = origin;
        }

        public char Letter { get; }

        public int Area => points.Count;

        public int Perimeter { get; private set; }

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