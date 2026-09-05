using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class ValueCopyHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICopyHandler> Handlers { get; } =
        ValueRandom.SupportedTypes.Select(t => new RefCopyHandler(t));
}
