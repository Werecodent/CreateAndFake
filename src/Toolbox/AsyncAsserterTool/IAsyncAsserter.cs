global using AsyncAsserterMod = System.Func<
    CreateAndFake.AsyncAsserterTool.AsyncAsserterOptions,
    CreateAndFake.AsyncAsserterTool.AsyncAsserterOptions
>;
using CreateAndFake.AsserterTool;
using CreateAndFake.AsyncAsserterTool.Categories;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.AsyncAsserterTool;

/// <summary>Handles common test scenarios.</summary>
public interface IAsyncAsserter
    : ITool<AsyncAsserterOptions>,
        IAsyncEnumerableAsserter,
        IAsyncObjectAsserter
{
    /// <inheritdoc cref="IAsserter.Pass()"/>
    Task Pass();

    /// <inheritdoc cref="IAsserter.Pass(AsserterMod)"/>
    Task Pass(AsyncAsserterMod? optionConfiguration);

    /// <inheritdoc cref="IAsserter.Fail(string,string)"/>
    Task Fail(string? details = null, Task<string?>? content = null);

    /// <inheritdoc cref="IAsserter.Fail(AsserterMod,string,string)"/>
    Task Fail(
        AsyncAsserterMod? optionConfiguration,
        string? details = null,
        Task<string?>? content = null
    );

    /// <inheritdoc cref="IAsserter.Fail(Exception,string)"/>
    Task Fail(Task<Exception?>? exception, string? details = null);

    /// <inheritdoc cref="IAsserter.Fail(Exception,AsserterMod,string)"/>
    Task Fail(
        Task<Exception?>? exception,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );
}
