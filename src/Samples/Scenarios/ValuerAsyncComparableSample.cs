using CreateAndFake.Design;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public class ValuerAsyncComparableSample : IValuerAsyncComparable
{
    public string? StringValue { get; set; }

    public int NumberValue;

    public virtual async IAsyncEnumerable<Difference> CompareAsync(object? other, IValuer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        if (other is ValuerAsyncComparableSample sample)
        {
            await foreach (
                Difference diff in valuer
                    .CompareAsync(StringValue, sample.StringValue)
                    .ConfigureAwait(false)
            )
            {
                yield return new Difference($".{nameof(StringValue)}", diff);
            }
            await foreach (
                Difference diff in valuer
                    .CompareAsync(NumberValue, sample.NumberValue)
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

    public virtual Task<int> GetValueHashAsync(IValuer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.GetHashCodeAsync(new object?[] { StringValue, NumberValue });
    }
}
