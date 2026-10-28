namespace Werecodent.CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common delegate test scenarios.</summary>
public interface IAsserterAction
{
    /// <inheritdoc cref="Throws{T}(Action,AsserterMod,string)"/>
    T Throws<T>(Action? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Action? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsException(Action,AsserterMod,string)"/>
    Exception ThrowsException(Action? behavior, string? details = null);

    /// <inheritdoc cref="IAsserterDelegate.ThrowsException(Delegate,AsserterMod,string)"/>
    Exception ThrowsException(
        Action? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsNo{T}(Action,AsserterMod,string)"/>
    void ThrowsNo<T>(Action? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Action? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsNoException(Action,AsserterMod,string)"/>
    void ThrowsNoException(Action? behavior, string? details = null);

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNoException(Delegate,AsserterMod,string)"/>
    void ThrowsNoException(
        Action? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
