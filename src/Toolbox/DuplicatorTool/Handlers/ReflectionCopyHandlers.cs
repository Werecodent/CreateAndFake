using System.Reflection;
using CreateAndFake.Design.Types;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class ReflectionCopyHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICopyHandler> Handlers { get; } =
        RuntimeDetails
            .RuntimeTypes.Select(t => new RefCopyHandler(t))
            .Concat([new RefCopyHandler(typeof(AssemblyName))]);
}
