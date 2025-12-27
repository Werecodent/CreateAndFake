namespace CreateAndFake.AsserterTool.Categories;

#pragma warning disable CA1716 // Matches existing usage.

/// <summary>Handles common object test scenarios.</summary>
public interface IAsyncObjectAsserter
{
    /// <inheritdoc cref="IObjectAsserter.Is(object,object,string)"/>
    Task IsAsync(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.Is(object,object,AsserterMod,string)"/>
    Task IsAsync(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.IsNot(object,object,string)"/>
    Task IsNotAsync(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.IsNot(object,object,AsserterMod,string)"/>
    Task IsNotAsync(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesEqual(object,object,string)"/>
    Task ValuesEqualAsync(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.ValuesEqual(object,object,AsserterMod,string)"/>
    Task ValuesEqualAsync(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesNotEqual(object,object,string)"/>
    Task ValuesNotEqualAsync(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.ValuesNotEqual(object,object,AsserterMod,string)"/>
    Task ValuesNotEqualAsync(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.AreUnique(object,object,string)"/>
    Task AreUniqueAsync(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.AreUnique(object,object,AsserterMod,string)"/>
    Task AreUniqueAsync(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}

#pragma warning restore CA1716
