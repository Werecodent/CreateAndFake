using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Handlers;

internal static class ReflectionCompareHandlers
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICompareHandler> Handlers { get; } =
        RuntimeDetails
            .RuntimeTypes.Select(t => new DefaultEqualityCompareHandler(t))
            .Concat([new DefaultEqualityCompareHandler(typeof(AssemblyName))]);
}
