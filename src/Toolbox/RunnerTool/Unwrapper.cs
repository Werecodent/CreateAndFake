using System.Collections;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Types;

namespace CreateAndFake.RunnerTool;

/// <summary>Extracts the contents from containers.</summary>
internal static class Unwrapper
{
    private static readonly MethodInfo _EnumerateAsync = typeof(Unwrapper).GetMethod(
        nameof(EnumerateAsync),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;

    private static readonly MethodInfo _Enumerate = typeof(Unwrapper).GetMethod(
        nameof(Enumerate),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;

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
            return await UnwrapTask(
                    RunGenericUnwrap(_EnumerateAsync, typeof(IAsyncEnumerable<>), result, options)!
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
            return RunGenericUnwrap(_Enumerate, typeof(IEnumerable<>), result, options);
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
            !TypeDescriber
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

    private static object? RunGenericUnwrap(
        MethodInfo method,
        Type wrapperType,
        object result,
        RunnerOptions options
    )
    {
        return method
            .MakeGenericMethod(
                TypeDescriber.FindConcreteType(result.GetType(), wrapperType).GetGenericArguments()
            )
            .Invoke(null, [result, options]);
    }

    private static List<T> Enumerate<T>(object syncData, RunnerOptions options)
    {
        int i = 0;
        List<T> results = [];
        foreach (T item in (IEnumerable<T>)syncData)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, options.Valuer.Options.IterationLimit);
            results.Add(item);
        }
        return results;
    }

    private static Task<IList<T>> EnumerateAsync<T>(object asyncData, RunnerOptions options)
    {
        return AsyncSeriesHelper.ToListAsync(
            (IAsyncEnumerable<T>)asyncData,
            options.Valuer.Options.IterationLimit,
            CancellationToken.None
        );
    }
}
