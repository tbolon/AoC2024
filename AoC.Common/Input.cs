using System.Reflection;
using System.Runtime.CompilerServices;


namespace AoC;

/// <summary>Helper for puzzle input.</summary>
public static partial class Input
{
    /// <summary>
    /// Returns input splitted by lines, materialized in an array.
    /// </summary>
    public static string[] GetLinesArray(bool sample = false, [CallerFilePath] string path = "") => [.. GetLines(sample, path)];

    /// <summary>
    /// Returns input splitted by lines.
    /// </summary>
    public static IEnumerable<string> GetLines(bool sample = false, [CallerFilePath] string path = "") => GetInput(sample, path).Split('\n', options: StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l));

    /// <summary>
    /// Returns entire input as a single string.
    /// input is then cached on disk.
    /// </summary>
    public static string GetInput(bool sample = false, [CallerFilePath] string path = "")
    {
        // xxx\2024\Day02.cs
        var yearAndPath = DetectYearAndDayFromPathRegex().Match(path);
        if (!yearAndPath.Success)
        {
            throw new ArgumentOutOfRangeException(nameof(path), path, $"Callers should use class format [Year]/Day[DayNumber].cs");
        }

        var year = yearAndPath.Groups[1].Value.AsInt();
        var day = yearAndPath.Groups[2].Value.AsInt();

        if (!Directory.Exists(year.ToString()))
            Directory.CreateDirectory(year.ToString());
        var filename = $"{year}/Day{day:00}.txt";        

        if (sample)
        {            
            // 1) recherche sur le disque
            filename = $"{year}/Day{day:00}.sample.txt";
            if (File.Exists(filename)) return File.ReadAllText(filename);

            // 2) recherche dans l'assembly
            var assembly = Assembly.Load($"AoC.{year}") ?? throw new NotSupportedException($"Impossible de charger l'assembly AoC.{year}");
            var stream = assembly.GetManifestResourceStream($"AoC{year}.Day{day:00}.sample.txt") ?? throw new NotSupportedException($"Impossible de charger la ressource AoC{year}.Day{day:00}.sample.txt");
            using(var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }

            throw new NotSupportedException($"Impossible de charger le fichier exemple s'il n'est pas déjà présent sur le disque ou dans l'assembly. Vous devez créer le fichier {filename} sur le disque avec le contenu de l'exemple");
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
