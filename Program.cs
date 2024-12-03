using System.Diagnostics;

SysConsole.OutputEncoding = System.Text.Encoding.UTF8;

var day = args.FirstOrDefault().AsIntN() ?? DateTime.Today.Day;

var classType = typeof(Program).Assembly.GetTypes().FirstOrDefault(t => t.Name.StartsWith($"Day{day:00}")) ?? throw new NotSupportedException($"Can't find day {day}");
var method = classType.GetMethod("Solve", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static) ?? throw new NotSupportedException($"Can't find method Solve() on Day{day:00}");
var passCtx = method.GetParameters()?.FirstOrDefault(p => p.ParameterType == typeof(StatusContext)) != null;


MarkupLine($"🤖 Solving day {day}");

Status().Start("🧮 Computing...", ctx =>
{
    var sw = Stopwatch.StartNew();
    var result = method.Invoke(null, passCtx ? new object[] { ctx } : null);

    if (sw.ElapsedMilliseconds < 1000)
    {
        Thread.Sleep(1000 - (int)sw.ElapsedMilliseconds);
    }

    if (result != null)
    {
        MarkupLine($"💡 Result: [lime]{result}[/]");
    }
});

MarkupLine("❤️ Completed");

/// <summary>Helper for puzzle input.</summary>
static class Input
{
    /// <summary>
    /// Returns input splitted by lines, materialized in an array.
    /// </summary>
    public static string[] GetLinesArray(int day, bool sample = false) => GetLines(day, sample).ToArray();

    /// <summary>
    /// Returns input splitted by lines.
    /// </summary>
    public static IEnumerable<string> GetLines(int day, bool sample = false) => GetInput(day, sample).Split('\n', options: StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l));

    /// <summary>
    /// Returns entire input as a single string.
    /// input is then cached on disk.
    /// </summary>
    public static string GetInput(int day, bool sample = false)
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

static class Extensions
{
    public static int? AsIntN(this string? @this) => @this == null ? null : int.Parse(@this, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture);

    public static int AsInt(this string @this) => int.Parse(@this, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture);

    public static string[] SplitSpace(this string @this, bool removeEmptyEntries = true) => @this.Split(' ', removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
}