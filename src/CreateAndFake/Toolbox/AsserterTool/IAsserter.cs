global using AsserterMod = System.Func<
    CreateAndFake.Toolbox.AsserterTool.AsserterOptions,
    CreateAndFake.Toolbox.AsserterTool.AsserterOptions>;
using CreateAndFake.Toolbox.AsserterTool.Categories;

namespace CreateAndFake.Toolbox.AsserterTool;

/// <summary>Handles common test scenarios.</summary>
public interface IAsserter :
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
    /// <param name="optionConfiguration">Modifications of <see cref="AsserterOptions"/> to apply for this call.</param>
    void Pass(AsserterMod optionConfiguration);

    /// <inheritdoc cref="Fail(AsserterMod,string,string)"/>
    void Fail(string? details = null, string? content = null);

    /// <param name="content">Content responsible for the failure.</param>
    /// <inheritdoc cref="Fail(Exception,AsserterMod,string)"/>
    void Fail(AsserterMod optionConfiguration, string? details = null, string? content = null);

    /// <inheritdoc cref="Fail(Exception,AsserterMod,string)"/>
    void Fail(Exception? exception, string? details = null);

    /// <summary>Specifies the test has failed if it reaches this point.</summary>
    /// <param name="exception">Exception responsible for the failure.</param>
    /// <param name="details">Description to include in assertion failure messages.</param>
    /// <exception cref="AssertException">Always.</exception>
    /// <inheritdoc cref="Pass(AsserterMod)"/>
    void Fail(Exception? exception, AsserterMod optionConfiguration, string? details = null);
}