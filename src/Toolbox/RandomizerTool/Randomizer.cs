using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool;

/// <inheritdoc cref="IRandomizer"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Randomizer(RandomizerOptions options) : IRandomizer
{
    /// <summary>Handles hint based randomization.</summary>
    private static readonly RandomizerEngine _engine = new();

    /// <inheritdoc/>
    public RandomizerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _engine.SupportedTypes;

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
                .RandomizerCreateAttempts.StallUntil(
                    $"Trying to create instance of '{GenericTypeConverter.ExpandedName(type)}'",
                    () =>
                    {
                        return new RandomizerChainer(Options, _engine).Create(
                            type,
                            (optionConfiguration != null) ? _ => localOptions : null
                        );
                    },
                    result =>
                    {
                        if (localOptions.FinalCondition?.Invoke(result) ?? true)
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
                .Last();
        }
        catch (Exception e)
        {
            throw WrapCreateError(type, e);
        }
    }

    /// <inheritdoc/>
    public T Inject<T>(IEnumerable<object?>? values, RandomizerMod? optionConfiguration = null)
    {
        return (T)Inject(typeof(T), values, optionConfiguration);
    }

    /// <inheritdoc/>
    public object Inject(
        Type type,
        IEnumerable<object?>? values,
        RandomizerMod? optionConfiguration = null
    )
    {
        try
        {
            return new RandomizerChainer(Options, _engine).Inject(
                type,
                values,
                optionConfiguration
            );
        }
        catch (Exception e)
        {
            throw WrapCreateError(type, e);
        }
    }

    /// <inheritdoc/>
    public IRandomizer WithOptions(RandomizerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Randomizer(optionConfiguration.Invoke(Options));
    }

    /// <summary>Adds details to encountered exceptions during randomization.</summary>
    /// <param name="type">Type attempted to be created.</param>
    /// <param name="e">Encountered exception.</param>
    /// <returns>Exception to throw.</returns>
    private static ToolException WrapCreateError(Type type, Exception e)
    {
        Exception error = (e is AggregateException agg) ? agg.InnerException ?? e : e;

        string message;
        if (error is InsufficientExecutionStackException)
        {
            message =
                $"Ran into infinite generation trying to randomize type '{GenericTypeConverter.ExpandedName(type)}'.";
        }
        else if (error is TimeoutException)
        {
            message =
                $"Could not create instance of type '{GenericTypeConverter.ExpandedName(type)}' matching condition.";
        }
        else
        {
            message =
                $"Encountered issue creating instance of type '{GenericTypeConverter.ExpandedName(type)}'.";
        }
        return new ToolException(message, error);
    }
}
