using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Handlers;

internal static class ReflectionMutateHandlers
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<IMutateHandler> Handlers { get; } =
        RuntimeDetails
            .RuntimeTypes.Select(t => new NoMutateHandler(t))
            .Concat([new NoMutateHandler(typeof(AssemblyName))]);
}
