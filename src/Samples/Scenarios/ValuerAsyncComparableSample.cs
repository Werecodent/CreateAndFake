using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public class ValuerAsyncComparableSample : IValuerAsyncComparable
{
    public string? StringValue { get; set; }

    public int NumberValue;

    public virtual async IAsyncEnumerable<Difference> CompareAsync(
        object? other,
        IValuer valuer,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        if (other is ValuerAsyncComparableSample sample)
        {
            await foreach (
                Difference diff in valuer
                    .CompareAsync(StringValue, sample.StringValue, canceler)
                    .ConfigureAwait(false)
            )
            {
                yield return new Difference($".{nameof(StringValue)}", diff);
            }
            await foreach (
                Difference diff in valuer
                    .CompareAsync(NumberValue, sample.NumberValue, canceler)
                    .ConfigureAwait(false)
            )
            {
                yield return new Difference($".{nameof(NumberValue)}", diff);
            }
        }
        else
        {
            yield return new Difference(GetType(), other?.GetType());
        }
    }

    public virtual Task<int> GetValueHashAsync(IValuer valuer, CancellationToken canceler)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.GetHashCodeAsync(new object?[] { StringValue, NumberValue }, canceler);
    }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
