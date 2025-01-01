using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFake.Toolbox.TesterTool;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public class Tester(TesterOptions options) : ITester
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
            checker.PreventsNullRefExceptionOnConstructors(type, true, localOptions.InjectionValues);
        }

        if (localOptions.IncludeInstanceMethods && !(type.IsAbstract && type.IsSealed))
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
        }

        if (localOptions.IncludeStaticMethods)
        {
            checker.PreventsNullRefExceptionOnStatics(type, true, localOptions.InjectionValues);
        }
    }

    /// <inheritdoc/>
    public virtual void PreventsNullRefException<T>(T instance, TesterMod? optionConfiguration = null)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        NullGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            checker.PreventsNullRefExceptionOnConstructors(typeof(T), false, localOptions.InjectionValues);
        }
        if (localOptions.IncludeInstanceMethods)
        {
            checker.PreventsNullRefExceptionOnMethods(instance!, localOptions.InjectionValues);
        }
        if (localOptions.IncludeStaticMethods)
        {
            checker.PreventsNullRefExceptionOnStatics(typeof(T), false, localOptions.InjectionValues);
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
            checker.PreventsMutationOnConstructors(type, true, localOptions.InjectionValues);
        }

        if (localOptions.IncludeInstanceMethods && !(type.IsAbstract && type.IsSealed))
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
        }

        if (localOptions.IncludeStaticMethods)
        {
            checker.PreventsMutationOnStatics(type, true, localOptions.InjectionValues);
        }
    }

    /// <inheritdoc/>
    public virtual void PreventsParameterMutation<T>(T instance, TesterMod? optionConfiguration = null)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        MutationGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            checker.PreventsMutationOnConstructors(typeof(T), false, localOptions.InjectionValues);
        }
        if (localOptions.IncludeInstanceMethods)
        {
            checker.PreventsMutationOnMethods(instance!, localOptions.InjectionValues);
        }
        if (localOptions.IncludeStaticMethods)
        {
            checker.PreventsMutationOnStatics(typeof(T), false, localOptions.InjectionValues);
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
