namespace AoC2021;

internal static class Day17
{
    public static void Part1()
    {
        var match = Regex.Match(Input.GetLines().First(), @"target area: x=([-\d]+)\.\.([-\d]+), y=([-\d]+)\.\.([-\d]+)", RegexOptions.CultureInvariant);

        var xmin = int.Parse(match.Groups[1].Value);
        var xmax = int.Parse(match.Groups[2].Value);
        Assert(xmin < xmax);
        var ymin = int.Parse(match.Groups[3].Value);
        var ymax = int.Parse(match.Groups[4].Value);
        Assert(ymin < ymax);

        var p = Point.Empty;

    }
}
