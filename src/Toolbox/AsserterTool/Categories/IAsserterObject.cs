using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.AsserterTool.Categories;

#pragma warning disable CA1716 // Matches existing usage.

/// <summary>Handles common object test scenarios.</summary>
public interface IAsserterObject
{
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void Is(object? expected, object? actual, string? details = null);

    /// <summary>Verifies <c>actual</c> equals <paramref name="expected"/> by value.</summary>
    /// <param name="expected">Instance to compare against.</param>
    /// <param name="actual">Instance to run assertion checks with.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <param name="details">Description to include in assertion failure messages.</param>
    /// <exception cref="AssertException">If the comparison fails to match the expected behavior.</exception>
    void Is(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IsNull(object,AsserterMod,string)"/>
    void IsNull(object? actual, string? details = null);

    /// <summary>Verifies <c>actual</c> equals <see langword="null"/>.</summary>
    /// <param name="actual">Instance to run assertion checks with.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <param name="details">Description to include in assertion failure messages.</param>
    /// <exception cref="AssertException">If the comparison fails to match the expected behavior.</exception>
    void IsNull(object? actual, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="IsNot(object,object,AsserterMod,string)"/>
    void IsNot(object? expected, object? actual, string? details = null);

    /// <summary>Verifies <c>actual</c> unequals <paramref name="expected"/> by value.</summary>
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void IsNot(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IsNotNull(object,AsserterMod,string)"/>
    void IsNotNull(object? actual, string? details = null);

    /// <summary>Verifies <c>actual</c> unequals <see langword="null"/>.</summary>
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void IsNotNull(object? actual, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="ReferenceEqual(object,object,AsserterMod,string)"/>
    void ReferenceEqual(object? expected, object? actual, string? details = null);

    /// <summary>Verifies <c>actual</c> equals <paramref name="expected"/> by reference.</summary>
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void ReferenceEqual(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ReferenceNotEqual(object,object,AsserterMod,string)"/>
    void ReferenceNotEqual(object? expected, object? actual, string? details = null);

    /// <summary>Verifies <c>actual</c> unequals <paramref name="expected"/> by reference.</summary>
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void ReferenceNotEqual(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ValuesEqual(object,object,AsserterMod,string)"/>
    void ValuesEqual(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void ValuesEqual(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ValuesNotEqual(object,object,AsserterMod,string)"/>
    void ValuesNotEqual(object? expected, object? actual, string? details = null);

    /// <inheritdoc cref="IsNot(object,object,AsserterMod,string)"/>
    void ValuesNotEqual(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="AreUnique(object,object,AsserterMod,string)"/>
    void AreUnique(object? expected, object? actual, string? details = null);

    /// <summary>Verifies <c>actual</c> shares no data with <paramref name="expected"/>.</summary>
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void AreUnique(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="Called(object,Times,AsserterMod)"/>
    void Called(object? fake, AsserterMod? optionConfiguration = null);

    /// <summary>Verifies all behaviors with associated times were called as expected.</summary>
    /// <param name="fake">Fake instance with behavior set.</param>
    /// <param name="total">Expected total number of calls to test as well.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <remarks>
    ///     For use on <see cref="IFaked"/> stubs from the <see cref="Faker"/> tool only.
    ///     When specifying <paramref name="total"/>, be aware of test framework calls for info/display.
    /// </remarks>
    void Called(object? fake, Times total, AsserterMod? optionConfiguration = null);
}

#pragma warning restore CA1716
