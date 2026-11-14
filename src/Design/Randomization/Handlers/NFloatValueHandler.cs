using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing NFloat values.</summary>
[ExcludeFromCreateAndFake]
internal sealed class NFloatValueHandler : IValueHandler
{
    /// <summary>Attempts to create a handler if NFloat exists in the current .NET version.</summary>
    /// <param name="size"><inheritdoc cref="_size" path="/summary"/></param>
    /// <returns>The created handler if NFloat exists, null otherwise.</returns>
    internal static NFloatValueHandler? TryToCreate(int? size = null)
    {
        try
        {
            return new NFloatValueHandler(
                Assembly
                    .Load("System.Private.CoreLib")
                    .GetType("System.Runtime.InteropServices.NFloat", true)!,
                size
            );
        }
        catch
        {
            return null;
        }
    }

    /// <summary>The size of a NFloat in the executing process, measured in bytes.</summary>
    private readonly int _size;

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <inheritdoc cref="RuneValueHandler"/>
    /// <param name="nFloatType">NFloat <see cref="Type"/> if current .NET version supports it.</param>
    private NFloatValueHandler(Type nFloatType, int? size)
    {
        SupportedType = nFloatType;

        _size =
            size
            ?? (int)
                nFloatType
                    .GetProperty("Size", BindingFlags.Public | BindingFlags.Static)!
                    .GetValue(null)!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        if (_size == 4)
        {
            return Activator.CreateInstance(SupportedType!, [gen.Next<float>()])!;
        }
        else
        {
            return Activator.CreateInstance(SupportedType!, [gen.Next<double>()])!;
        }
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return CreateSupported(gen, 0f, max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        if (_size == 4)
        {
            return Activator.CreateInstance(
                SupportedType!,
                [gen.Next(typeof(float), (float)(dynamic)min, (float)(dynamic)max)]
            )!;
        }
        else
        {
            return Activator.CreateInstance(
                SupportedType!,
                [gen.Next(typeof(double), (double)(dynamic)min, (double)(dynamic)max)]
            )!;
        }
    }
}
