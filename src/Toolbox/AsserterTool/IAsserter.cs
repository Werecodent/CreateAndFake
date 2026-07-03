global using AsserterMod = System.Func<
    CreateAndFake.AsserterTool.AsserterOptions,
    CreateAndFake.AsserterTool.AsserterOptions
>;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.AsserterTool.AsyncCategories;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.AsserterTool;

/// <summary>Handles common test scenarios.</summary>
public interface IAsserter
    : ITool<AsserterOptions>,
        IAsserterAsyncEnumerable,
        IAsserterAsyncObject,
        IAsserterTask,
        IAsserterValueTask,
        IAsserterAction,
        IAsserterComparable,
        IAsserterDelegate,
        IAsserterEnumerable,
        IAsserterFunc,
        IAsserterObject,
        IAsserterString,
        IAsserterType
{
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of Options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IAsserter WithOptions(AsserterMod optionConfiguration);

    /// <inheritdoc cref="Pass(AsserterMod)"/>
    void Pass();

    /// <summary>Specifies the test is successful if it reaches this point.</summary>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    void Pass(AsserterMod? optionConfiguration);

    /// <inheritdoc cref="Fail(AsserterMod,string)"/>
    [DoesNotReturn]
    void Fail(string? details = null);

    /// <summary>Specifies the test has failed if it reaches this point.</summary>
    /// <param name="details">Description to include in assertion failure messages.</param>
    /// <exception cref="AssertException">Always.</exception>
    /// <inheritdoc cref="Pass(AsserterMod)"/>
    [DoesNotReturn]
    void Fail(AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="Debug(AsserterMod,string)"/>
    void Debug(string? details = null);

    /// <summary>Fails the test only if the option is set.</summary>
    /// <param name="details">Description to include in assertion failure messages.</param>
    /// <exception cref="AssertException">Always.</exception>
    /// <inheritdoc cref="Pass(AsserterMod)"/>
    void Debug(AsserterMod? optionConfiguration, string? details = null);

    /// <summary>Runs each case and aggregates exceptions.</summary>
    /// <param name="cases">Assert cases.</param>
    void CheckAll(params IEnumerable<Action> cases);
}
