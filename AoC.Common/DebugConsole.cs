using Spectre.Console;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SysConsole = System.Console;

namespace AoC;


public static class DebugConsole
{
    /// <summary>
    /// Checks for a condition; if the condition is false, displays a message box that shows the call stack.
    /// </summary>
    /// <param name="condition">The conditional expression to evaluate. If the condition is true, a failure message is not sent and the message box is not displayed.</param>
    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public static void Assert(bool condition, [CallerArgumentExpression(nameof(condition))] string? message = null) => Debug.Assert(condition, message);

    [Conditional("DEBUG")]
    public static void Clear() => SysConsole.Clear();

    [Conditional("DEBUG")]
    public static void Markup(string value) => AnsiConsole.Console.Markup(value);

    [Conditional("DEBUG")]
    public static void SetCursorPosition(int left, int top) => SysConsole.SetCursorPosition(left, top);

    [Conditional("DEBUG")]
    public static void MarkupLine(string value) => AnsiConsole.Console.MarkupLine(value);

    [Conditional("DEBUG")]
    public static void WriteLine(string value) => SysConsole.WriteLine(value);

    [Conditional("DEBUG")]
    public static void WriteLine(bool value) => SysConsole.WriteLine(value);

    [Conditional("DEBUG")]
    public static void WriteLine(double value) => SysConsole.WriteLine(value);

    [Conditional("DEBUG")]
    public static void WriteLine(int value) => SysConsole.WriteLine(value);

    [Conditional("DEBUG")]
    public static void WriteLine() => SysConsole.WriteLine();

    [Conditional("DEBUG")]
    public static void WriteLine(object value) => SysConsole.WriteLine(value);

    [Conditional("DEBUG")]
    public static void Write(char c) => SysConsole.Write(c);

    [Conditional("DEBUG")]
    public static void Write(object c) => SysConsole.Write(c);

    [Conditional("DEBUG")]
    public static void Write(int c) => SysConsole.Write(c);

    [Conditional("DEBUG")]
    public static void Write(byte c) => SysConsole.Write(c);

    [Conditional("DEBUG")]
    public static void Write(char c, ConsoleColor color)
    {
        var t = SysConsole.ForegroundColor;
        if (t != color)
        {
            SysConsole.ForegroundColor = color;
            SysConsole.Write(c);
            SysConsole.ForegroundColor = t;
        }
        else
        {
            SysConsole.Write(c);
        }
    }

    [Conditional("DEBUG")]
    public static void Write(string text, ConsoleColor color)
    {
        var t = SysConsole.ForegroundColor;
        if (t != color)
        {
            SysConsole.ForegroundColor = color;
            SysConsole.Write(text);
            SysConsole.ForegroundColor = t;
        }
        else
        {
            SysConsole.Write(text);
        }
    }
}
