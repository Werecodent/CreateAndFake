using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsyncAsserterTool.Categories;

#pragma warning disable CA1716 // Matches existing usage.

/// <summary>Handles common object test scenarios.</summary>
public interface IAsyncObjectAsserter
{
    /// <inheritdoc cref="IObjectAsserter.Is(object,object,string)"/>
    Task Is(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.Is(object,object,AsserterMod,string)"/>
    Task Is(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.IsNot(object,object,string)"/>
    Task IsNot(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.IsNot(object,object,AsserterMod,string)"/>
    Task IsNot(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesEqual(object,object,string)"/>
    Task ValuesEqual(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.ValuesEqual(object,object,AsserterMod,string)"/>
    Task ValuesEqual(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesNotEqual(object,object,string)"/>
    Task ValuesNotEqual(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.ValuesNotEqual(object,object,AsserterMod,string)"/>
    Task ValuesNotEqual(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.AreUnique(object,object,string)"/>
    Task AreUnique(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.AreUnique(object,object,AsserterMod,string)"/>
    Task AreUnique(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );
}

#pragma warning restore CA1716
