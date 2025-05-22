using CreateAndFake.Design;

namespace CreateAndFake.AsyncAsserterTool;

/// <inheritdoc cref="IAsyncAsserter"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public partial class AsyncAsserter(AsyncAsserterOptions options) : IAsyncAsserter
{
    /// <inheritdoc/>
    public AsyncAsserterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Default option configuration to use.</summary>
    protected AsyncAsserterMod? Unconfigured { get; } = null;

    /// <summary>Merges <see cref="Options"/> with <paramref name="optionConfiguration"/>.</summary>
    /// <param name="optionConfiguration">Provided modifications of <see cref="Options"/> to merge.</param>
    /// <returns>The merged options to use.</returns>
    protected AsyncAsserterOptions ApplyConfiguration(AsyncAsserterMod? optionConfiguration)
    {
        return optionConfiguration?.Invoke(Options) ?? Options;
    }

    /// <inheritdoc/>
    public virtual Task Pass()
    {
        return Pass(Unconfigured);
    }

    /// <inheritdoc/>
    public virtual Task Pass(AsyncAsserterMod? optionConfiguration)
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        localOptions.Asserter.Pass();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task Fail(string? details = null, Task<string?>? content = null)
    {
        return Fail(Unconfigured, details, content);
    }

    /// <inheritdoc/>
    public virtual async Task Fail(
        AsyncAsserterMod? optionConfiguration,
        string? details = null,
        Task<string?>? content = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        localOptions.Asserter.Fail(
            details,
            (content != null) ? await content.ConfigureAwait(false) : null
        );
    }

    /// <inheritdoc/>
    public virtual Task Fail(Task<Exception?>? exception, string? details = null)
    {
        return Fail(exception, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task Fail(
        Task<Exception?>? exception,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        localOptions.Asserter.Fail(
            (exception != null) ? await exception.ConfigureAwait(false) : null,
            details
        );
    }

    /// <summary>Finds a suitable <c>Type</c> name to use for assertion messages.</summary>
    /// <param name="expected">Instance being compared to <c>actual</c>.</param>
    /// <param name="actual">Instance to run assertion checks with.</param>
    /// <returns>The <c>Type</c> name to use if found; <c>null</c> otherwise.</returns>
    private static string? GetTypeName(object? expected, object? actual)
    {
        return ExpandTypeName((expected ?? actual)?.GetType());
    }

    /// <summary>Builds <c>Type</c> name with generic argument names.</summary>
    /// <param name="type"><c>Type</c> to describe.</param>
    /// <returns>The built name.</returns>
    private static string? ExpandTypeName(Type? type)
    {
        if (type != null && type.IsGenericType)
        {
            return string.Concat(
                type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.InvariantCulture)),
                "<",
                string.Join(",", type.GetGenericArguments().Select(ExpandTypeName)),
                ">"
            );
        }
        else
        {
            return type?.Name;
        }
    }

    /// <inheritdoc/>
    public IAsyncAsserter WithOptions(AsyncAsserterMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration, nameof(optionConfiguration));
        return new AsyncAsserter(optionConfiguration.Invoke(Options));
    }
}
