using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Primitives;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class SystemCopyHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICopyHandler> Handlers { get; } =
    [
        new FactoryCopyHandler<StringBuilder>((source, _) => new StringBuilder(source.ToString())),
        new FactoryCopyHandler<Uri>((source, _) => new Uri(source.OriginalString)),
        new RefCopyHandler(typeof(TimeZoneInfo)),
        new RefCopyHandler(typeof(ECCurve)),
        new RefCopyHandler(typeof(string)),
        new RefCopyHandler(typeof(object)),
        new RefCopyHandler(typeof(StringSegmentComparer)),
        new RefCopyHandler(typeof(RuntimeMethodHandle)),
        new FactoryCopyHandler<Vector2>((source, _) => new Vector2(source.X, source.Y)),
        new FactoryCopyHandler<Vector3>((source, _) => new Vector3(source.X, source.Y, source.Z)),
        new FactoryCopyHandler<Vector4>(
            (source, _) => new Vector4(source.X, source.Y, source.Z, source.W)
        ),
        new FactoryCopyHandler<UriBuilder>((source, _) => new UriBuilder(source.Uri)),
        new FactoryCopyHandler<DateTimeFormatInfo>(
            (source, _) => source.IsReadOnly ? source : (DateTimeFormatInfo)source.Clone()
        ),
        new FactoryCopyHandler<NumberFormatInfo>(
            (source, _) => source.IsReadOnly ? source : (NumberFormatInfo)source.Clone()
        ),
        new FactoryCopyHandler<CultureInfo>(
            (source, _) => source.IsReadOnly ? source : (CultureInfo)source.Clone()
        ),
        new FactoryCopyHandler<StreamReader>(
            (source, duplicator) => new StreamReader(duplicator.Copy(source.BaseStream))
        ),
        new FactoryCopyHandler<StreamWriter>(
            (source, duplicator) =>
            {
                source.Flush();
                return new StreamWriter(duplicator.Copy(source.BaseStream));
            }
        ),
        new FactoryCopyHandler<CancellationTokenSource>(
            (source, _) =>
            {
                CancellationTokenSource result = new();
                if (source.IsCancellationRequested)
                {
                    result.Cancel();
                }
                return result;
            }
        ),
#pragma warning disable S2930 // Must remain undisposed to last the life of the token.
        new FactoryCopyHandler<CancellationToken>(
            (source, _) =>
            {
                if (source.CanBeCanceled)
                {
                    CancellationTokenSource tokenSource = new();
                    if (source.IsCancellationRequested)
                    {
                        tokenSource.Cancel();
                    }
                    return tokenSource.Token;
                }
                else
                {
                    return new CancellationToken(source.IsCancellationRequested);
                }
            }
        ),
#pragma warning restore S2930
#if NET9_0_OR_GREATER
        new RefCopyHandler(typeof(Lock)),
#endif
    ];
}
