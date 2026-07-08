using System.Reflection;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Randomization.Handlers;

#pragma warning disable CA2263 // Not available in all .NET versions.

/// <summary>Handles randomizing Rune values.</summary>
[ExcludeFromCreateAndFake]
internal sealed class RuneValueHandler : IValueHandler
{
    /// <summary>Rune factory using underlying <see langword="int"/> value.</summary>
    private readonly ConstructorInfo _fromValue;

    /// <summary>Determines if an <see langword="int"/> can be used to successfully create a Rune.</summary>
    private readonly Func<int, bool> _isValidRuneValue;

    /// <summary>Attempts to create a handler if Rune exists in the current .NET version.</summary>
    /// <returns>The created handler if Rune exists, null otherwise.</returns>
    internal static RuneValueHandler? TryToCreate()
    {
        try
        {
            return new RuneValueHandler(
                Assembly.Load("System.Runtime").GetType("System.Text.Rune", true)!
            );
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <inheritdoc cref="RuneValueHandler"/>
    /// <param name="runeType">Rune <see cref="Type"/> if current .NET version supports it.</param>
    private RuneValueHandler(Type runeType)
    {
        SupportedType = runeType;

        _fromValue = runeType.GetConstructor([typeof(int)])!;
        _isValidRuneValue =
            (Func<int, bool>)
                runeType
                    .GetMethod("IsValid", [typeof(int)])!
                    .CreateDelegate(typeof(Func<int, bool>));
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        int runeValue = NextRuneValue(gen, 0x0000, 0x10FFFF);
        return _fromValue.Invoke([runeValue]);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        int runeValue = NextRuneValue(gen, 0x0000, ((dynamic)max).Value);
        return _fromValue.Invoke([runeValue]);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        int runeValue = NextRuneValue(gen, ((dynamic)min).Value, ((dynamic)max).Value);
        return _fromValue.Invoke([runeValue]);
    }

    /// <summary>Generates a constrained value that can be used to successfully create a Rune.</summary>
    /// <inheritdoc cref="CreateSupported(IRandom,object,object)"/>
    private int NextRuneValue(IRandom gen, int min, int max)
    {
        int result;
        do
        {
            result = gen.Next(min, max);
        } while (!_isValidRuneValue(result));

        return result;
    }
}

#pragma warning restore CA2263
