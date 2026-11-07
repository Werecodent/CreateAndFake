using System.Collections;
using System.Collections.Specialized;
using System.Numerics;
using System.Text;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class SystemCompareHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICompareHandler> Handlers { get; } =
    [
        new DefaultEqualityCompareHandler(typeof(string)),
        new DefaultEqualityCompareHandler(typeof(RuntimeMethodHandle)),
        new ConvertCompareHandler<StringBuilder>((s, _) => s.ToString()),
        new ConvertCompareHandler<UriBuilder>((s, _) => s.Uri),
        new ConvertCompareHandler<StreamReader>((reader, _) => reader.BaseStream),
        new ConvertCompareHandler<Vector2>((v, _) => new { v.X, v.Y }),
        new ConvertCompareHandler<Vector3>(
            (v, _) =>
                new
                {
                    v.X,
                    v.Y,
                    v.Z,
                }
        ),
        new ConvertCompareHandler<Vector4>(
            (v, _) =>
                new
                {
                    v.X,
                    v.Y,
                    v.Z,
                    v.W,
                }
        ),
        new ConvertCompareHandler<StreamWriter>(
            (writer, _) =>
            {
                writer.Flush();
                return writer.BaseStream;
            }
        ),
        new ConvertCompareHandler<CancellationToken>(
            (token, _) => new { token.CanBeCanceled, token.IsCancellationRequested }
        ),
        new ConvertCompareHandler<StringDictionary>(
            (dict, _) =>
                dict.Cast<DictionaryEntry>().ToDictionary(e => (string)e.Key, e => (string?)e.Value)
        ),
        new ConvertCompareHandler<ValueTask>(
            (task, _) =>
                new
                {
                    SingleCallSource = SingleCallValueTaskSource.ExtractFrom(task),
                    Token = SingleCallValueTaskSource.ExtractTokenFrom(task),
                }
        ),
#if NET9_0_OR_GREATER
        new DefaultEqualityCompareHandler(typeof(Lock)),
#endif
    ];
}
