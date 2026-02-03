using System.Globalization;
using System.Text;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.DuplicatorTool.Handlers;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning common types for <see cref="IDuplicator"/> .</summary>
public sealed class HandlerCopyHint : CopyHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly ICopyHandler[] _Copiers =
    [
        new FactoryCopyHandler<TimeSpan>(
            (source, copier) => new TimeSpan(copier.Copy(source.Ticks))
        ),
        new FactoryCopyHandler<CultureInfo>(
            (source, _) => source.IsReadOnly ? source : (CultureInfo)source.Clone()
        ),
        new FactoryCopyHandler<DateTimeFormatInfo>(
            (source, _) => source.IsReadOnly ? source : (DateTimeFormatInfo)source.Clone()
        ),
        new FactoryCopyHandler<NumberFormatInfo>(
            (source, _) => source.IsReadOnly ? source : (NumberFormatInfo)source.Clone()
        ),
        new FactoryCopyHandler<StringBuilder>((source, _) => new StringBuilder(source.ToString())),
        new FactoryCopyHandler<Uri>((source, _) => new Uri(source.OriginalString)),
        new FactoryCopyHandler<Guid>((source, _) => new Guid(source.ToByteArray())),
        new FactoryCopyHandler<WeakReference>(
            (source, _) => new WeakReference(source.Target, source.TrackResurrection)
        ),
        new RefCopyHandler(typeof(UIntPtr)),
        new RefCopyHandler(typeof(IntPtr)),
        new FactoryCopyHandler<CancellationTokenSource>(
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

    private static readonly IDictionary<Type, ICopyHandler> _CopiersByType =
        TypeSupporter.GroupBySupportedType(_Copiers.Concat(ReflectionCopyHandlers.Handlers));

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator);

        if (
            source != null
            && _CopiersByType.TryGetValue(source.GetType(), out ICopyHandler? copier)
        )
        {
            return new(copier.CopySupported(source, duplicator));
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}
