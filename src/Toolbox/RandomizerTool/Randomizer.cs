using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.RandomizerTool;

/// <inheritdoc cref="IRandomizer"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public sealed class Randomizer(RandomizerOptions options) : IRandomizer
{
    /// <summary>Default set of hints to use for randomization.</summary>
    private static readonly ImmutableArray<CreateHint> _DefaultHints =
    [
        new ValueCreateHint(),
        new EnumCreateHint(),
        new GenericCreateHint(),
        new AsyncCollectionCreateHint(),
        new CollectionCreateHint(),
        new ImmutableCollectionCreateHint(),
        new FrozenCollectionCreateHint(),
        new LegacyCollectionCreateHint(),
        new SpanCreateHint(),
        new StringCreateHint(),
        new DelegateCreateHint(),
        new TaskCreateHint(),
        new CommonSystemCreateHint(),
        new TypeInfoCreateHint(),
        new InjectedCreateHint(),
        new FakeCreateHint(),
        new FakedCreateHint(),
        new ExceptionCreateHint(),
        new SelfCreateHint(),
        new OptionsCreateHint(),
        new ObjectCreateHint(),
    ];

    /// <inheritdoc/>
    public RandomizerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Generators used to randomize specific types.</summary>
    private readonly ImmutableArray<CreateHint> _hints = BuildHints(options);

    /// <summary>Builds hints to use for randomization based upon <paramref name="newOptions"/>.</summary>
    /// <param name="newOptions">Configuration for randomization.</param>
    /// <returns>Built hints to use.</returns>
    private static ImmutableArray<CreateHint> BuildHints(RandomizerOptions newOptions)
    {
        return newOptions.IncludeDefaultHints
            ? newOptions.Hints.AddRange(_DefaultHints)
            : newOptions.Hints;
    }

    /// <summary>Picks hints to use for randomization based upon <paramref name="localOptions"/>.</summary>
    /// <param name="localOptions">Potentially modified configuration to use.</param>
    /// <returns>Cached hints if possible; built hints otherwise.</returns>
    private ImmutableArray<CreateHint> SelectHints(RandomizerOptions localOptions)
    {
        return
            Options.IncludeDefaultHints == localOptions.IncludeDefaultHints
            && Options.Hints == localOptions.Hints
            ? _hints
            : BuildHints(localOptions);
    }

    /// <inheritdoc/>
    public T Create<T>(RandomizerMod? optionConfiguration = null)
    {
        return (T)Create(typeof(T), optionConfiguration);
    }

    /// <inheritdoc/>
    public object Create(Type type, RandomizerMod? optionConfiguration = null)
    {
        RandomizerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        try
        {
            return localOptions
                .Limiter.StallUntil(
                    $"Trying to create instance of '{type}'",
                    () =>
                        CreateByHint(
                            type,
                            new RandomizerChainer(localOptions, (t, c) => CreateByHint(t, c))
                        ),
                    result =>
                    {
                        if (localOptions.FinalCondition?.Invoke(result!) ?? true)
                        {
                            return true;
                        }
                        else
                        {
                            Disposer.Cleanup(result);
                            return false;
                        }
                    }
                )
                .Last()!;
        }
        catch (Exception e)
        {
            throw WrapCreateError(type, e);
        }
    }

    /// <summary>Adds details to encountered exceptions during randomization.</summary>
    /// <param name="type">Type attempted to be created.</param>
    /// <param name="e">Encountered exception.</param>
    /// <returns>Exception to throw.</returns>
    private static Exception WrapCreateError(Type type, Exception e)
    {
        Exception error = (e is AggregateException agg) ? agg.InnerException ?? e : e;

        if (error is InsufficientExecutionStackException)
        {
            return new InsufficientExecutionStackException(
                $"Ran into infinite generation trying to randomize type '{type}'.",
                error
            );
        }
        else if (error is TimeoutException)
        {
            return new TimeoutException(
                $"Could not create instance of type '{type}' matching condition.",
                error
            );
        }
        else if (error is NotSupportedException)
        {
            return new NotSupportedException(
                $"Encountered issue creating instance of type '{type}'.",
                error
            );
        }
        else
        {
            return new InvalidOperationException(
                $"Encountered issue creating instance of type '{type}'.",
                error
            );
        }
    }

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="Create(Type,RandomizerMod)"/>
    private object CreateByHint(Type type, RandomizerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        CreateHintResult? result = SelectHints(chainer.Options)
            .Select(h => h.TryCreate(type, chainer))
            .FirstOrDefault(r => r.HasData);

        if (result != null)
        {
            return result.Data!;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{type}' not supported by the randomizer. "
                    + "Create a hint to generate the type and pass it to the randomizer."
            );
        }
    }

    /// <inheritdoc/>
    public T Inject<T>(params IEnumerable<object?>? values)
    {
        return (T)Inject(typeof(T), values);
    }

    /// <inheritdoc/>
    public object Inject(Type type, params IEnumerable<object?>? values)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        List<Tuple<Type, object>> data =
        [
            .. (values ?? [])
                .Where(v => v != null)
                .Select(v => (v is Fake fake) ? fake.Dummy : v)
                .Where(v => v != null)
                .Select(v => Tuple.Create(v!.GetType(), v)),
        ];

        ConstructorInfo? maker =
            FindConstructor(type, data, BindingFlags.Public)
            ?? FindConstructor(type, data, BindingFlags.NonPublic);

        if (
            maker == null
            || type.Inherits<Fake>()
            || type.Inherits(typeof(Injected<>))
            || type.Inherits<Delegate>()
        )
        {
            return Create(type);
        }
        else
        {
            return maker.Invoke(CreateInjectArgs(maker, data));
        }
    }

    /// <summary>Creates the args to inject an instance with.</summary>
    /// <param name="maker">Constructor to use.</param>
    /// <param name="data">Canned data to prefer.</param>
    /// <returns>The created args to inject an instance with.</returns>
    private object?[] CreateInjectArgs(ConstructorInfo maker, List<Tuple<Type, object>> data)
    {
        ParameterInfo[] info = maker.GetParameters();
        object?[] args = new object[info.Length];

        for (int i = 0; i < args.Length; i++)
        {
            Tuple<Type, object>? match = data.FirstOrDefault(t =>
                t.Item1.Inherits(info[i].ParameterType)
            );
            if (match != default)
            {
                args[i] = match.Item2;
                _ = data.Remove(match);
            }
            else
            {
                args[i] = Create(info[i].ParameterType);
            }
        }
        return args;
    }

    /// <summary>Finds the constructor with the most matches then by fewest parameters.</summary>
    /// <param name="type">Type to find a constructor for.</param>
    /// <param name="data">Injection data to use.</param>
    /// <param name="scope">Scope of constructors to find.</param>
    /// <returns>Constructor if found; null otherwise.</returns>
    private static ConstructorInfo? FindConstructor(
        Type type,
        List<Tuple<Type, object>> data,
        BindingFlags scope
    )
    {
        return type.GetConstructors(BindingFlags.Instance | scope)
            .GroupBy(c =>
                c.GetParameters().Count(p => data.Any(t => t.Item1.Inherits(p.ParameterType)))
            )
            .Where(g => g.Key > 0)
            .OrderByDescending(g => g.Key)
            .FirstOrDefault()
            ?.OrderBy(c => c.GetParameters())
            .FirstOrDefault();
    }
}
