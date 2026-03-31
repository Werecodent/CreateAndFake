namespace CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common async test scenarios.</summary>
public interface IAsserterTask
{
    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(Task? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(Task? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(Task<object?>? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(
        Task<object?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(Func<Task?>? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(
        Func<Task?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(Func<Task<object?>?>? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(
        Func<Task<object?>?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(Task? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(Task? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(Task<object?>? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(
        Task<object?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(Func<Task?>? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(
        Func<Task?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(Func<Task<object?>?>? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(
        Func<Task<object?>?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;
}
