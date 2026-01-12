using System.Globalization;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning common types for <see cref="IDuplicator"/> .</summary>
public sealed class CommonSystemCopyHint : CopyHint
{
    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator, nameof(duplicator));

        if (source is TimeSpan span)
        {
            return new(new TimeSpan(duplicator.Copy(span.Ticks)));
        }
        else if (source is CultureInfo culture)
        {
            return new(culture.IsReadOnly ? culture : culture.Clone());
        }
        else if (source is DateTimeFormatInfo dateTimeFormat)
        {
            return new(dateTimeFormat.IsReadOnly ? dateTimeFormat : dateTimeFormat.Clone());
        }
        else if (source is NumberFormatInfo numberFormat)
        {
            return new(numberFormat.IsReadOnly ? numberFormat : numberFormat.Clone());
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
        else if (source is MethodBase method)
        {
            return new(method);
        }
        else if (source is ParameterInfo parameter)
        {
            return new(parameter);
        }
        else if (source is IntPtr intPointer)
        {
            return new(new IntPtr((int)intPointer));
        }
        else if (source is CancellationTokenSource canceler)
        {
            CancellationTokenSource result = new();
            if (canceler.IsCancellationRequested)
            {
                result.Cancel();
            }
            return new(result);
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}
