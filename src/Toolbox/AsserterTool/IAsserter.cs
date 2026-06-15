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
        IAsserterAction,
        IAsserterAsyncEnumerable,
        IAsserterAsyncObject,
        IAsserterComparable,
        IAsserterDelegate,
        IAsserterEnumerable,
        IAsserterFunc,
        IAsserterObject,
        IAsserterString,
        IAsserterTask,
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

    /// <inheritdoc cref="Fail(AsserterMod,string,string)"/>
    [DoesNotReturn]
    void Fail(string? details = null, string? content = null);

    /// <param name="content">Content responsible for the failure.</param>
    /// <inheritdoc cref="Fail(Exception,AsserterMod,string)"/>
    [DoesNotReturn]
    void Fail(AsserterMod? optionConfiguration, string? details = null, string? content = null);

    /// <inheritdoc cref="Fail(Exception,AsserterMod,string)"/>
    [DoesNotReturn]
    void Fail(Exception? exception, string? details = null);

    /// <summary>Specifies the test has failed if it reaches this point.</summary>
    /// <param name="exception">Exception responsible for the failure.</param>
    /// <param name="details">Description to include in assertion failure messages.</param>
    /// <exception cref="AssertException">Always.</exception>
    /// <inheritdoc cref="Pass(AsserterMod)"/>
    [DoesNotReturn]
    void Fail(Exception? exception, AsserterMod? optionConfiguration, string? details = null);

    /// <summary>Runs each case and aggregates exceptions.</summary>
    /// <param name="cases">Assert cases.</param>
    void CheckAll(params IEnumerable<Action> cases);
}
