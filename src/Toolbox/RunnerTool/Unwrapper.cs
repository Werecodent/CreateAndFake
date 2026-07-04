using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Types;

namespace CreateAndFake.RunnerTool;

/// <summary>Extracts the contents from containers.</summary>
internal static class Unwrapper
{
    /// <summary>Ensures the result is completed.</summary>
    /// <param name="call">Potentially wrapped data.</param>
    /// <param name="options">Configured options to apply to this call.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The unwrapped result.</returns>
    internal static async Task<object?> UnwrapResult(
        Func<object?> call,
        RunnerOptions options,
        CancellationToken canceler
    )
    {
        object? result = call?.Invoke();

        TypeDescriber describer = TypeDescriber.For(result?.GetType());
        if (
            describer.Inherits<Task>()
            || describer.Inherits<ValueTask>()
            || describer.Inherits(typeof(ValueTask<>))
        )
        {
            result = await UnwrapTask(result!).ConfigureAwait(false);
        }

        if (result == null)
        {
            return null;
        }

        Type resultType = result.GetType();

        if (resultType.Inherits(typeof(ICollection<>)))
        {
            return result;
        }

        // Required to execute async yield return methods.
        if (resultType.Inherits(typeof(IAsyncEnumerable<>)))
        {
            dynamic collection = await AsyncSeriesHelper
                .ToListAsync((dynamic)result, options.Valuer.Options.IterationLimit, canceler)
                .ConfigureAwait(false);

            return AsyncSeriesHelper.CreateFromAsync(
                collection,
                options.Valuer.Options.IterationLimit,
                canceler
            );
        }

        // Required to execute yield return methods.
        if (resultType.Inherits(typeof(IEnumerable<>)))
        {
            return Enumerable.AsEnumerable(CollectYieldedResults((dynamic)result, options));
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
            !GenericConverter
                .ExpandName(resultType)
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

    /// <summary>Iterates through yielded results.</summary>
    /// <typeparam name="T">The <paramref name="series"/>' item <see cref="Type"/>.</typeparam>
    /// <param name="series">Items to iterate.</param>
    /// <param name="options">Configured options to apply to this call.</param>
    /// <returns>The collected <paramref name="series"/>.</returns>
    private static List<T> CollectYieldedResults<T>(IEnumerable<T> series, RunnerOptions options)
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
