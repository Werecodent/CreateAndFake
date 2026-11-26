using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class ReflectionCompareHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICompareHandler> Handlers { get; } =
        RuntimeDetails
            .RuntimeTypes.Select(t => new DefaultEqualityCompareHandler(t))
            .Concat([new DefaultEqualityCompareHandler(typeof(AssemblyName))]);
}
