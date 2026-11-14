using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class ValueCompareHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICompareHandler> Handlers { get; } =
        ValueRandom.SupportedTypes.Select(t => new DefaultEqualityCompareHandler(t));
}
