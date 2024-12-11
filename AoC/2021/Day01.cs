namespace AoC2021;

internal class Day01
{
    public static int Solve()
    {
        var lines = Input.GetLines()
            .Select(l => int.Parse(l.Trim()))
            .ToArray();


        var increasing = 0;
        for (var i = 1; i < lines.Length; i++)
        {
            if (lines[i] > lines[i - 1]) increasing++;
        }

        return increasing;
    }
}
