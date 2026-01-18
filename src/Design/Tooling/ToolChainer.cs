using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tooling;

/// <summary>Handles recursive tool behavior.</summary>
/// <typeparam name="TSelf">Self type reference.</typeparam>
/// <typeparam name="TEngine">Engine driving the chainer behavior.</typeparam>
/// <typeparam name="TOptions">Type for the options.</typeparam>
/// <typeparam name="THint">Type for the hints.</typeparam>
/// <inheritdoc cref="ToolChainer{T, T, T, T}"/>
public abstract class ToolChainer<TSelf, TEngine, TOptions, THint> : IToolChainer<TOptions>
    where TSelf : ToolChainer<TSelf, TEngine, TOptions, THint>
    where TEngine : IToolEngine<THint>
    where TOptions : IToolHintOptions<TOptions, THint>
    where THint : IToolHint
{
    /// <summary>Callback mechanism.</summary>
    protected TEngine Engine { get; }

    /// <summary>Configured options.</summary>
    public TOptions Options { get; }

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => Engine.SupportedTypes;

    /// <summary>Current nested level.</summary>
    private readonly int _nestedDepth;

    /// <inheritdoc cref="IToolChainer{T}"/>
    /// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
    /// <param name="engine"><inheritdoc cref="IToolEngine{T}" path="/summary"/></param>
    protected ToolChainer(TOptions options, TEngine engine)
    {
        ArgumentGuard.ThrowIfNull(options, engine);
        Options = options;
        Engine = engine;
        _nestedDepth = 0;
    }

    /// <inheritdoc cref="IToolChainer{T}"/>
    /// <param name="options"><inheritdoc cref="ITool{T}.Options" path="/summary"/></param>
    /// <param name="prevChainer">Previous chainer to build upon.</param>
    protected ToolChainer(TOptions options, TSelf prevChainer)
    {
        ArgumentGuard.ThrowIfNull(options, prevChainer);
        Options = options;
        Engine = prevChainer.Engine;
        _nestedDepth = prevChainer._nestedDepth + 1;

        if (_nestedDepth >= options.MaxHintRecursion)
        {
            throw new ToolException($"Reached max recursion depth of '{options.MaxHintRecursion}'");
        }
    }

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
