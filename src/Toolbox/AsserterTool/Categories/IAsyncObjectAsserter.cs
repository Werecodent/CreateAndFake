namespace CreateAndFake.AsserterTool.Categories;

#pragma warning disable CA1716 // Matches existing usage.
#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles common object test scenarios.</summary>
public interface IAsyncObjectAsserter
{
    /// <inheritdoc cref="IObjectAsserter.Is(object,object,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.Is(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.IsNot(object,object,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsNotAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.IsNot(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsNotAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesEqual(object,object,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ValuesEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesEqual(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ValuesEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesNotEqual(object,object,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ValuesNotEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesNotEqual(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ValuesNotEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.AreUnique(object,object,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task AreUniqueAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.AreUnique(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task AreUniqueAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}

#pragma warning restore CA1716, CA1068
