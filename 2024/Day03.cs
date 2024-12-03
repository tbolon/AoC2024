namespace AoC2024;

static partial class Day03
{
    public static int Solve()
    {
        var input = Input.GetInput();

        List<Token> tokens =
        [
            .. MulRegex().Matches(input).Cast<Match>()
                .Select(m => new Token(m.Index, TokenType.Mul, m.Groups[1].Value.AsInt() * m.Groups[2].Value.AsInt())),
            .. DoDontRegex().Matches(input).Cast<Match>()
                .Select(m => new Token(m.Index, m.Groups[1].Value != "" ? TokenType.Dont : TokenType.Do)),
        ];

        return tokens
            .OrderBy(t => t.Index)
            .Aggregate(
                (Score: 0, DoMul: true),
                static (ctx, token) => (
                    ctx.Score + (ctx.DoMul ? token.Score : 0),
                    token.Type switch { TokenType.Do => true, TokenType.Dont => false, _ => ctx.DoMul }
                )
            ).Score;
    }

    record Token(int Index, TokenType Type, int Score = 0);

    enum TokenType { Mul, Do, Dont, }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"do(n't)?\(\)")]
    private static partial Regex DoDontRegex();
}