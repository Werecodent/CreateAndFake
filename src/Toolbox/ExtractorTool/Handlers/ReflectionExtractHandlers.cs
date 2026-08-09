using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class ReflectionExtractHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<IExtractHandler> Handlers { get; } =
        RuntimeDetails
            .RuntimeTypes.Select(t => new SelfExtractHandler(t))
            .Concat([new SelfExtractHandler(typeof(AssemblyName))]);
}
