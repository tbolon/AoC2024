using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AoC;

public static class Helper
{
    /// <summary>
    /// Checks for a condition; if the condition is false, displays a message box that shows the call stack.
    /// </summary>
    /// <param name="condition">The conditional expression to evaluate. If the condition is true, a failure message is not sent and the message box is not displayed.</param>
    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public static void Assert(bool condition, [CallerArgumentExpression(nameof(condition))] string? message = null) => Debug.Assert(condition, message);
}
