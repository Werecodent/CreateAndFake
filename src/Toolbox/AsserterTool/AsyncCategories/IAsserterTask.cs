using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsserterTool.AsyncCategories;

#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles common async test scenarios.</summary>
public interface IAsserterTask
{
    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> ThrowsAsync<T>(Task? behavior, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> ThrowsAsync<T>(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> ThrowsAsync<T>(
        Task<object?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> ThrowsAsync<T>(
        Task<object?>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> ThrowsAsync<T>(
        Func<Task?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> ThrowsAsync<T>(
        Func<Task?>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> ThrowsAsync<T>(
        Func<Task<object?>?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> ThrowsAsync<T>(
        Func<Task<object?>?>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ThrowsNoAsync<T>(Task? behavior, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ThrowsNoAsync<T>(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ThrowsNoAsync<T>(
        Task<object?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ThrowsNoAsync<T>(
        Task<object?>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ThrowsNoAsync<T>(Func<Task?>? behavior, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ThrowsNoAsync<T>(
        Func<Task?>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ThrowsNoAsync<T>(
        Func<Task<object?>?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ThrowsNoAsync<T>(
        Func<Task<object?>?>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;
}

#pragma warning restore CA1068
