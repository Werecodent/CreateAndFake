using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Comparisons;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Tooling;
using CreateAndFake.Design.Types;

namespace CreateAndFake.ValuerTool.Engine;

/// <inheritdoc cref="IValuer"/>
public sealed class ValuerEngine : ToolEngine<ICompareHint>, IValuerEngine
{
    /// <inheritdoc/>
    public IEnumerable<Difference> Compare(object? expected, object? actual, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (ReferenceEquals(expected, actual))
        {
            return [];
        }
        else if (expected is null || actual is null)
        {
            return [new Difference(expected, actual)];
        }

        DifferenceHintResult? result;
        try
        {
            result = SelectHints(chainer)
                .Select(h => h.TryCompare(expected, actual, chainer))
                .FirstOrDefault(r => r?.HasData ?? false);
        }
        catch (Exception e)
        {
            throw new ToolException(
                $"Error comparing instance of type '{TypeDescriber.ExpandedName(expected)}' "
                    + $"with an instance of type '{TypeDescriber.ExpandedName(actual)}'.",
                e
            );
        }

        if (result != null)
        {
            return WithErrorHandling(result.Data!, expected, actual);
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{TypeDescriber.ExpandedName(expected)}' not supported by the valuer. "
                    + "Create a hint to generate the type."
            );
        }
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (ReferenceEquals(expected, actual))
        {
            return AsyncSeriesHelper<Difference>.Empty;
        }
        else if (expected is null || actual is null)
        {
            return AsyncSeriesHelper.CreateFromAsync([new Difference(expected, actual)], canceler);
        }

        DifferenceHintAsyncResult? result;
        try
        {
            result = SelectHints(chainer)
                .Select(h => h.TryAsyncCompare(expected, actual, chainer, canceler))
                .FirstOrDefault(r => r?.HasData ?? false);
        }
        catch (Exception e)
        {
            throw new ToolException(
                $"Error comparing instance of type '{TypeDescriber.ExpandedName(expected)}' "
                    + $"with an instance of type '{TypeDescriber.ExpandedName(actual)}'.",
                e
            );
        }

        if (result != null)
        {
            return WithErrorHandlingAsync(result.Data!, expected, actual, canceler);
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{TypeDescriber.ExpandedName(expected)}' not supported by the valuer. "
                    + "Create a hint to generate the type and pass it to the valuer."
            );
        }
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (item is null)
        {
            return ValueComparer.NullHash;
        }

        HashCodeHintResult? result;
        try
        {
            result = SelectHints(chainer)
                .Select(h => h.TryGetHashCode(item, chainer))
                .FirstOrDefault(r => r?.HasData ?? false);
        }
        catch (Exception e)
        {
            throw new ToolException(
                $"Error hashing instance of type '{TypeDescriber.ExpandedName(item)}'.",
                e
            );
        }

        if (result != null)
        {
            return result.Data;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{TypeDescriber.ExpandedName(item)}' not supported by the valuer. "
                    + "Create a hint to generate the type and pass it to the valuer."
            );
        }
    }

    /// <inheritdoc/>
    public Task<int> GetHashCodeAsync(
        object? item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (item is null)
        {
            return Task.FromResult(ValueComparer.NullHash);
        }

        HashCodeHintAsyncResult? result;
        try
        {
            result = SelectHints(chainer)
                .Select(h => h.TryAsyncGetHashCode(item, chainer, canceler))
                .FirstOrDefault(r => r?.HasData ?? false);
        }
        catch (Exception e)
        {
            throw new ToolException(
                $"Error hashing instance of type '{TypeDescriber.ExpandedName(item)}'.",
                e
            );
        }

        if (result != null)
        {
            return WithErrorHandlingAsync(result.Data!, item);
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{TypeDescriber.ExpandedName(item)}' not supported by the valuer."
                    + "Create a hint to generate the type and pass it to the valuer."
            );
        }
    }

    /// <summary>Iterates the result and wraps any exception encountered with details.</summary>
    /// <param name="result">Result of the tool hint being returned.</param>
    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<T> WithErrorHandling<T>(
        IEnumerable<T> result,
        object? expected,
        object? actual
    )
    {
        IEnumerator<T> enumerator = result.GetEnumerator();
        try
        {
            bool hasNext;
            do
            {
                try
                {
                    hasNext = enumerator.MoveNext();
                }
                catch (Exception e)
                {
                    throw new ToolException(
                        $"Error comparing instance of type '{TypeDescriber.ExpandedName(expected)}'"
                            + $" with an instance of type '{TypeDescriber.ExpandedName(actual)}'.",
                        e
                    );
                }

                if (hasNext)
                {
                    yield return enumerator.Current;
                }
            } while (hasNext);
        }
        finally
        {
            Disposer.Cleanup(enumerator);
        }
    }

    /// <summary>Iterates the result and wraps any exception encountered with details.</summary>
    /// <param name="result">Result of the tool hint being returned.</param>
    /// <inheritdoc cref="CompareAsync"/>
    private static async IAsyncEnumerable<T> WithErrorHandlingAsync<T>(
        IAsyncEnumerable<T> result,
        object? expected,
        object? actual,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        IAsyncEnumerator<T> enumerator = result.GetAsyncEnumerator(canceler);
        await using (enumerator.ConfigureAwait(false))
        {
            bool hasNext;
            do
            {
                try
                {
                    canceler.ThrowIfCancellationRequested();
                    hasNext = await enumerator.MoveNextAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw new ToolException(
                        "Error comparing instance of type "
                            + $"'{TypeDescriber.ExpandedName(expected)}' with an "
                            + $"instance of type '{TypeDescriber.ExpandedName(actual)}'.",
                        e
                    );
                }

                if (hasNext)
                {
                    yield return enumerator.Current;
                }
            } while (hasNext);
        }
    }

    /// <summary>Awaits the result and wraps any exception encountered with details.</summary>
    /// <param name="result">Result of the tool hint being returned.</param>
    /// <inheritdoc cref="GetHashCodeAsync"/>
    private static async Task<T> WithErrorHandlingAsync<T>(Task<T> result, object? item)
    {
        try
        {
            return await result.ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new ToolException(
                $"Error hashing instance of type '{TypeDescriber.ExpandedName(item)}'.",
                e
            );
        }
    }
}
