using Werecodent.CreateAndFake.AsserterTool.Categories;

namespace Werecodent.CreateAndFake.AsserterTool.AsyncCategories;

#pragma warning disable CA1716 // Matches existing usage.

/// <summary>Handles common object test scenarios.</summary>
public interface IAsserterAsyncObject
{
    /// <inheritdoc cref="IAsserterObject.Is(object,object,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.Is(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.IsNot(object,object,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsNotAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.IsNot(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsNotAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.ValuesEqual(object,object,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ValuesEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.ValuesEqual(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ValuesEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.ValuesNotEqual(object,object,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ValuesNotEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.ValuesNotEqual(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ValuesNotEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.AreUnique(object,object,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task AreUniqueAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.AreUnique(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task AreUniqueAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}

#pragma warning restore
