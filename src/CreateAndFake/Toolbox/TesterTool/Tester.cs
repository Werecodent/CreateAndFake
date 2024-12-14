using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFake.Toolbox.TesterTool;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public class Tester(TesterOptions options)
{
    /// <inheritdoc/>
    public TesterOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public virtual void PreventsNullRefException<T>(TesterMod? optionConfiguration = null)
    {
        PreventsNullRefException(typeof(T), optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual void PreventsNullRefException(Type type, TesterMod? optionConfiguration = null)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        NullGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Null reference check on constructors for type '{type}'",
                () => checker.PreventsNullRefExceptionOnConstructors(type, true, localOptions.InjectionValues));
        }

        if (localOptions.IncludeInstanceMethods && !(type.IsAbstract && type.IsSealed))
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Null reference check on methods for type '{type}'",
                () =>
                {
                    object? instance = (localOptions.InjectionValues.Length > 0)
                        ? Options.Randomizer.Inject(type, localOptions.InjectionValues)
                        : Options.Randomizer.Create(type);
                    try
                    {
                        checker.PreventsNullRefExceptionOnMethods(instance!, localOptions.InjectionValues);
                    }
                    finally
                    {
                        Disposer.Cleanup(instance);
                    }
                });
        }

        if (localOptions.IncludeStaticMethods)
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Null reference check on static methods for type '{type}'",
                () => checker.PreventsNullRefExceptionOnStatics(type, true, localOptions.InjectionValues));
        }
    }

    /// <inheritdoc/>
    public virtual void PreventsNullRefException<T>(T instance, TesterMod? optionConfiguration = null)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        NullGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Null reference check on constructors for type '{typeof(T).Name}'",
                () => checker.PreventsNullRefExceptionOnConstructors(typeof(T), false, localOptions.InjectionValues));
        }
        if (localOptions.IncludeInstanceMethods)
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Null reference check on methods for type '{typeof(T).Name}'",
                () => checker.PreventsNullRefExceptionOnMethods(instance!, localOptions.InjectionValues));
        }
        if (localOptions.IncludeStaticMethods)
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Null reference check on static methods for type '{typeof(T).Name}'",
                () => checker.PreventsNullRefExceptionOnStatics(typeof(T), false, localOptions.InjectionValues));
        }
    }

    /// <inheritdoc/>
    public virtual void PreventsParameterMutation<T>(TesterMod? optionConfiguration = null)
    {
        PreventsParameterMutation(typeof(T), optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual void PreventsParameterMutation(Type type, TesterMod? optionConfiguration = null)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        MutationGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Parameter mutation check on constructors for type '{type}'",
                () => checker.PreventsMutationOnConstructors(type, true, localOptions.InjectionValues));
        }

        if (localOptions.IncludeInstanceMethods && !(type.IsAbstract && type.IsSealed))
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Parameter mutation check on methods for type '{type}'",
                () =>
                {
                    object? instance = (localOptions.InjectionValues.Length > 0)
                        ? Options.Randomizer.Inject(type, localOptions.InjectionValues)
                        : Options.Randomizer.Create(type);
                    try
                    {
                        checker.PreventsMutationOnMethods(instance!, localOptions.InjectionValues);
                    }
                    finally
                    {
                        Disposer.Cleanup(instance);
                    }
                });
        }

        if (localOptions.IncludeStaticMethods)
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Parameter mutation check on static methods for type '{type}'",
                () => checker.PreventsMutationOnStatics(type, true, localOptions.InjectionValues));
        }
    }

    /// <inheritdoc/>
    public virtual void PreventsParameterMutation<T>(T instance, TesterMod? optionConfiguration = null)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        MutationGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Parameter mutation check on constructors for type '{typeof(T).Name}'",
                () => checker.PreventsMutationOnConstructors(typeof(T), false, localOptions.InjectionValues));
        }
        if (localOptions.IncludeInstanceMethods)
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Parameter mutation check on methods for type '{typeof(T).Name}'",
                () => checker.PreventsMutationOnMethods(instance!, localOptions.InjectionValues));
        }
        if (localOptions.IncludeStaticMethods)
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Parameter mutation check on static methods for type '{typeof(T).Name}'",
                () => checker.PreventsMutationOnStatics(typeof(T), false, localOptions.InjectionValues));
        }
    }

    /// <inheritdoc/>
    public virtual void PassthroughWithNoExceptions<T>(TesterMod? optionConfiguration = null)
    {
        PassthroughWithNoExceptions(Options.Randomizer.Create<Injected<T>>()!.Dummy!, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual void PassthroughWithNoExceptions(object instance, TesterMod? optionConfiguration = null)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        new ExceptionGuarder(localOptions).CallAllMethods(instance, localOptions.InjectionValues);
    }
}
