using System.Reflection;

namespace CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing DateOnly values.</summary>
internal sealed class DateOnlyValueHandler : IValueHandler
{
    /// <summary>DateOnly <see cref="Type"/> if current .NET version supports it.</summary>
    private static readonly Type? _DateOnlyType = Assembly
        .Load("System.Runtime")
        .GetType("System.DateOnly", false);

    /// <summary>Number of days from 1/1/0001 to 12/31/9999.</summary>
    private const int _MaxDayNumber = 3652058;

    /// <summary>DateOnly factory using day numbers.</summary>
    private readonly MethodInfo _createFromDayNumber;

    /// <summary>Attempts to create a handler if DateOnly exists in the current .NET version.</summary>
    /// <returns>The created handler if DateOnly exists, null otherwise.</returns>
    internal static DateOnlyValueHandler? TryToCreate()
    {
        return (_DateOnlyType != null) ? new DateOnlyValueHandler(_DateOnlyType) : null;
    }

    /// <inheritdoc cref="DateOnlyValueHandler"/>
    /// <param name="dateOnlyType"><inheritdoc cref="_DateOnlyType" path="/summary"/></param>
    internal DateOnlyValueHandler(Type dateOnlyType)
    {
        _createFromDayNumber = dateOnlyType.GetMethod("FromDayNumber")!;
    }

    /// <inheritdoc/>
    public Type? SupportedType => _DateOnlyType;

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        int dayNumber = gen.Next(0, _MaxDayNumber);
        return _createFromDayNumber.Invoke(null, [dayNumber])!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        int dayNumber = gen.Next<int>(0, ((dynamic)max).DayNumber);
        return _createFromDayNumber.Invoke(null, [dayNumber])!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        int dayNumber = gen.Next<int>(((dynamic)min).DayNumber, ((dynamic)max).DayNumber);
        return _createFromDayNumber.Invoke(null, [dayNumber])!;
    }
}
