using System.Text.RegularExpressions;

static partial class Day03
{
    public static void Solve()
    {
        var input = Input.GetFile(3);

        List<Token> tokens =
        [
            .. MulRegex().Matches(input).Cast<Match>().Select(m => new Token(m.Index, TokenType.Mul, int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value))),
            .. DoDontRegex().Matches(input).Cast<Match>().Select(m => new Token(m.Index, m.Groups[1].Value != "" ? TokenType.Dont : TokenType.Do)),
        ];

        var score = 0;
        var doMul = true;

        foreach (var token in tokens.Order())
        {
            if (token.type == TokenType.Mul)
            {
                score += doMul ? token.score : 0;
            }
            else
            {
                doMul = token.type == TokenType.Do;
            }
        }

        WriteLine(score);
    }

    record Token(int index, TokenType type, int score = 0) : IComparable<Token>
    {
        public int CompareTo(Token? other) => index.CompareTo((other?.index ?? 0));
    }

    enum TokenType { Mul, Do, Dont, }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"do(n't)?\(\)")]
    private static partial Regex DoDontRegex();
}