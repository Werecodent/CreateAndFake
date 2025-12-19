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
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> for the new tool.</param>
    /// <returns>The created tool.</returns>
    IAsyncAsserter WithOptions(AsyncAsserterMod optionConfiguration);

    /// <inheritdoc cref="IAsserter.Pass()"/>
    Task PassAsync();

    /// <inheritdoc cref="IAsserter.Pass(AsserterMod)"/>
    Task PassAsync(AsyncAsserterMod? optionConfiguration);

    /// <inheritdoc cref="IAsserter.Fail(string,string)"/>
    Task FailAsync(string? details = null, Task<string?>? content = null);

    /// <inheritdoc cref="IAsserter.Fail(AsserterMod,string,string)"/>
    Task FailAsync(
        AsyncAsserterMod? optionConfiguration,
        string? details = null,
        Task<string?>? content = null
    );

    /// <inheritdoc cref="IAsserter.Fail(Exception,string)"/>
    Task FailAsync(Task<Exception?>? exception, string? details = null);

    /// <inheritdoc cref="IAsserter.Fail(Exception,AsserterMod,string)"/>
    Task FailAsync(
        Task<Exception?>? exception,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );
}
