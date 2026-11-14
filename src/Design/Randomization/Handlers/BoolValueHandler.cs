namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <inheritdoc/>
internal sealed class BoolValueHandler : ValueHandler<bool>
{
    /// <inheritdoc/>
    protected override bool Create(IRandom gen)
    {
        return gen.NextBytes(1)[0] % 2 == 1;
    }

    /// <inheritdoc/>
    protected override bool Create(IRandom gen, bool min, bool max)
    {
        return min == max ? min : Create(gen);
    }
}
