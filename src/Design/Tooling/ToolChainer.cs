using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tooling;

/// <typeparam name="TSelf">Self-reference <see cref="Type"/>.</typeparam>
/// <typeparam name="TEngine">The <see cref="Type"/> for the hint engine.</typeparam>
/// <typeparam name="TOptions">The <see cref="Type"/> managing configuration options.</typeparam>
/// <typeparam name="THint">The <see cref="Type"/> of hints being used.</typeparam>
/// <inheritdoc cref="IToolChainer{T,T}"/>
public abstract class ToolChainer<TSelf, TEngine, TOptions, THint> : IToolChainer<TOptions, THint>
    where TSelf : ToolChainer<TSelf, TEngine, TOptions, THint>
    where TEngine : IToolEngine<THint>
    where TOptions : IToolHintOptions<TOptions, THint>
    where THint : IToolHint
{
    /// <summary><inheritdoc cref="IToolEngine{T}"/></summary>
    protected TEngine Engine { get; }

    /// <inheritdoc/>
    public TOptions Options { get; }

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => Engine.SupportedTypes;

    /// <summary>Current recursion level.</summary>
    private readonly int _nestedDepth;

    /// <summary>Starting constructor used to begin recursion tracking with.</summary>
    /// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
    /// <param name="engine"><inheritdoc cref="Engine" path="/summary"/></param>
    protected ToolChainer(TOptions options, TEngine engine)
    {
        ArgumentGuard.ThrowIfNull(options, engine);

        Options = options;
        Engine = engine;
        _nestedDepth = 0;
    }

    /// <summary>Nested constructor used to work on a child value with.</summary>
    /// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
    /// <param name="prevChainer">Previous chainer to build upon.</param>
    /// <remarks>
    ///     Not to be called directly. <see cref="GetSubChainer"/>
    ///     should be called instead. Implementation should be private.
    /// </remarks>
    /// <exception cref="EngineException">
    ///     Upon hitting the <see cref="IToolHintOptions{T,T}.MaxHintRecursion"/> depth.
    /// </exception>
    protected ToolChainer(TOptions options, TSelf prevChainer)
    {
        ArgumentGuard.ThrowIfNull(options, prevChainer);

        Options = options;
        Engine = prevChainer.Engine;
        _nestedDepth = prevChainer._nestedDepth + 1;

        if (_nestedDepth >= options.MaxHintRecursion)
        {
            throw new EngineException(
                $"Reached max recursion depth of '{options.MaxHintRecursion}'"
            );
        }
    }

    /// <summary>Gets the appropriate <typeparamref name="TSelf"/> for child values.</summary>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> for the new chainer.</param>
    /// <returns>The created <typeparamref name="TSelf"/> to use.</returns>
    protected TSelf GetSubChainer(Func<TOptions, TOptions>? optionConfiguration)
    {
        TOptions subOptions = Options.NestedOptions ?? Options;
        return CreateSubChainer(
            (optionConfiguration != null) ? optionConfiguration.Invoke(subOptions) : subOptions
        );
    }

    /// <summary>
    ///     Provides access to the concrete implementation of <see cref="ToolChainer(TOptions,TSelf)"/>
    ///     for the <see langword="base"/> <see langword="class"/>.
    /// </summary>
    /// <param name="subOptions">Configured options to use for the new chainer.</param>
    /// <returns>The created <typeparamref name="TSelf"/> to use for child values.</returns>
    /// <remarks>
    ///     Not to be called directly. <see cref="GetSubChainer"/> should be called instead.
    ///     Implementation should be like:
    ///     <example><c>return new ToolChainer(subOptions, this);</c></example>
    /// </remarks>
    protected abstract TSelf CreateSubChainer(TOptions subOptions);

    /// <inheritdoc/>
    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
