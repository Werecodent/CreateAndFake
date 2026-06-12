using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterFunc
{
    /// <inheritdoc/>
    public virtual T HasResult<T>(Func<T>? behavior, string? details = null)
    {
        return HasResult(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual T HasResult<T>(
        Func<T>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return HasResult<T>((Delegate?)behavior, optionConfiguration, details);
    }
}
