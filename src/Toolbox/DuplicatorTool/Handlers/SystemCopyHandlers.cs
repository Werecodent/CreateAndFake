using System.Globalization;
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
        new FactoryCopyHandler<Guid>((source, _) => new Guid(source.ToByteArray())),
        new FactoryCopyHandler<Uri>((source, _) => new Uri(source.OriginalString)),
        new FactoryCopyHandler<ValueTuple>((_, __) => ValueTuple.Create()),
        new RefCopyHandler(typeof(TimeZoneInfo)),
        new RefCopyHandler(typeof(ECCurve)),
        new RefCopyHandler(typeof(UIntPtr)),
        new RefCopyHandler(typeof(IntPtr)),
        new RefCopyHandler(typeof(string)),
        new RefCopyHandler(typeof(object)),
        new RefCopyHandler(typeof(StringSegmentComparer)),
        new RefCopyHandler(typeof(RuntimeMethodHandle)),
        new RefCopyHandler(typeof(DateTimeFormatInfo)),
        new RefCopyHandler(typeof(NumberFormatInfo)),
        new RefCopyHandler(typeof(CultureInfo)),
        new FactoryCopyHandler<UriBuilder>((source, _) => new UriBuilder(source.Uri)),
        new FactoryCopyHandler<WeakReference>(
            (source, _) => new WeakReference(source.Target, source.TrackResurrection)
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
        new FactoryCopyHandler<CancellationToken>(
            (source, _) => new CancellationToken(source.IsCancellationRequested)
        ),
#if NET9_0_OR_GREATER
        new RefCopyHandler(typeof(Lock)),
#endif
    ];
}
