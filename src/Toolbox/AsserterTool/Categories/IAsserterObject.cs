using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;

namespace Werecodent.CreateAndFake.AsserterTool.Categories;

#pragma warning disable CA1716 // Matches existing usage.

/// <summary>Handles common object test scenarios.</summary>
public interface IAsserterObject
{
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void Is(object? expected, object? actual, string? details = null);

    /// <summary>Verifies <c>actual</c> equals <paramref name="expected"/> by value.</summary>
    /// <param name="expected">Instance to compare against.</param>
    /// <param name="actual">Instance to test.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
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
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void IsNull(object? actual, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="IsNot(object,object,AsserterMod,string)"/>
    void IsNot(object? expected, object? actual, string? details = null);

    /// <summary>Verifies <c>actual</c> does not equal <paramref name="expected"/> by value.</summary>
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void IsNot(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IsNotNull(object,AsserterMod,string)"/>
    void IsNotNull([NotNull] object? actual, string? details = null);

    /// <summary>Verifies <c>actual</c> does not equal <see langword="null"/>.</summary>
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void IsNotNull(
        [NotNull] object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

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

    /// <summary>Verifies <c>actual</c> does not equal <paramref name="expected"/> by reference.</summary>
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

    /// <inheritdoc cref="Inherits{T}(object,AsserterMod,string)"/>
    void Inherits<TParent>(object? instance, string? details = null);

    /// <summary>
    ///     Verifies that <typeparamref name="TParent"/> is a base <see langword="class"/>
    ///     or <see langword="interface"/> for the <c>instance</c>.
    /// </summary>
    /// <typeparam name="TParent">
    ///     Expected base <see langword="class"/>/<see langword="interface"/> for the <c>instance</c>.
    /// </typeparam>
    /// <param name="instance">Instance to test.</param>
    /// <inheritdoc cref="Is(object,object,AsserterMod,string)"/>
    void Inherits<TParent>(
        object? instance,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="Inherits(Type,object,AsserterMod,string)"/>
    void Inherits(Type? parent, object? instance, string? details = null);

    /// <summary>
    ///     Verifies that the <paramref name="parent"/> is a base <see langword="class"/>
    ///     or <see langword="interface"/> for the <c>instance</c>.
    /// </summary>
    /// <param name="parent">
    ///     Expected base <see langword="class"/>/<see langword="interface"/> for the <c>instance</c>.
    /// </param>
    /// <inheritdoc cref="Inherits{T}(object,AsserterMod,string)"/>
    void Inherits(
        Type? parent,
        object? instance,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="InheritedBy{T}(object,AsserterMod,string)"/>
    void InheritedBy<TChild>(object? instance, string? details = null);

    /// <summary>
    ///     Verifies that the <c>instance</c> <see cref="Type"/> is a base <see langword="class"/>
    ///     or <see langword="interface"/> for <typeparamref name="TChild"/>.
    /// </summary>
    /// <typeparam name="TChild">
    ///     Expected child <see cref="Type"/> inheriting the given <c>instance</c> <see cref="Type"/>.
    /// </typeparam>
    /// <inheritdoc cref="Inherits{T}(object,AsserterMod,string)"/>
    void InheritedBy<TChild>(
        object? instance,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="InheritedBy(Type,object,AsserterMod,string)"/>
    void InheritedBy(Type? child, object? instance, string? details = null);

    /// <summary>
    ///     Verifies that the <c>instance</c> <see cref="Type"/> is a base <see langword="class"/>
    ///     or <see langword="interface"/> for the <paramref name="child"/>.
    /// </summary>
    /// <param name="child">
    ///     Expected child <see cref="Type"/> potentially inheriting the given <c>instance</c> <see cref="Type"/>.
    /// </param>
    /// <inheritdoc cref="InheritedBy{T}(object,AsserterMod,string)"/>
    void InheritedBy(
        Type? child,
        object? instance,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="Called(object,Times,AsserterMod)"/>
    void Called(object? fake, AsserterMod? optionConfiguration = null);

    /// <summary>Verifies all behaviors with associated <see cref="Times"/> were called as expected.</summary>
    /// <param name="fake"><see cref="IFaked"/> instance with behavior set.</param>
    /// <param name="total">Expected total number of calls across all methods to test as well.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <remarks>
    ///     For use on <see cref="IFaked"/> stubs from the <see cref="Faker"/> tool only.
    ///     When specifying <paramref name="total"/>, be aware of test framework calls for info/display.
    /// </remarks>
    /// <exception cref="ToolException">If the <c>instance</c> is not an <see cref="IFaked"/> instance.</exception>
    /// <exception cref="FakeVerifyException">For any behavior verification failures.</exception>
    void Called(object? fake, Times total, AsserterMod? optionConfiguration = null);

    /// <inheritdoc cref="Fail(object,AsserterMod,string)"/>
    [DoesNotReturn]
    void Fail(object? content, string? details = null);

    /// <param name="content">Content responsible for the failure.</param>
    /// <inheritdoc cref="IAsserter.Fail(AsserterMod,string)"/>
    [DoesNotReturn]
    void Fail(object? content, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="Debug(object,AsserterMod,string)"/>
    void Debug(object? content, string? details = null);

    /// <param name="content">Content to display information for.</param>
    /// <inheritdoc cref="IAsserter.Debug(AsserterMod,string)"/>
    void Debug(object? content, AsserterMod? optionConfiguration, string? details = null);
}

#pragma warning restore
