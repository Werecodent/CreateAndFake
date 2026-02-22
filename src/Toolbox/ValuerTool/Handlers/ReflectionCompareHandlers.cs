using System.Reflection;
using CreateAndFake.Design.Types;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Handlers;

internal static class ReflectionCompareHandlers
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICompareHandler> Handlers { get; } =
        RuntimeDetails
            .RuntimeTypes.Select(t => new DefaultEqualityCompareHandler(t))
            .Concat([new DefaultEqualityCompareHandler(typeof(AssemblyName))]);
}
