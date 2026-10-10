using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing DateOnly values.</summary>
[ExcludeFromCreateAndFake]
internal sealed class DateOnlyValueHandler : IValueHandler
{
    /// <summary>Number of days from 1/1/0001 to 12/31/9999.</summary>
    private const int _MaxDayNumber = 3652058;

    /// <summary>DateOnly factory using day numbers.</summary>
    private readonly MethodInfo _createFromDayNumber;

    /// <summary>Attempts to create a handler if DateOnly exists in the current .NET version.</summary>
    /// <returns>The created handler if DateOnly exists, null otherwise.</returns>
    internal static DateOnlyValueHandler? TryToCreate()
    {
        try
        {
            return new DateOnlyValueHandler(
                Assembly.Load("System.Runtime").GetType("System.DateOnly", true)!
            );
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <inheritdoc cref="DateOnlyValueHandler"/>
    /// <param name="dateOnlyType">DateOnly <see cref="Type"/> if current .NET version supports it.</param>
    private DateOnlyValueHandler(Type dateOnlyType)
    {
        SupportedType = dateOnlyType;

        _createFromDayNumber = dateOnlyType.GetMethod("FromDayNumber")!;
    }

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
