using System.Collections.Immutable;

namespace CreateAndFake.Design.Randomization.Handlers;

#pragma warning disable CA2263 // Not available in all .NET versions.

/// <summary>Holds a collection of related handlers.</summary>
internal static class TimeValueHandlers
{
    /// <summary>All possible <see cref="DateTimeKind"/>s.</summary>
    private static readonly ImmutableArray<DateTimeKind> _Kinds =
    [
        .. Enum.GetValues(typeof(DateTimeKind)).Cast<DateTimeKind>(),
    ];

    /// <summary>Maximum supported <see cref="DateTimeOffset.Offset"/>.</summary>
    private const long _MaxOffsetTicks = 14 * TimeSpan.TicksPerHour;

    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<IValueHandler> Handlers { get; } =
    [
        new FactoryValueHandler<TimeSpan>(
            TimeSpan.MinValue,
            TimeSpan.MaxValue,
            (gen, min, max) => TimeSpan.FromTicks(gen.Next(min.Ticks, max.Ticks))
        ),
        new FactoryValueHandler<DateTime>(
            DateTime.MinValue,
            DateTime.MaxValue,
            (gen, min, max) => new DateTime(gen.Next(min.Ticks, max.Ticks), gen.NextItem(_Kinds))
        ),
        new FactoryValueHandler<DateTimeOffset>(
            DateTimeOffset.MinValue,
            DateTimeOffset.MaxValue,
            (gen, min, max) =>
            {
                long ticks = gen.Next(min.Ticks - min.Offset.Ticks, max.Ticks - max.Offset.Ticks);

                long offset =
                    gen.Next(
                        -Math.Min(ticks - DateTime.MinValue.Ticks, _MaxOffsetTicks),
                        Math.Min(DateTime.MaxValue.Ticks - ticks, _MaxOffsetTicks)
                    ) / TimeSpan.TicksPerMinute;

                return new(ticks + offset * TimeSpan.TicksPerMinute, TimeSpan.FromMinutes(offset));
            }
        ),
    ];
}

#pragma warning restore CA2263
