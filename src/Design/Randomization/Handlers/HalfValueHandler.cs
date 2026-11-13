using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing Half values.</summary>
[ExcludeFromCreateAndFake]
internal sealed class HalfValueHandler : IValueHandler
{
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

    /// <summary>Handles converting using dynamic with generics.</summary>
    private readonly MethodInfo _maker;

    /// <summary>Represents the smallest possible value of Half.</summary>
    private readonly float _minHalf;

    /// <summary>Represents the largest possible value of Half.</summary>
    private readonly float _maxHalf;

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <inheritdoc cref="HalfValueHandler"/>
    /// <param name="halfType">Half <see cref="Type"/> if current .NET version supports it.</param>
    private HalfValueHandler(Type halfType)
    {
        SupportedType = halfType;

        _minHalf = (float)
            (dynamic)
                halfType
                    .GetProperty("MinValue", BindingFlags.Public | BindingFlags.Static)!
                    .GetValue(null)!;
        _maxHalf = (float)
            (dynamic)
                halfType
                    .GetProperty("MaxValue", BindingFlags.Public | BindingFlags.Static)!
                    .GetValue(null)!;

        _maker = typeof(BFloat16ValueHandler)
            .GetMethod(nameof(CastFromFloat), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(halfType);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        return _maker.Invoke(null, [gen.Next(_minHalf, _maxHalf)])!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return CreateSupported(gen, 0f, max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        return _maker.Invoke(null, [gen.Next((float)(dynamic)min, (float)(dynamic)max)])!;
    }

    /// <summary>Converts <paramref name="value"/> to Half.</summary>
    /// <typeparam name="T"><see cref="SupportedType"/>.</typeparam>
    /// <param name="value">Value to convert.</param>
    /// <returns>The converted value.</returns>
    private static T CastFromFloat<T>(float value)
    {
        return (T)(dynamic)value;
    }
}
