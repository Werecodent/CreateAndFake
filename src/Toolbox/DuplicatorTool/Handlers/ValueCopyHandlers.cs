using CreateAndFake.Design.Randomization;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class ValueCopyHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICopyHandler> Handlers { get; } =
        ValueRandom.SupportedTypes.Select(t => new RefCopyHandler(t));
}
