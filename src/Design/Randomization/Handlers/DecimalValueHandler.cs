namespace CreateAndFake.Design.Randomization.Handlers;

/// <inheritdoc/>
internal sealed class DecimalValueHandler : ValueHandler<decimal>
{
    /// <inheritdoc/>
    protected override decimal Create(IRandom gen)
    {
        return new decimal(
            gen.Next<int>(),
            gen.Next<int>(),
            gen.Next<int>(),
            gen.Next<bool>(),
            gen.Next<byte>(29)
        );
    }

    /// <inheritdoc/>
    protected override decimal Create(IRandom gen, decimal min, decimal max)
    {
        decimal percent = NextPercent(gen);
        decimal result = max * percent + min * (1m - percent);
        return result.CompareTo(min) > 0 ? result : min;
    }

    /// <inheritdoc cref="IRandom.NextPercent"/>
    private static decimal NextPercent(IRandom gen)
    {
        decimal percent;
        do
        {
            percent = new(gen.Next<int>(), gen.Next<int>(), gen.Next(542101085), false, 28);
        } while (percent >= 1m);

        return percent;
    }
}
