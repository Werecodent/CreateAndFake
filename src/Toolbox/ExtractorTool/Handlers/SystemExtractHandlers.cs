using System.Numerics;
using System.Reflection;
using System.Text;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class SystemExtractHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<IExtractHandler> Handlers { get; } =
    [
        new SelfExtractHandler(typeof(Lock)),
        new SelfExtractHandler(typeof(string)),
        new SelfExtractHandler(typeof(AssemblyName)),
        new SelfExtractHandler(typeof(StringBuilder)),
        new SelfExtractHandler(typeof(CancellationToken)),
        new SelfExtractHandler(typeof(RuntimeMethodHandle)),
        new ConvertExtractHandler<Vector2>(x => [x.X, x.Y]),
        new ConvertExtractHandler<Vector3>(x => [x.X, x.Y, x.Z]),
        new ConvertExtractHandler<Vector4>(x => [x.X, x.Y, x.Z, x.W]),
        new ConvertExtractHandler<StreamReader>(x => [x.BaseStream]),
        new ConvertExtractHandler<StreamWriter>(x =>
        {
            x.Flush();
            return [x.BaseStream];
        }),
    ];
}
