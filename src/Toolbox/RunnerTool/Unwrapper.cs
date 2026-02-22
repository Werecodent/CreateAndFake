using System.Collections;
using System.Reflection;
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
    /// <returns>The unwrapped result.</returns>
    internal static async Task<object?> UnwrapResult(Func<object?> call)
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
                    RunGenericUnwrap(_EnumerateAsync, typeof(IAsyncEnumerable<>), result)!
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
            return RunGenericUnwrap(_Enumerate, typeof(IEnumerable<>), result);
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

    private static object? RunGenericUnwrap(MethodInfo method, Type wrapperType, object result)
    {
        return method
            .MakeGenericMethod(
                TypeDescriber.FindConcreteType(result.GetType(), wrapperType).GetGenericArguments()
            )
            .Invoke(null, [result]);
    }

    private static IList<T> Enumerate<T>(object syncData)
    {
        return [.. (IEnumerable<T>)syncData];
    }

    private static Task<IList<T>> EnumerateAsync<T>(object asyncData)
    {
        return AsyncEnumHelper.ToListAsync((IAsyncEnumerable<T>)asyncData, CancellationToken.None);
    }
}
