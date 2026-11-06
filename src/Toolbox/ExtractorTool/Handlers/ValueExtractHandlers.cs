using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class ValueExtractHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<IExtractHandler> Handlers { get; } =
        ValueRandom.SupportedTypes.Select(t => new SelfExtractHandler(t));
}
