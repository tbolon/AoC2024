using AoC;
using Spectre.Console;
using System.Diagnostics;
using System.Reflection;
using static AoC.DebugConsole;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// this program accepts <day> and <year> as command line parameters
// if omitted, current day and current year will be infered from today

args = args.Skip(1).ToArray();

string[] cmd = args;

do
{
    var day = cmd.FirstOrDefault()?.AsIntN() ?? DateTime.Today.Day;
    var year = cmd.Skip(1).FirstOrDefault()?.AsIntN() ?? DateTime.Today.Year;
    if (year < 100) year += 2000; // we accept two last digits of year as a valid input

    // fancy or not?
    var fancy = year >= 2024;

    // find class
    MethodInfo? method = null;
    try
    {
        var assembly = Assembly.Load($"AoC.{year}") ?? throw new NotSupportedException($"Can't load AoC.{year} assembly");
        var classType = assembly.GetTypes().FirstOrDefault(t => t.Namespace == $"AoC{year}" && t.Name == $"Day{day:00}") ?? throw new NotSupportedException($"Can't find class AoC{year}.Day{day:00}");
        method = classType.GetMethod("Solve", BindingFlags.Public | BindingFlags.Static) ?? throw new NotSupportedException($"Can't find method Solve() on AoC{year}.Day{day:00}");
        fancy = fancy && method?.GetCustomAttribute<NoFancyAttribute>() == null;
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

        object? result = null;
        var sw = Stopwatch.StartNew();

        if (!fancy)
        {
            try
            {
                result = method.Invoke(null, null);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                result = null;
            }
        }
        else
        {
            AnsiConsole.Status().Start("🧮 Computing...", ctx =>
            {
                var sw = Stopwatch.StartNew();

                try
                {
                    result = method.Invoke(null, passCtx ? new object[] { ctx } : null);
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteException(ex);
                    result = null;
                }
            });

            MarkupLine($"🤖 Computing completed, press Enter to rerun the last command, or enter <day> <year>");
        }

        if (result != null)
        {
            MarkupLine($"💡 Result: [lime]{result}[/] (in {sw.Elapsed.TotalSeconds:n2}s)");
        }
        else
        {
            MarkupLine($"☠️ Method did not return any result...");
        }
    }
    else
    {
        MarkupLine($"🤖 Enter <day> <year> to run a day code");
    }

    // parse input, exit and quit command will exit (or press Ctrl+C)
    var input = Console.ReadLine()?.ToLower();
    if (input == null || input == "quit" || input == "exit") break;
    if (input == string.Empty) input = $"{day} {year}";
    cmd = input.Split(' ');
} while (true);

MarkupLine("❤️ See ya");
return 0;
