using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.MutatorTool.Engine;

namespace Werecodent.CreateAndFake.MutatorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class ReflectionMutateHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<IMutateHandler> Handlers { get; } =
        RuntimeDetails
            .RuntimeTypes.Select(t => new NoMutateHandler(t))
            .Concat([new NoMutateHandler(typeof(AssemblyName))]);
}
