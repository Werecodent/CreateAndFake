using System.Globalization;
using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.DuplicatorTool.CopyHints;

/// <summary>Handles cloning common types for <see cref="IDuplicator"/> .</summary>
public sealed class CommonSystemCopyHint : CopyHint
{
    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, DuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator, nameof(duplicator));

        if (source is TimeSpan span)
        {
            return new(new TimeSpan(duplicator.Copy(span.Ticks)));
        }
        else if (source is CultureInfo info)
        {
            return new(
                info.IsReadOnly
                    ? CultureInfo.ReadOnly(new CultureInfo(info.Name, info.UseUserOverride))
                    : new CultureInfo(info.Name, info.UseUserOverride)
            );
        }
        else if (source is Uri link)
        {
            return new(new Uri(link.OriginalString));
        }
        else if (source is Guid guid)
        {
            return new(new Guid(guid.ToByteArray()));
        }
        else if (source is WeakReference reference)
        {
            return new(new WeakReference(reference.Target, reference.TrackResurrection));
        }
        else if (source is Type type)
        {
            return new(type);
        }
        else if (source is MemberInfo member)
        {
            return new(member);
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}
