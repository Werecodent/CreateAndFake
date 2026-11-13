using System.Numerics;
using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing UInt128 values.</summary>
[ExcludeFromCreateAndFake]
internal sealed class UInt128ValueHandler : IValueHandler
{
    /// <summary>Attempts to create a handler if UInt128 exists in the current .NET version.</summary>
    /// <returns>The created handler if UInt128 exists, null otherwise.</returns>
    internal static UInt128ValueHandler? TryToCreate()
    {
        try
        {
            return new UInt128ValueHandler(
                Assembly.Load("System.Private.CoreLib").GetType("System.UInt128", true)!
            );
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Handles randomization using dynamic with generics.</summary>
    private readonly MethodInfo _maker;

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <inheritdoc cref="RuneValueHandler"/>
    /// <param name="int128Type">UInt128 <see cref="Type"/> if current .NET version supports it.</param>
    private UInt128ValueHandler(Type int128Type)
    {
        SupportedType = int128Type;

        _maker = typeof(UInt128ValueHandler)
            .GetMethod(nameof(CreateViaBigInteger), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(int128Type);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        return Activator.CreateInstance(SupportedType!, [gen.Next<ulong>(), gen.Next<ulong>()])!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return CreateSupported(gen, 0, max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        return _maker.Invoke(null, [gen, min, max])!;
    }

    /// <inheritdoc cref="CreateSupported(IRandom,object,object)"/>
    private static T CreateViaBigInteger<T>(IRandom gen, object min, object max)
    {
        return (T)(dynamic)gen.Next((BigInteger)(dynamic)min, (BigInteger)(dynamic)max);
    }
}
