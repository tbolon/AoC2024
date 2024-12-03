using System.Text.RegularExpressions;

static class Day03
{
    public static void Solve()
    {
        var input = Input.GetFile(3);

        List<Token> tokens = new();

        foreach (Match match in Regex.Matches(input, @"mul\((\d+),(\d+)\)"))
        {
            var left = int.Parse(match.Groups[1].Value);
            var right = int.Parse(match.Groups[2].Value);
            tokens.Add(new Token(match.Index, TokenType.Mul, left * right));
        }

        foreach (Match match in Regex.Matches(input, @"do(n't)?\(\)"))
        {
            tokens.Add(new Token(match.Index, match.Groups[1].Value != "" ? TokenType.Dont : TokenType.Do));
        }

        var score = 0;
        var doMul = true;

        foreach (var token in tokens.OrderBy(t => t.index))
        {
            if (doMul && token.type == TokenType.Mul)
            {
                score += token.score;
            }
            else if (token.type == TokenType.Dont)
            {
                doMul = false;
            }
            else if (token.type == TokenType.Do)
            {
                doMul = true;
            }
        }

        WriteLine(score);
    }

    record Token(int index, TokenType type, int score = 0);

    enum TokenType { Mul, Do, Dont, }

}