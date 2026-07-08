using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Reiteration;

/// <summary>Handles serialization/deserialization for <see cref="ILimiter"/>.</summary>
public sealed class LimiterTypeConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(int) || sourceType == typeof(string);
    }

    /// <inheritdoc/>
    public override object? ConvertFrom(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object value
    )
    {
        if (value is int tries)
        {
            return new Limiter(tries);
        }
        else if (value is string data)
        {
            return Limiter.ConvertFrom(data, culture);
        }
        else
        {
            throw new ArgumentException(
                $"Cannot convert {nameof(Limiter)} from type: '{GenericConverter.ExpandName(value)}'",
                nameof(value)
            );
        }
    }

    /// <inheritdoc/>
    public override bool CanConvertTo(
        ITypeDescriptorContext? context,
        [NotNullWhen(true)] Type? destinationType
    )
    {
        return destinationType == typeof(string);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object? value,
        Type destinationType
    )
    {
        if (value is Limiter limiter)
        {
            return limiter.ToString();
        }
        else
        {
            throw new ArgumentException(
                $"Can only convert {nameof(Limiter)}s, not type: '{GenericConverter.ExpandName(value)}'",
                nameof(value)
            );
        }
    }
}
