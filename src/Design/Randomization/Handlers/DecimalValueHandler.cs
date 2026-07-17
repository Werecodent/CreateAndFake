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
        while (true)
        {
            decimal percent = NextPercent(gen);
            try
            {
                decimal result = max * percent + min * (1.0m - percent);
                return result.CompareTo(min) > 0 ? result : min;
            }
            catch (OverflowException)
            {
                // Rare edge case due to decimal imprecision.
            }
        }
    }

    /// <inheritdoc cref="IRandom.NextPercent"/>
    private static decimal NextPercent(IRandom gen)
    {
        return new(gen.Next<int>(), gen.Next<int>(), gen.Next(542101086), false, 28);
    }
}
