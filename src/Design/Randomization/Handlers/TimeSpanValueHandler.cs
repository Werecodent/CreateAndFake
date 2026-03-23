namespace CreateAndFake.Design.Randomization.Handlers;

/// <inheritdoc/>
internal sealed class TimeSpanValueHandler : ValueHandler<TimeSpan>
{
    /// <inheritdoc/>
    protected override TimeSpan Create(IRandom gen)
    {
        return Create(gen, TimeSpan.MinValue, TimeSpan.MaxValue);
    }

    /// <inheritdoc/>
    protected override TimeSpan Create(IRandom gen, TimeSpan min, TimeSpan max)
    {
        return TimeSpan.FromTicks(gen.Next(min.Ticks, max.Ticks));
    }
}
