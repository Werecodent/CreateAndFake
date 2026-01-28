namespace CreateAndFake.Design.Randomization.Handlers;

/// <inheritdoc/>
internal sealed class DateTimeValueHandler : ValueHandler<DateTime>
{
    /// <inheritdoc/>
    protected override DateTime Create(IRandom gen)
    {
        return Create(gen, DateTime.MinValue, DateTime.MaxValue);
    }

    /// <inheritdoc/>
    protected override DateTime Create(IRandom gen, DateTime min, DateTime max)
    {
        return new DateTime(
            gen.Next(min.ToUniversalTime().Ticks, max.ToUniversalTime().Ticks),
            DateTimeKind.Utc
        );
    }
}
