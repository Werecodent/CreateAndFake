global using AsserterMod = System.Func<
    CreateAndFake.AsserterTool.AsserterOptions,
    CreateAndFake.AsserterTool.AsserterOptions>;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsserterTool;

/// <summary>Handles common test scenarios.</summary>
public interface IAsserter :
    IAsyncAsserter,
    IComparableAsserter,
    IDelegateAsserter,
    IEnumerableAsserter,
    IObjectAsserter,
    IStringAsserter,
    ITypeAsserter
{
    /// <summary>Configured options for <c>this</c>.</summary>
    AsserterOptions Options { get; }

    /// <inheritdoc cref="Pass(AsserterMod)"/>
    void Pass();

    /// <summary>Specifies the test is successful if it reaches this point.</summary>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
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
}