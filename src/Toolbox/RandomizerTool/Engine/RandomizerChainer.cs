using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.RandomizerTool.Engine;

/// <summary>Provides a callback into <see cref="IRandomizer"/> to create child values.</summary>
public sealed class RandomizerChainer
    : ToolChainer<RandomizerChainer, IRandomizerEngine, RandomizerOptions, CreateHint>,
        IRandomizerChainer
{
    /// <summary>Types not to create as to prevent infinite recursion.</summary>
    private readonly IDictionary<Type, object> _history;

    /// <inheritdoc/>
    public RandomizerChainer(RandomizerOptions options, IRandomizerEngine engine)
        : base(options, engine)
    {
        _history = new Dictionary<Type, object>();
    }

    /// <inheritdoc/>
    private RandomizerChainer(RandomizerOptions options, RandomizerChainer prevChainer)
        : base(options, prevChainer)
    {
        _history = prevChainer._history;
    }

    /// <inheritdoc/>
    protected override RandomizerChainer CreateSubChainer(RandomizerOptions subOptions)
    {
        return new RandomizerChainer(subOptions, this);
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
        object result = Engine.Create(type, GetSubChainer(optionConfiguration));

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
        return Engine.Inject(type, values, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public IRandomizer WithOptions(RandomizerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new RandomizerChainer(optionConfiguration.Invoke(Options), this);
    }
}
