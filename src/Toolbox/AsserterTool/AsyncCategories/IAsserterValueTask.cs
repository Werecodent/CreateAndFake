namespace CreateAndFake.AsserterTool.AsyncCategories;

#pragma warning disable CS1591

/// <summary>Handles common async test scenarios.</summary>
public interface IAsserterValueTask
{
    Task<T> HasResultAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        string? details = null
    );

    Task<T> HasResultAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    Task<T> ThrowsAsync<T>(ValueTask? operation, CancellationToken canceler, string? details = null)
        where T : Exception;

    Task<T> ThrowsAsync<T>(
        ValueTask? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    Task<TException> ThrowsAsync<TException, TContent>(
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception;

    Task<TException> ThrowsAsync<TException, TContent>(
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception;

    Task<Exception> ThrowsExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        string? details = null
    );

    Task<Exception> ThrowsExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    Task ThrowsNoAsync<T>(ValueTask? operation, CancellationToken canceler, string? details = null)
        where T : Exception;

    Task ThrowsNoAsync<T>(
        ValueTask? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    Task ThrowsNoAsync<TException, TContent>(
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception;

    Task ThrowsNoAsync<TException, TContent>(
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception;

    Task ThrowsNoExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        string? details = null
    );

    Task ThrowsNoExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}

#pragma warning restore
