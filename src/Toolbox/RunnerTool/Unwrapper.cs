using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.RunnerTool;

/// <summary>Extracts the contents from containers.</summary>
internal static class Unwrapper
{
    /// <summary>Ensures the result is completed.</summary>
    /// <param name="call">Potentially wrapped data.</param>
    /// <param name="options">Configured options to apply to this call.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The unwrapped result.</returns>
    internal static async Task<object?> UnwrapResultAsync(
        Func<object?> call,
        RunnerOptions options,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(options);

        object? result = call?.Invoke();
        if (result == null)
        {
            return null;
        }

        TypeDescriber describer = TypeDescriber.For(result.GetType());

        if (result is Task plainTask)
        {
            object? content = await TaskHelper.AwaitAsync(plainTask).ConfigureAwait(false);
            if (content == VoidType.Instance)
            {
                return Task.CompletedTask;
            }
            else
            {
                return result;
            }
        }
        else if (result is ValueTask valueTask)
        {
            await valueTask.ConfigureAwait(false);
            return Task.CompletedTask;
        }
        else if (describer.Inherits(typeof(ValueTask<>)))
        {
            dynamic? content = await ((dynamic)result).ConfigureAwait(false);
            if (content == null)
            {
                return typeof(Task)
                    .GetMethod(nameof(Task.FromResult))!
                    .MakeGenericMethod(result.GetType().GetGenericArguments().Single())
                    .Invoke(null, [null]);
            }
            else
            {
                return Task.FromResult(content);
            }
        }

        Type resultType = result.GetType();
        if (resultType == typeof(string) || resultType.Inherits(typeof(ICollection<>)))
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
