using CreateAndFake.Design;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Tooling;
using CreateAndFake.Design.Types;

namespace CreateAndFake.RandomizerTool.Engine;

/// <summary>Provides a callback into <see cref="IRandomizer"/> to create child values.</summary>
public sealed class RandomizerChainer
    : ToolChainer<RandomizerChainer, IRandomizerEngine, RandomizerOptions, ICreateHint>,
        IRandomizerChainer
{
    /// <summary>Types not to create as to prevent infinite recursion.</summary>
    private readonly IDictionary<Type, object> _createHistory;

    private readonly ISet<Type> _attemptHistory;

    /// <inheritdoc/>
    public RandomizerChainer(RandomizerOptions options, IRandomizerEngine engine)
        : base(options, engine)
    {
        _createHistory = new Dictionary<Type, object>();
        _attemptHistory = new HashSet<Type>();
    }

    /// <inheritdoc/>
    private RandomizerChainer(RandomizerOptions options, RandomizerChainer prevChainer)
        : base(options, prevChainer)
    {
        _createHistory = prevChainer._createHistory;
        _attemptHistory = prevChainer._attemptHistory;
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
        return _createHistory.ContainsKey(type) || _attemptHistory.Contains(type);
    }

    /// <inheritdoc/>
    public object CreateSpecific(Type type, Type parent, RandomizerMod? optionConfiguration = null)
    {
        if (_attemptHistory.Add(type))
        {
            try
            {
                return Engine.Create(type, GetSubChainer(optionConfiguration));
            }
            finally
            {
                _ = _attemptHistory.Remove(type);
            }
        }
        else
        {
            throw new EngineException(
                $"Type '{GenericTypeConverter.ExpandedName(type)}' already created."
            );
        }
    }

    /// <inheritdoc/>
    public object CreateInternal(
        Type type,
        object parent,
        RandomizerMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(type, parent);

        if (_createHistory.TryGetValue(type, out object? previous))
        {
            return previous;
        }
        else if (parent.GetType() == type)
        {
            return parent;
        }

        if (!_createHistory.ContainsKey(parent.GetType()))
        {
            _createHistory.Add(parent.GetType(), parent);
            try
            {
                return Engine.Create(type, GetSubChainer(optionConfiguration));
            }
            finally
            {
                _ = _createHistory.Remove(parent.GetType());
            }
        }
        else
        {
            throw new EngineException(
                $"Type '{GenericTypeConverter.ExpandedName(parent)}' already created."
            );
        }
    }

    /// <inheritdoc/>
    public T Create<T>(RandomizerMod? optionConfiguration = null)
    {
        return (T)Engine.Create(typeof(T), GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public object Create(Type type, RandomizerMod? optionConfiguration = null)
    {
        return Engine.Create(type, GetSubChainer(optionConfiguration));
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
