namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <inheritdoc/>
internal sealed class DateTimeOffsetValueHandler : ValueHandler<DateTimeOffset>
{
    /// <summary>Maximum supported <see cref="DateTimeOffset.Offset"/>.</summary>
    private const long _MaxOffsetTicks = 14 * TimeSpan.TicksPerHour;

    /// <inheritdoc/>
    protected override DateTimeOffset Create(IRandom gen)
    {
        return Create(gen, DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
    }

    /// <inheritdoc/>
    protected override DateTimeOffset Create(IRandom gen, DateTimeOffset min, DateTimeOffset max)
    {
        long ticks = gen.Next(min.Ticks - min.Offset.Ticks, max.Ticks - max.Offset.Ticks);

        long offset =
            gen.Next(
                -Math.Min(ticks - DateTime.MinValue.Ticks, _MaxOffsetTicks),
                Math.Min(DateTime.MaxValue.Ticks - ticks, _MaxOffsetTicks)
            ) / TimeSpan.TicksPerMinute;

        return new(ticks + offset * TimeSpan.TicksPerMinute, TimeSpan.FromMinutes(offset));
    }
}
