using System.Globalization;
using System.Text;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class SystemCopyHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICopyHandler> Handlers { get; } =
    [
        new FactoryCopyHandler<StringBuilder>((source, _) => new StringBuilder(source.ToString())),
        new FactoryCopyHandler<TimeSpan>((source, _) => new TimeSpan(source.Ticks)),
        new FactoryCopyHandler<Guid>((source, _) => new Guid(source.ToByteArray())),
        new FactoryCopyHandler<Uri>((source, _) => new Uri(source.OriginalString)),
        new RefCopyHandler(typeof(TimeZoneInfo)),
        new RefCopyHandler(typeof(DateTime)),
        new RefCopyHandler(typeof(decimal)),
        new RefCopyHandler(typeof(UIntPtr)),
        new RefCopyHandler(typeof(IntPtr)),
        new RefCopyHandler(typeof(string)),
        new RefCopyHandler(typeof(object)),
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
        /*new FactoryCopyHandler<WeakReference>(
            (source, _) => new WeakReference(source.Target, source.TrackResurrection)
        ),*/
        new FactoryCopyHandler<CancellationTokenSource>(
            (source, _) =>
            {
#pragma warning disable S2930 // Must only be GC when the resulting token has expired.
                CancellationTokenSource result = new();
#pragma warning restore S2930
                if (source.IsCancellationRequested)
                {
                    result.Cancel();
                }
                return source;
            }
        ),
        new FactoryCopyHandler<CancellationToken>(
            (source, _) => new CancellationToken(source.IsCancellationRequested)
        ),
    ];
}
