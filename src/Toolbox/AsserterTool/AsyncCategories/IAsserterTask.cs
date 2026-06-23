using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsserterTool.AsyncCategories;

/// <summary>Handles common async test scenarios.</summary>
public interface IAsserterTask
{
    /// <inheritdoc cref="IAsserterDelegate.HasResult{T}(Delegate,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> HasResultAsync<T>(
        Task<T>? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterDelegate.HasResult{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> HasResultAsync<T>(
        Task<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

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
