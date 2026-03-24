using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Types;

namespace CreateAndFake.RunnerTool;

/// <summary>Extracts the contents from containers.</summary>
internal static class Unwrapper
{
    /// <summary>Ensures the result is completed.</summary>
    /// <param name="call">Potentially wrapped data.</param>
    /// <param name="options">Options for unwrapping.</param>
    /// <returns>The unwrapped result.</returns>
    internal static async Task<object?> UnwrapResult(Func<object?> call, RunnerOptions options)
    {
        object? result = call.Invoke();

        while (
            result != null
            && (
                result.GetType().Inherits<Task>()
                || result.GetType().Inherits<ValueTask>()
                || result.GetType().Inherits(typeof(ValueTask<>))
            )
        )
        {
            result = await UnwrapTask(result).ConfigureAwait(false);
        }

        if (result == null)
        {
            return null;
        }

        Type resultType = result.GetType();
        if (resultType.Inherits(typeof(IAsyncEnumerable<>)))
        {
            return await AsyncSeriesHelper
                .ToListAsync(
                    (dynamic)result,
                    options.Valuer.Options.IterationLimit,
                    CancellationToken.None
                )
                .ConfigureAwait(false);
        }

        if (
            resultType.Inherits<ICollection>()
            || resultType.Inherits(typeof(ICollection<>))
            || resultType == typeof(string)
        )
        {
            return result;
        }

        // Required to execute yield return methods.
        if (resultType.Inherits(typeof(IEnumerable<>)))
        {
            return Collect((dynamic)result, options);
        }

        return result;
    }

    /// <summary>Ensures the result is completed.</summary>
    /// <param name="result">Potentially wrapped data.</param>
    /// <returns>The unwrapped result.</returns>
    private static async Task<object?> UnwrapTask(object result)
    {
        Type resultType = result.GetType();
        if (
            !GenericTypeConverter
                .ExpandedName(resultType)
                .Contains("VoidTaskResult", StringComparison.Ordinal)
            && resultType.GetProperty("Result") != null
        )
        {
            return await ((dynamic)result).ConfigureAwait(false);
        }
        else
        {
            await ((dynamic)result).ConfigureAwait(false);
            return VoidReturn.Instance;
        }
    }

    private static List<T> Collect<T>(IEnumerable<T> series, RunnerOptions options)
    {
        int i = 0;
        List<T> results = [];
        foreach (T item in series)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, options.Valuer.Options.IterationLimit);
            results.Add(item);
        }
        return results;
    }
}
