Day03.Solve();

static class ProgramHelper
{
    /// <summary>
    /// Checks for a condition; if the condition is false, displays a message box that shows the call stack.
    /// </summary>
    /// <param name="condition">The conditional expression to evaluate. If the condition is true, a failure message is not sent and the message box is not displayed.</param>
    public static void Assert(bool condition) => System.Diagnostics.Debug.Assert(condition);

    /// <summary>
    /// Checks for a condition; if the condition is false, outputs a specified message and displays a message box that shows the call stack.
    /// </summary>
    /// <param name="condition">The conditional expression to evaluate. If the condition is true, the specified message is not sent and the message box is not displayed.</param>
    /// <param name="message">The message to display.</param>
    public static void Assert(bool condition, string? message) => System.Diagnostics.Debug.Assert(condition, message);

    public static void ReadKey(bool intercept = true) => Console.ReadKey(intercept);

    public static void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);

    public static void Clear() => Console.Clear();

    public static void WriteLine(object value, ConsoleColor? color = null) => WriteLine(value?.ToString(), color);

    public static void WriteLine(string? message, ConsoleColor? color = null) => Console.Write(message + Environment.NewLine, color);

    public static void WriteLine() => Console.Write(Environment.NewLine);

    public static void Write(object value, ConsoleColor? color = null) => Write(value?.ToString(), color);

    public static void Write(string? message, ConsoleColor? color = null)
    {
        ConsoleColor? previousColor = null;
        if (color != null)
        {
            previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color.Value;
        }

        Console.Write(message);

        if (previousColor != null)
        {
            Console.ForegroundColor = previousColor.Value;
        }
    }
}

/// <summary>Helper for puzzle input.</summary>
static class Input
{
    public static string[] GetLinesArray(int day, bool sample = false) => GetLines(day, sample).ToArray();

    public static IEnumerable<string> GetLines(int day, bool sample = false) => GetFile(day, sample).Split('\n', options: StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l));

    /// <summary>
    /// Renvoie le contenu complet de l'input du jour x.
    /// </summary>
    public static string GetFile(int day, bool sample = false)
    {
        var filename = $"Day{day:00}.txt";

        if (sample)
        {
            filename = $"Day{day:00}.sample.txt";
            if (File.Exists(filename))
            {
                return File.ReadAllText(filename);
            }

            throw new NotSupportedException($"Impossible de charger le fichier exemple s'il n'est pas déjà présent sur le disque. Vous devez créer le fichier {filename} sur le disque avec le contenu de l'exemple");
        }

        if (File.Exists(filename))
        {
            return File.ReadAllText(filename);
        }

        var sessionId = Environment.GetEnvironmentVariable("AOC_SESSION");
        if (string.IsNullOrEmpty(sessionId))
            // pwsh: $env:AOC_SESSION = "..."
            throw new InvalidOperationException($"You must set AOC_SESSION environment variable with your AoC session cookie value");

        var cookieContainer = new System.Net.CookieContainer();
        cookieContainer.Add(new System.Net.Cookie("session", sessionId, "/", ".adventofcode.com"));

        using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
        using var client = new HttpClient(handler);

        var text = client.GetStringAsync($"https://adventofcode.com/2024/day/{day}/input").Result;

        File.WriteAllText(filename, text);

        return text;
    }
}
