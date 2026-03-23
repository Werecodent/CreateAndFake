using System.Reflection;

namespace CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing TimeOnly values.</summary>
internal sealed class TimeOnlyValueHandler : IValueHandler
{
    /// <summary>TimeOnly <see cref="Type"/> if current .NET version supports it.</summary>
    private static readonly Type? _TimeOnlyType = Assembly
        .Load("System.Runtime")
        .GetType("System.TimeOnly", false);

    /// <summary>Number of ticks in a day.</summary>
    private const long _MaxTicks = TimeSpan.TicksPerDay - 1;

    /// <summary>TimeOnly factory via underlying tick value.</summary>
    private readonly ConstructorInfo _fromTicks;

    /// <summary>Attempts to create a handler if TimeOnly exists in the current .NET version.</summary>
    /// <returns>The created handler if TimeOnly exists, null otherwise.</returns>
    internal static TimeOnlyValueHandler? TryToCreate()
    {
        return (_TimeOnlyType != null) ? new TimeOnlyValueHandler(_TimeOnlyType) : null;
    }

    /// <inheritdoc cref="TimeOnlyValueHandler"/>
    /// <param name="timeOnlyType"><inheritdoc cref="_TimeOnlyType" path="/summary"/></param>
    internal TimeOnlyValueHandler(Type timeOnlyType)
    {
        _fromTicks = timeOnlyType.GetConstructor([typeof(long)])!;
    }

    /// <inheritdoc/>
    public Type? SupportedType => _TimeOnlyType;

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        long ticks = gen.Next(0, _MaxTicks);
        return _fromTicks.Invoke([ticks])!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        long ticks = gen.Next<long>(0, ((dynamic)max).Ticks);
        return _fromTicks.Invoke([ticks]);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        long ticks = gen.Next<long>(((dynamic)min).Ticks, ((dynamic)max).Ticks);
        return _fromTicks.Invoke([ticks]);
    }
}
