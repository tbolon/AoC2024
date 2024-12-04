using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

SysConsole.OutputEncoding = System.Text.Encoding.UTF8;

// this program accepts <day> and <year> as command line parameters
// if omitted, current day and current year will be infered from today

string[] cmd = args;

do
{
    var day = cmd.FirstOrDefault()?.AsIntN() ?? DateTime.Today.Day;
    var year = cmd.Skip(1).FirstOrDefault()?.AsIntN() ?? DateTime.Today.Year;
    if (year < 100) year += 2000; // we accept two last digits of year as a valid input

    // find class
    MethodInfo? method = null;
    try
    {
        var classType = typeof(Program).Assembly.GetTypes().FirstOrDefault(t => t.Namespace == $"AoC{year}" && t.Name == $"Day{day:00}") ?? throw new NotSupportedException($"Can't find class AoC{year}.Day{day:00}");
        method = classType.GetMethod("Solve", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static) ?? throw new NotSupportedException($"Can't find method Solve() on AoC{year}.Day{day:00}");
    }
    catch (NotSupportedException ex)
    {
        MarkupLine($"[red]{ex.Message}[/]");
    }

    // if method exists, run it
    if (method != null)
    {
        // Solve(StatusContext ctx) method is supported to return intermediate value
        var passCtx = method.GetParameters()?.FirstOrDefault(p => p.ParameterType == typeof(StatusContext)) != null;

        // let's go
        MarkupLine($"🤖 Solving day {day} year {year}");

        if (year < 2024)
        {
            method.Invoke(null, null);
        }
        else
        {
            Status().Start("🧮 Computing...", ctx =>
            {
                var sw = Stopwatch.StartNew();

                object? result;
                try
                {
                    result = method.Invoke(null, passCtx ? new object[] { ctx } : null);
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                    result = null;
                }

                // we wait at least 1 second
                //if (sw.ElapsedMilliseconds < 1000) Thread.Sleep(1000 - (int)sw.ElapsedMilliseconds);

                if (result != null)
                {
                    MarkupLine($"💡 Result: [lime]{result}[/]");
                }
                else
                {
                    MarkupLine($"☠️ Method did not return any result...");
                }
            });

            MarkupLine($"🤖 Computing completed, press Enter to rerun the last command, or enter <day> <year>");
        }
    }
    else
    {
        MarkupLine($"🤖 Enter <day> <year> to run a day code");
    }

    // parse input, exit and quit command will exit (or press Ctrl+C)
    var input = SysConsole.ReadLine()?.ToLower();
    if (input == null || input == "quit" || input == "exit") break;
    if (input == string.Empty) input = $"{day} {year}";
    cmd = input.Split(' ');
} while (true);

MarkupLine("❤️ See ya");

static class Helper
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
}

/// <summary>Helper for puzzle input.</summary>
static partial class Input
{
    /// <summary>
    /// Returns input splitted by lines, materialized in an array.
    /// </summary>
    public static string[] GetLinesArray([CallerFilePath] string path = "", bool sample = false) => GetLines(path, sample).ToArray();

    /// <summary>
    /// Returns input splitted by lines.
    /// </summary>
    public static IEnumerable<string> GetLines([CallerFilePath] string path = "", bool sample = false) => GetInput(path, sample).Split('\n', options: StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l));

    /// <summary>
    /// Returns entire input as a single string.
    /// input is then cached on disk.
    /// </summary>
    public static string GetInput([CallerFilePath] string path = "", bool sample = false)
    {
        // xxx\2024\Day02.cs
        var yearAndPath = DetectYearAndDayFromPathRegex().Match(path);
        if (!yearAndPath.Success)
        {
            throw new ArgumentOutOfRangeException(nameof(path), path, $"Callers should use class format [Year]/Day[DayNumber].cs");
        }

        var year = yearAndPath.Groups[1].Value.AsInt();
        var day = yearAndPath.Groups[2].Value.AsInt();

        var filename = $"{year}/Day{day:00}.txt";

        if (sample)
        {
            filename = $"{year}/Day{day:00}.sample.txt";
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

        var text = client.GetStringAsync($"https://adventofcode.com/{year}/day/{day}/input").Result;

        File.WriteAllText(filename, text);

        return text;
    }

    [GeneratedRegex(@"(\d{4})[/\\]Day(\d\d)\.cs$", RegexOptions.CultureInvariant)]
    private static partial Regex DetectYearAndDayFromPathRegex();
}
