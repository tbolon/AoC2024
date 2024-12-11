using System.Diagnostics;

var sw = Stopwatch.StartNew();
var result = AoC2024.Day11.Solve();
sw.Stop();
SysConsole.WriteLine($"{result} in {sw.ElapsedMilliseconds} ms");
