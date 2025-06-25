using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.RandomizerTool.Engine;

/// <summary>Provides a callback into <see cref="IRandomizer"/> to create child values.</summary>
public sealed class RandomizerChainer
    : ToolChainer<RandomizerChainer, RandomizerOptions, CreateHint>,
        IRandomizerChainer
{
    /// <summary>Callback mechanism.</summary>
    private readonly IRandomizerEngine _engine;

    /// <summary>Types not to create as to prevent infinite recursion.</summary>
    private readonly IDictionary<Type, object> _history;

    /// <inheritdoc cref="IRandomizerChainer"/>
    /// <param name="options"><inheritdoc cref="ITool{T}.Options" path="/summary"/></param>
    /// <param name="engine"><inheritdoc cref="_engine" path="/summary"/></param>
    public RandomizerChainer(RandomizerOptions options, IRandomizerEngine engine)
        : base(options)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        _history = new Dictionary<Type, object>();
    }

    /// <inheritdoc cref="IRandomizerChainer"/>
    /// <param name="options"><inheritdoc cref="ITool{T}.Options" path="/summary"/></param>
    /// <param name="prevChainer">Previous chainer to build upon.</param>
    private RandomizerChainer(RandomizerOptions options, RandomizerChainer prevChainer)
        : base(options)
    {
        _engine = prevChainer._engine;
        _history = prevChainer._history;
    }

    /// <inheritdoc/>
    protected override RandomizerChainer CreateSubChainer(RandomizerOptions options)
    {
        return new RandomizerChainer(options, this);
    }

    /// <inheritdoc/>
    public bool AlreadyCreated<T>()
    {
        return AlreadyCreated(typeof(T));
    }

    /// <inheritdoc/>
    public bool AlreadyCreated(Type type)
    {
        return _history.ContainsKey(type);
    }

    /// <inheritdoc/>
    public object Create(Type type, object? parent, RandomizerMod? optionConfiguration = null)
    {
        if (AlreadyCreated(type))
        {
            return _history[type];
        }
        else if (parent?.GetType() == type)
        {
            return parent;
        }

        if (parent != null && !_history.ContainsKey(parent.GetType()))
        {
            _history.Add(parent.GetType(), parent);
        }

        RuntimeHelpers.EnsureSufficientExecutionStack();
        object result = _engine.Create(type, GetSubChainer(optionConfiguration));

        if (parent != null)
        {
            _ = _history.Remove(parent.GetType());
        }
        return result;
    }

    /// <inheritdoc/>
    public T Create<T>(RandomizerMod? optionConfiguration = null)
    {
        return (T)Create(typeof(T), null, optionConfiguration);
    }

    /// <inheritdoc/>
    public object Create(Type type, RandomizerMod? optionConfiguration = null)
    {
        return Create(type, null, optionConfiguration);
    }

    /// <inheritdoc/>
    public T Inject<T>(IEnumerable<object?>? values, RandomizerMod? optionConfiguration = null)
    {
        return (T)Inject(typeof(T), values);
    }

    /// <inheritdoc/>
    public object Inject(
        Type type,
        IEnumerable<object?>? values,
        RandomizerMod? optionConfiguration = null
    )
    {
        return _engine.Inject(type, values, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public IRandomizer WithOptions(RandomizerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration, nameof(optionConfiguration));
        return new RandomizerChainer(optionConfiguration.Invoke(Options), this);
    }
}
