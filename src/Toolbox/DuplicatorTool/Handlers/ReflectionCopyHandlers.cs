using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Handlers;

internal static class ReflectionCopyHandlers
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICopyHandler> Handlers { get; } =
        RuntimeDetails
            .RuntimeTypes.Select(t => new RefCopyHandler(t))
            .Concat([new RefCopyHandler(typeof(AssemblyName))]);
}
