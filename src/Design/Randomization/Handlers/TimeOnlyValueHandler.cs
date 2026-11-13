using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing TimeOnly values.</summary>
[ExcludeFromCreateAndFake]
internal sealed class TimeOnlyValueHandler : IValueHandler
{
    /// <summary>Number of ticks in a day.</summary>
    private const long _MaxTicks = TimeSpan.TicksPerDay - 1;

    /// <summary>Attempts to create a handler if TimeOnly exists in the current .NET version.</summary>
    /// <returns>The created handler if TimeOnly exists, null otherwise.</returns>
    internal static TimeOnlyValueHandler? TryToCreate()
    {
        try
        {
            return new TimeOnlyValueHandler(
                Assembly.Load("System.Runtime").GetType("System.TimeOnly", true)!
            );
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <inheritdoc cref="TimeOnlyValueHandler"/>
    /// <param name="timeOnlyType">TimeOnly <see cref="Type"/> if current .NET version supports it.</param>
    private TimeOnlyValueHandler(Type timeOnlyType)
    {
        SupportedType = timeOnlyType;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        long ticks = gen.Next(0, _MaxTicks);
        return Activator.CreateInstance(SupportedType!, [ticks])!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        long ticks = gen.Next<long>(0, ((dynamic)max).Ticks);
        return Activator.CreateInstance(SupportedType!, [ticks])!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        long ticks = gen.Next<long>(((dynamic)min).Ticks, ((dynamic)max).Ticks);
        return Activator.CreateInstance(SupportedType!, [ticks])!;
    }
}
