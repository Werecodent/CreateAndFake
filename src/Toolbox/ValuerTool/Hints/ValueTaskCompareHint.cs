using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

/// <inheritdoc/>
public sealed class ValueTaskCompareHint : CompareHint
{
    /// <inheritdoc/>
    protected override bool EnableValueHashForComparisons => false;

    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.ValueTaskHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(ValueTask<>)];

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        return TypeDescriber.For(expected.GetType()).Inherits(typeof(ValueTask<>));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer chainer
    )
    {
        if (expected.GetType() != actual.GetType())
        {
            return [new Difference(expected.GetType(), actual.GetType())];
        }
        else
        {
            return CompareTasks((dynamic)expected, (dynamic)actual, chainer);
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer chainer)
    {
        return GetTaskHashCode((dynamic)item, chainer);
    }

    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<Difference> CompareTasks<T>(
        ValueTask<T> expected,
        ValueTask<T> actual,
        IValuerChainer chainer
    )
    {
        SingleCallValueTaskSource<T>? expectedSource = SingleCallValueTaskSource<T>.ExtractFrom(
            expected
        );
        SingleCallValueTaskSource<T>? actualSource = SingleCallValueTaskSource<T>.ExtractFrom(
            actual
        );

        foreach (Difference diff in chainer.Compare(expectedSource, actualSource))
        {
            yield return new Difference("(Source)", diff);
        }

        foreach (
            Difference diff in chainer.Compare(
                SingleCallValueTaskSource<T>.ExtractTokenFrom(expected),
                SingleCallValueTaskSource<T>.ExtractTokenFrom(actual)
            )
        )
        {
            yield return new Difference("(Token)", diff);
        }
    }

    /// <inheritdoc cref="GetHashCode"/>
    private static int GetTaskHashCode<T>(ValueTask<T> item, IValuerChainer chainer)
    {
        int hash = chainer.GetHashCode(SingleCallValueTaskSource<T>.ExtractTokenFrom(item));

        SingleCallValueTaskSource<T>? source = SingleCallValueTaskSource<T>.ExtractFrom(item);
        if (source != null)
        {
            hash = hash * ValueComparer.HashMultiplier + chainer.GetHashCode(source);
        }

        return hash;
    }
}
