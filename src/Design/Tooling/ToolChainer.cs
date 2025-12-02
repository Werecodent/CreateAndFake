using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tooling;

/// <summary>Handles recursive tool behavior.</summary>
/// <typeparam name="TSelf">Self type reference.</typeparam>
/// <typeparam name="TOptions">Type for the options.</typeparam>
/// <typeparam name="THint">Type for the hints.</typeparam>
/// <inheritdoc cref="ToolChainer{T, T, T}"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
public abstract class ToolChainer<TSelf, TOptions, THint>(TOptions options)
    where TSelf : ToolChainer<TSelf, TOptions, THint>
    where TOptions : IToolHintOptions<TOptions, THint>
    where THint : IToolHint
{
    /// <summary>Configured options.</summary>
    public TOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Test</summary>
    /// <param name="optionConfiguration"></param>
    /// <returns></returns>
    protected TSelf GetSubChainer(Func<TOptions, TOptions>? optionConfiguration)
    {
        if (optionConfiguration is not null || Options.NestedOptions is not null)
        {
            TOptions subOptions = Options.NestedOptions ?? Options;

            return CreateSubChainer(
                (optionConfiguration != null) ? optionConfiguration.Invoke(subOptions) : subOptions
            );
        }
        else
        {
            return (TSelf)this;
        }
    }

    /// <summary>Test</summary>
    /// <param name="options"></param>
    /// <returns></returns>
    protected abstract TSelf CreateSubChainer(TOptions options);

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
