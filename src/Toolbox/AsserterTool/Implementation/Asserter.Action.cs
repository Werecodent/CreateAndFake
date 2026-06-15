using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterAction
{
    /// <inheritdoc/>
    public virtual T Throws<T>(Action? behavior, string? details = null)
        where T : Exception
    {
        return Throws<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(
        Action? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        return Throws<T>((Delegate?)behavior, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(Action? behavior, string? details = null)
        where T : Exception
    {
        ThrowsNo<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(
        Action? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        ThrowsNo<T>((Delegate?)behavior, optionConfiguration, details);
    }
}
