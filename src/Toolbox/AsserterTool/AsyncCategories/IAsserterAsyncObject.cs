using Werecodent.CreateAndFake.AsserterTool.Categories;

namespace Werecodent.CreateAndFake.AsserterTool.AsyncCategories;

/// <summary>Handles common object test scenarios.</summary>
public interface IAsserterAsyncObject
{
    /// <inheritdoc cref="IAsserterObject.Is(object,object,string)"/>
    /// <inheritdoc cref="IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task IsAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.Is(object,object,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task IsAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.IsNot(object,object,string)"/>
    /// <inheritdoc cref="IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task IsNotAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.IsNot(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task IsNotAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.ValuesEqual(object,object,string)"/>
    /// <inheritdoc cref="IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task ValuesEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.ValuesEqual(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task ValuesEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.ValuesNotEqual(object,object,string)"/>
    /// <inheritdoc cref="IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task ValuesNotEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.ValuesNotEqual(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task ValuesNotEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.AreUnique(object,object,string)"/>
    /// <inheritdoc cref="IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task AreUniqueAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterObject.AreUnique(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task AreUniqueAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
