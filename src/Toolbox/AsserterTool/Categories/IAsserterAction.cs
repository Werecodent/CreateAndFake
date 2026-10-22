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

    /// <inheritdoc cref="IAsserterDelegate.ThrowsException(Delegate,string)"/>
    Exception ThrowsException(Action? behavior, string? details = null);

    /// <inheritdoc cref="IAsserterDelegate.ThrowsException(Delegate,AsserterMod,string)"/>
    Exception ThrowsException(
        Action? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,string)"/>
    void ThrowsNo<T>(Action? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Action? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNoException(Delegate,string)"/>
    void ThrowsNoException(Action? behavior, string? details = null);

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNoException(Delegate,AsserterMod,string)"/>
    void ThrowsNoException(
        Action? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
