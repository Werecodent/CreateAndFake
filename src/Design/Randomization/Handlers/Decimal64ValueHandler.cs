using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing Decimal64 values.</summary>
[ExcludeFromCreateAndFake]
internal sealed class Decimal64ValueHandler : IValueHandler
{
    /// <summary>Attempts to create a handler if Decimal64 exists in the current .NET version.</summary>
    /// <returns>The created handler if Decimal64 exists, null otherwise.</returns>
    internal static Decimal64ValueHandler? TryToCreate()
    {
        try
        {
            return new Decimal64ValueHandler(
                Assembly.Load("System.Private.CoreLib").GetType("System.Numerics.Decimal64", true)!
            );
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Handles randomization using dynamic with generics.</summary>
    private readonly MethodInfo _maker;

    /// <summary>Determines if a Decimal64 is -∞.</summary>
    private readonly MethodInfo _isNegativeInfinity;

    /// <summary>Determines if a Decimal64 is ∞.</summary>
    private readonly MethodInfo _isPositiveInfinity;

    /// <summary>Represents <c>0</c> for Decimal64.</summary>
    private readonly object _zero;

    /// <summary>Represents the smallest possible value of Decimal64.</summary>
    private readonly object _minDecimal64;

    /// <summary>Represents the largest possible value of Decimal64.</summary>
    private readonly object _maxDecimal64;

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <inheritdoc cref="RuneValueHandler"/>
    /// <param name="decimal32Type">Decimal64 <see cref="Type"/> if current .NET version supports it.</param>
    private Decimal64ValueHandler(Type decimal32Type)
    {
        SupportedType = decimal32Type;

        _zero = Activator.CreateInstance(decimal32Type)!;
        _minDecimal64 = decimal32Type
            .GetProperty("MinValue", BindingFlags.Public | BindingFlags.Static)!
            .GetValue(null)!;
        _maxDecimal64 = decimal32Type
            .GetProperty("MaxValue", BindingFlags.Public | BindingFlags.Static)!
            .GetValue(null)!;

        _isNegativeInfinity = decimal32Type.GetMethod(
            "IsNegativeInfinity",
            BindingFlags.Public | BindingFlags.Static
        )!;
        _isPositiveInfinity = decimal32Type.GetMethod(
            "IsPositiveInfinity",
            BindingFlags.Public | BindingFlags.Static
        )!;
        _maker = typeof(Decimal64ValueHandler)
            .GetMethod(nameof(Create), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(decimal32Type);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        return CreateSupported(gen, _minDecimal64, _maxDecimal64);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return CreateSupported(gen, _zero, max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        return _maker.Invoke(this, [gen, min, max])!;
    }

    /// <inheritdoc cref="CreateSupported(IRandom,object,object)"/>
    private T Create<T>(IRandom gen, dynamic min, dynamic max)
        where T : IComparable
    {
        dynamic usefulMin = (bool)_isNegativeInfinity.Invoke(null, [min])! ? _minDecimal64 : min;
        dynamic usefulMax = (bool)_isPositiveInfinity.Invoke(null, [max])! ? _maxDecimal64 : max;

        dynamic percent = gen.NextPercent();
        T result = usefulMax * (T)percent + usefulMin * (T)(1d - percent);
        return result.CompareTo(min) > 0 ? result : min;
    }
}
