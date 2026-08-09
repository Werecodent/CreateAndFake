namespace Werecodent.CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common delegate test scenarios.</summary>
public interface IAsserterAction
{
    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,string)"/>
    T Throws<T>(Action? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Action? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,string)"/>
    void ThrowsNo<T>(Action? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Action? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;
}
