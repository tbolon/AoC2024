using BenchmarkDotNet.Attributes;

namespace AoC2024
{
    [MemoryDiagnoser]
    public class Day07Bench
    {
        private readonly List<(long result, int[] values)> _formulas;

        public Day07Bench()
        {
            _formulas = Day07.LoadFormulas();
        }

        [Benchmark]
        public long Iteratif() => Day07.Solve_Iteratif(_formulas);

        [Benchmark]
        public long Recursif() => Day07.Solve_Recursive(_formulas);
    }
}
