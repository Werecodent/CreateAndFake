using System.Collections;
using Werecodent.CreateAndFake.Design;

namespace Werecodent.CreateAndFake.ValuerTool;

/// <summary>Provides specialized behavior based upon <see cref="Valuer"/> functionality.</summary>
/// <remarks>Behavior here might be integrated into actual features at some point.</remarks>
internal static class ValuerHelpers
{
    internal static async Task<IDictionary<int, IList<object>>> ByHashesAsync(
        IEnumerable set,
        IValuer valuer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(set, valuer);

        Dictionary<int, IList<object>> result = [];
        foreach (object item in set)
        {
            int valueHash = await valuer.GetHashCodeAsync(item, canceler).ConfigureAwait(false);

            if (result.TryGetValue(valueHash, out IList<object>? values))
            {
                values.Add(item);
            }
            else
            {
                result[valueHash] = [item];
            }
        }
        return result;
    }

    internal static async Task<ICollection<object>> FindAllIntersectAsync(
        IEnumerable<object> seriesA,
        IEnumerable<object> seriesB,
        IValuer valuer,
        CancellationToken canceler
    )
    {
        IDictionary<int, IList<object>> byHashA = await ByHashesAsync(seriesA, valuer, canceler)
            .ConfigureAwait(false);
        IDictionary<int, IList<object>> byHashB = await ByHashesAsync(seriesB, valuer, canceler)
            .ConfigureAwait(false);

        List<object> intersects = [];
        foreach (KeyValuePair<int, IList<object>> pair in byHashA)
        {
            if (byHashB.TryGetValue(pair.Key, out IList<object>? match))
            {
                foreach (object item in pair.Value)
                {
                    foreach (object found in match)
                    {
                        if (await valuer.EqualsAsync(item, found, canceler).ConfigureAwait(false))
                        {
                            intersects.Add(item);
                        }
                    }
                }
            }
        }
        return intersects;
    }
}
