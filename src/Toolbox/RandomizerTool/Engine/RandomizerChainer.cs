using System.Runtime.CompilerServices;
using CreateAndFake.Design;

namespace CreateAndFake.RandomizerTool.Engine;

/// <summary>Provides a callback into <see cref="IRandomizer"/> to create child values.</summary>
public sealed class RandomizerChainer : IRandomizerChainer
{
    /// <summary>Callback mechanism.</summary>
    private readonly IRandomizerEngine _engine;

    /// <summary>Types not to create as to prevent infinite recursion.</summary>
    private readonly IDictionary<Type, object> _history;

    /// <inheritdoc cref="RandomizerOptions"/>
    public RandomizerOptions Options { get; }

    /// <inheritdoc cref="IRandomizerChainer"/>
    /// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
    /// <param name="engine"><inheritdoc cref="_engine" path="/summary"/></param>
    public RandomizerChainer(RandomizerOptions options, IRandomizerEngine engine)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        Options = options ?? throw new ArgumentNullException(nameof(options));

        _history = new Dictionary<Type, object>();
    }

    /// <inheritdoc cref="IRandomizerChainer"/>
    /// <param name="prevChainer">Previous chainer to build upon.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> for the new tool.</param>
    private RandomizerChainer(RandomizerChainer prevChainer, RandomizerMod? optionConfiguration)
    {
        Options =
            (optionConfiguration != null)
                ? optionConfiguration.Invoke(
                    prevChainer.Options.NestedOptions ?? prevChainer.Options
                )
                : prevChainer.Options.NestedOptions ?? prevChainer.Options;
        _engine = prevChainer._engine;

        _history = prevChainer._history;
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
        object result =
            (optionConfiguration != null || Options.NestedOptions != null)
                ? _engine.Create(type, new RandomizerChainer(this, optionConfiguration))
                : _engine.Create(type, this);

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
        return (optionConfiguration != null || Options.NestedOptions != null)
            ? _engine.Inject(type, values, new RandomizerChainer(this, optionConfiguration))
            : _engine.Inject(type, values, this);
    }

    /// <inheritdoc/>
    public IRandomizer WithOptions(RandomizerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration, nameof(optionConfiguration));
        return new RandomizerChainer(this, optionConfiguration);
    }
}
