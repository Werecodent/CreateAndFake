using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFake.Toolbox.TesterTool;

#pragma warning disable CA1865 // Use 'string.IndexOf(char)' instead: Not available for all versions.

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
            checker.PreventsNullRefExceptionOnConstructors(type, true);
        }

        CreateInstanceAndTestMethods(type, localOptions, checker.PreventsNullRefExceptionOnMethods);

        if (localOptions.IncludeStaticMethods)
        {
            checker.PreventsNullRefExceptionOnStatics(type, true);
        }
    }

    /// <inheritdoc/>
    public virtual void PreventsNullRefException<T>(T instance, TesterMod? optionConfiguration = null)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        NullGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            checker.PreventsNullRefExceptionOnConstructors(typeof(T), false);
        }
        if (localOptions.IncludeInstanceMethods)
        {
            checker.PreventsNullRefExceptionOnMethods(instance!);
        }
        if (localOptions.IncludeStaticMethods)
        {
            checker.PreventsNullRefExceptionOnStatics(typeof(T), false);
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
            checker.PreventsMutationOnConstructors(type, true);
        }

        CreateInstanceAndTestMethods(type, localOptions, checker.PreventsMutationOnMethods);

        if (localOptions.IncludeStaticMethods)
        {
            checker.PreventsMutationOnStatics(type, true);
        }
    }

    /// <inheritdoc/>
    public virtual void PreventsParameterMutation<T>(T instance, TesterMod? optionConfiguration = null)
    {
        ArgumentGuard.ThrowIfNull(instance, nameof(instance));

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        MutationGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            checker.PreventsMutationOnConstructors(typeof(T), false);
        }
        if (localOptions.IncludeInstanceMethods)
        {
            checker.PreventsMutationOnMethods(instance);
        }
        if (localOptions.IncludeStaticMethods)
        {
            checker.PreventsMutationOnStatics(typeof(T), false);
        }
    }

    /// <inheritdoc/>
    public virtual void PassthroughWithNoExceptions<T>(TesterMod? optionConfiguration = null)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        object instance = localOptions.Randomizer.Create<Injected<T>>()!.Dummy!;

        new ExceptionGuarder(localOptions).CallAllMethods(instance);
    }

    /// <inheritdoc/>
    public virtual void PassthroughWithNoExceptions(object instance, TesterMod? optionConfiguration = null)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        new ExceptionGuarder(localOptions).CallAllMethods(instance);
    }

    /// <summary>Attempts to test all methods.</summary>
    /// <param name="type">Type being tested.</param>
    /// <param name="localOptions">Configured options to use.</param>
    /// <param name="checker">Test to run.</param>
    private static void CreateInstanceAndTestMethods(Type type, TesterOptions localOptions, Action<object> checker)
    {
        if (localOptions.IncludeInstanceMethods && !(type.IsAbstract && type.IsSealed))
        {
            object instance = (localOptions.InjectionValues.Length > 0)
                ? localOptions.Randomizer.Inject(type, localOptions.InjectionValues)
                : localOptions.Randomizer.Create(type);
            try
            {
                checker.Invoke(instance);
            }
            finally
            {
                Disposer.Cleanup(instance);
            }
        }
    }

    /// <inheritdoc/>
    public virtual void ProvidesTestClassCoverage(Assembly codeAssembly, TesterMod? optionConfiguration = null)
    {
        ArgumentGuard.ThrowIfNull(codeAssembly, nameof(codeAssembly));

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        Assembly testAssembly = Assembly.GetCallingAssembly();
        BindingFlags scope = localOptions.IncludeInternals
            ? BindingFlags.Public | BindingFlags.NonPublic
            : BindingFlags.Public;

        FrozenSet<string> testClasses = testAssembly
            .GetTypes()
            .Select(t => t.Name)
            .ToFrozenSet();

        localOptions.Asserter.IsEmpty(
            TypeExtensions.FindLoadedClassTypes(codeAssembly)
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsVisibleTo(testAssembly.GetName()))
                .Where(t =>
                {
                    IEnumerable<string> possibleTestNames;
                    if (t.IsGenericTypeDefinition)
                    {
                        string baseName = t.Name.Substring(0, t.Name.IndexOf("`", StringComparison.InvariantCulture));
                        possibleTestNames = localOptions.TestClassNameGenericSubstitutes.Select(sub => baseName + sub);
                    }
                    else
                    {
                        possibleTestNames = [t.Name];
                    }
                    return possibleTestNames.All(name => !testClasses.Contains(name + localOptions.TestClassNameSuffix));
                })
                .Where(t => !localOptions.TestClassCoverageExceptions.Contains(t.Name)),
            "Missing tests for classes.");
    }
}

#pragma warning restore CA2249 // Use 'string.IndexOf(char)' instead
