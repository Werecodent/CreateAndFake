using System.Globalization;
using System.Text;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning common types for <see cref="IDuplicator"/> .</summary>
public sealed class CopierCopyHint : CopyHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly ICopier[] _Copiers =
    [
        new Copier<TimeSpan>((source, copier) => new TimeSpan(copier.Copy(source.Ticks))),
        new Copier<CultureInfo>(
            (source, _) => source.IsReadOnly ? source : (CultureInfo)source.Clone()
        ),
        new Copier<DateTimeFormatInfo>(
            (source, _) => source.IsReadOnly ? source : (DateTimeFormatInfo)source.Clone()
        ),
        new Copier<NumberFormatInfo>(
            (source, _) => source.IsReadOnly ? source : (NumberFormatInfo)source.Clone()
        ),
        new Copier<StringBuilder>((source, _) => new StringBuilder(source.ToString())),
        new Copier<Uri>((source, _) => new Uri(source.OriginalString)),
        new Copier<Guid>((source, _) => new Guid(source.ToByteArray())),
        new Copier<WeakReference>(
            (source, _) => new WeakReference(source.Target, source.TrackResurrection)
        ),
        new Copier<IntPtr>((source, _) => new IntPtr((int)source)),
        new Copier<CancellationTokenSource>(
            (source, _) =>
            {
#pragma warning disable S2930 // Must be GC when the resulting token is expired.
                CancellationTokenSource result = new();
#pragma warning restore S2930
                if (source.IsCancellationRequested)
                {
                    result.Cancel();
                }
                return source;
            }
        ),
    ];

    private static readonly IDictionary<Type, ICopier> _CopiersByType =
        TypeSupporter.GroupBySupportedType(_Copiers);

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator);

        if (source != null && _CopiersByType.TryGetValue(source.GetType(), out ICopier? copier))
        {
            return new(copier.CopySupported(source, duplicator));
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}
