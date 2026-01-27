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
        decimal percent = (decimal)gen.NextPercent();
        decimal result = max * percent + min * (1 - percent);
        return result.CompareTo(min) > 0 ? result : min;
    }
}
