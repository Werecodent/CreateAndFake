using System.Globalization;
using System.Reflection;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing Half values.</summary>
[ExcludeFromCreateAndFake]
internal sealed class HalfValueHandler : IValueHandler
{
    private const float _MinHalf = -65504;

    private const float _MaxHalf = 65504;

    /// <summary>Half factory using underlying <see langword="string"/> value.</summary>
    private readonly MethodInfo _fromString;

    /// <summary>Attempts to create a handler if Half exists in the current .NET version.</summary>
    /// <returns>The created handler if Half exists, null otherwise.</returns>
    internal static HalfValueHandler? TryToCreate()
    {
        try
        {
            return new HalfValueHandler(
                Assembly.Load("System.Runtime").GetType("System.Half", true)!
            );
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <inheritdoc cref="HalfValueHandler"/>
    /// <param name="halfType">Half <see cref="Type"/> if current .NET version supports it.</param>
    private HalfValueHandler(Type halfType)
    {
        SupportedType = halfType;

        _fromString = halfType.GetMethod("Parse", [typeof(string)])!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        return ToHalf(gen.Next(_MinHalf, _MaxHalf));
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return ToHalf(gen.Next(ToFloat(max)));
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        return ToHalf(gen.Next(ToFloat(min), ToFloat(max)));
    }

    private object ToHalf(float value)
    {
        return _fromString.Invoke(null, [value.ToString(CultureInfo.InvariantCulture)])!;
    }

    private static float ToFloat(object value)
    {
        return float.Parse(value.ToString()!, CultureInfo.InvariantCulture);
    }
}
