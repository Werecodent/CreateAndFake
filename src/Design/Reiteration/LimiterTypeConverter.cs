using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CreateAndFake.Design.Reiteration;

/// <summary>Handles conversion for <see cref="ILimiter"/>.</summary>
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
                $"Cannot convert {nameof(Limiter)} from type: {value?.GetType()}",
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
        return ((Limiter?)value)?.ToString();
    }
}
