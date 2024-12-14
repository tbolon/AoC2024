using System.Globalization;
using System.Numerics;

namespace AoC;

public static class Extensions
{
    public static IEnumerable<(T1 First, T2 Second)> CrossJoin<T1, T2>(this IEnumerable<T1> @this, IEnumerable<T2> other)
    {
        foreach (var first in @this)
        {
            foreach (var second in other)
            {
                yield return (first, second);
            }
        }
    }

    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> @this, IEnumerable<(TKey first, TValue second)> values) where TKey : notnull
    {
        foreach ((var first, var second) in values)
        {
            @this.Add(first, second);
        }
    }

    public static IDictionary<TKey, TValue> AddOrIncrement<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value) where TKey : notnull where TValue : INumber<TValue>
    {
        if (@this.TryGetValue(key, out var existing))
            @this[key] = existing + value;
        else
            @this[key] = value;

        return @this;
    }

    public static string StringJoin<T>(this IEnumerable<T> @this, string separator = ",") => string.Join(separator, @this);

    public static int IndexOf<T>(this T[] array, T value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (Equals(array[i], value))
                return i;
        }

        return -1;
    }

    public static T Middle<T>(this T[] @this)
    {
        Assert(@this.Length % 2 == 1);
        return @this[@this.Length / 2];
    }

    public static T Middle<T>(this IEnumerable<T> @this)
    {
        Assert(@this.Count() % 2 == 1);
        return @this.ElementAt(@this.Count() / 2);
    }

    public static TResult Transform<T, TResult>(this T[] @this, Func<T[], TResult> func) => func(@this);
}

public static class StringExtensions
{
    public static int AsInt(this string @this) => int.Parse(@this, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);

    public static int? AsIntN(this string? @this) => string.IsNullOrEmpty(@this) ? null : int.Parse(@this, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);

    public static string[] SplitSpace(this string @this, bool removeEmptyEntries = true) => @this.Split(' ', removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
}