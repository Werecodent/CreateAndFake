using System.Runtime.CompilerServices;

namespace CreateAndFake.Toolbox.RandomizerTool;

/// <summary>Provides a callback into <see cref="IRandomizer"/> to create child values.</summary>
public sealed class RandomizerChainer
{
    /// <summary>Callback to <see cref="IRandomizer"/> to randomize child values.</summary>
    private readonly Func<Type, RandomizerChainer, object> _randomizer;

    /// <summary>Types not to create as to prevent infinite recursion.</summary>
    private readonly IDictionary<Type, object> _history;

    /// <summary>Container of the instance to create.</summary>
    public object? Parent { get; }

    /// <inheritdoc cref="RandomizerOptions"/>
    public RandomizerOptions Options { get; }

    /// <inheritdoc cref="RandomizerChainer"/>
    /// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
    /// <param name="randomizer"><inheritdoc cref="_randomizer" path="/summary"/></param>
    public RandomizerChainer(RandomizerOptions options, Func<Type, RandomizerChainer, object> randomizer)
    {
        _randomizer = randomizer ?? throw new ArgumentNullException(nameof(randomizer));
        Options = options ?? throw new ArgumentNullException(nameof(options));

        _history = new Dictionary<Type, object>();
        Parent = null;
    }

    /// <inheritdoc cref="RandomizerChainer"/>
    /// <param name="prevChainer">Previous chainer to build upon.</param>
    /// <param name="options">FIX ME</param>
    /// <param name="parent"><inheritdoc cref="Parent" path="/summary"/></param>
    private RandomizerChainer(RandomizerChainer prevChainer, RandomizerOptions? options, object? parent)
    {
        Parent = parent;
        Options = options ?? prevChainer.Options.NestedOptions ?? prevChainer.Options;
        _randomizer = prevChainer._randomizer;

        if (parent != null)
        {
            _history = prevChainer._history
                .Append(new KeyValuePair<Type, object>(parent.GetType(), parent))
                .ToDictionary(p => p.Key, p => p.Value);
        }
        else
        {
            _history = prevChainer._history;
        }
    }

    /// <summary>Checks if <typeparamref name="T"/> has already been created by the randomizer.</summary>
    /// <typeparam name="T"><c>Type</c> to check.</typeparam>
    /// <returns><c>true</c> if <typeparamref name="T"/> already created; <c>false</c> otherwise.</returns>
    public bool AlreadyCreated<T>()
    {
        return AlreadyCreated(typeof(T));
    }

    /// <summary>Checks if <paramref name="type"/> has already been created by the randomizer.</summary>
    /// <param name="type"><c>Type</c> to check.</param>
    /// <returns><c>true</c> if <paramref name="type"/> already created; <c>false</c> otherwise.</returns>
    public bool AlreadyCreated(Type type)
    {
        return _history.ContainsKey(type);
    }

    /// <summary>Calls the randomizer to create a random <typeparamref name="T"/> instance.</summary>
    /// <typeparam name="T">Type to create.</typeparam>
    /// <returns>The created <typeparamref name="T"/> instance.</returns>
    public T Create<T>()
    {
        return (T)Create(typeof(T), null);
    }

    /// <summary>Calls the randomizer to create a random instance of the given <paramref name="type"/>.</summary>
    /// <param name="type">Type to create.</param>
    /// <param name="parent"><inheritdoc cref="Parent" path="/summary"/></param>
    /// <returns>The created instance.</returns>
    public object Create(Type type, object? parent = null)
    {
        return Create(type, null, parent);
    }

    /// <summary>Calls the randomizer to create a random instance of the given <paramref name="type"/>.</summary>
    /// <param name="type">Type to create.</param>
    /// <param name="options">FIX ME</param>
    /// <param name="parent"><inheritdoc cref="Parent" path="/summary"/></param>
    /// <returns>The created instance.</returns>
    public object Create(Type type, RandomizerOptions? options, object? parent = null)
    {
        if (parent != null)
        {
            if (AlreadyCreated(type))
            {
                return _history[type];
            }
            else if (parent.GetType() == type)
            {
                return parent;
            }
        }
        else if (AlreadyCreated(type))
        {
            throw new InfiniteLoopException(type, _history.Keys);
        }

        RuntimeHelpers.EnsureSufficientExecutionStack();
        return _randomizer.Invoke(type, new RandomizerChainer(this, options, (parent != Parent) ? parent : null));
    }
}
