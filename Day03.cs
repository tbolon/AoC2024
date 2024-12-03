static partial class AdventOfCode2024_Day03
{
    public static void Solve()
    {
        var input = Input.GetFile(3);

        List<Token> tokens =
        [
            .. MulRegex().Matches(input).Cast<Match>()
                .Select(m => new Token(m.Index, TokenType.Mul, int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value))),
            .. DoDontRegex().Matches(input).Cast<Match>()
                .Select(m => new Token(m.Index, m.Groups[1].Value != "" ? TokenType.Dont : TokenType.Do)),
        ];

        var score = tokens
            .OrderBy(t => t.Index)
            .Aggregate(
                (Score: 0, DoMul: true),
                static (x, t) => (
                    x.Score + (x.DoMul ? t.Score : 0),
                    t.Type switch { TokenType.Do => true, TokenType.Dont => false, _ => x.DoMul }
                )
            ).Score;

        WriteLine(score);
    }

    record Token(int Index, TokenType Type, int Score = 0);

    enum TokenType { Mul, Do, Dont, }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"do(n't)?\(\)")]
    private static partial Regex DoDontRegex();
}