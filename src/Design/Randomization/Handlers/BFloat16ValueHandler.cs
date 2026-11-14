using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing BFloat16 values.</summary>
[ExcludeFromCreateAndFake]
internal sealed class BFloat16ValueHandler : IValueHandler
{
    /// <summary>Attempts to create a handler if BFloat16 exists in the current .NET version.</summary>
    /// <returns>The created handler if BFloat16 exists, null otherwise.</returns>
    internal static BFloat16ValueHandler? TryToCreate()
    {
        try
        {
            return new BFloat16ValueHandler(
                Assembly.Load("System.Private.CoreLib").GetType("System.Numerics.BFloat16", true)!
            );
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Handles converting using dynamic with generics.</summary>
    private readonly MethodInfo _maker;

    /// <summary>Represents the smallest possible value of BFloat16.</summary>
    private readonly float _minBFloat;

    /// <summary>Represents the largest possible value of BFloat16.</summary>
    private readonly float _maxBFloat;

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <inheritdoc cref="RuneValueHandler"/>
    /// <param name="bFloat16Type">BFloat16 <see cref="Type"/> if current .NET version supports it.</param>
    private BFloat16ValueHandler(Type bFloat16Type)
    {
        SupportedType = bFloat16Type;

        _minBFloat = (float)
            (dynamic)
                bFloat16Type
                    .GetProperty("MinValue", BindingFlags.Public | BindingFlags.Static)!
                    .GetValue(null)!;
        _maxBFloat = (float)
            (dynamic)
                bFloat16Type
                    .GetProperty("MaxValue", BindingFlags.Public | BindingFlags.Static)!
                    .GetValue(null)!;

        _maker = typeof(BFloat16ValueHandler)
            .GetMethod(nameof(CastFromFloat), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(bFloat16Type);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        return _maker.Invoke(null, [gen.Next(_minBFloat, _maxBFloat)])!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return CreateSupported(gen, 0, max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        return _maker.Invoke(null, [gen.Next((float)(dynamic)min, (float)(dynamic)max)])!;
    }

    /// <summary>Converts <paramref name="value"/> to BFloat16.</summary>
    /// <typeparam name="T"><see cref="SupportedType"/>.</typeparam>
    /// <param name="value">Value to convert.</param>
    /// <returns>The converted value.</returns>
    private static T CastFromFloat<T>(float value)
    {
        return (T)(dynamic)value;
    }
}
