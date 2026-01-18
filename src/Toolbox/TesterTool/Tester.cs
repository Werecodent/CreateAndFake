using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.TesterTool;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public class Tester(TesterOptions options) : ITester
{
    /// <inheritdoc/>
    public TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => [];

    /// <inheritdoc/>
    public virtual Task PreventsNullRefException<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return PreventsNullRefException(typeof(T), canceler, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual async Task PreventsNullRefException(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        NullGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            await checker.PreventsNullRefExceptionOnConstructors(type, true).ConfigureAwait(false);
        }

        await CreateInstanceAndTestMethodsAsync(
                type,
                localOptions,
                checker.PreventsNullRefExceptionOnMethods
            )
            .ConfigureAwait(false);

        if (localOptions.IncludeStaticMethods)
        {
            await checker.PreventsNullRefExceptionOnStatics(type, true).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public virtual async Task PreventsNullRefException<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(instance, nameof(instance));

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        NullGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            await checker
                .PreventsNullRefExceptionOnConstructors(typeof(T), false)
                .ConfigureAwait(false);
        }
        if (localOptions.IncludeInstanceMethods)
        {
            await checker.PreventsNullRefExceptionOnMethods(instance).ConfigureAwait(false);
        }
        if (localOptions.IncludeStaticMethods)
        {
            await checker.PreventsNullRefExceptionOnStatics(typeof(T), false).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public virtual Task PreventsParameterMutation<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return PreventsParameterMutation(typeof(T), canceler, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual async Task PreventsParameterMutation(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        MutationGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            await checker
                .PreventsMutationOnConstructors(type, true, canceler)
                .ConfigureAwait(false);
        }

        await CreateInstanceAndTestMethodsAsync(
                type,
                localOptions,
                o => checker.PreventsMutationOnMethods(o, canceler)
            )
            .ConfigureAwait(false);

        if (localOptions.IncludeStaticMethods)
        {
            await checker.PreventsMutationOnStatics(type, true, canceler).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public virtual async Task PreventsParameterMutation<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(instance, nameof(instance));

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        MutationGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            await checker
                .PreventsMutationOnConstructors(typeof(T), false, canceler)
                .ConfigureAwait(false);
        }
        if (localOptions.IncludeInstanceMethods)
        {
            await checker.PreventsMutationOnMethods(instance, canceler).ConfigureAwait(false);
        }
        if (localOptions.IncludeStaticMethods)
        {
            await checker
                .PreventsMutationOnStatics(typeof(T), false, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public virtual Task PassthroughWithNoExceptions<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        object instance = localOptions.Randomizer.Create<Injected<T>>()!.Dummy!;

        return new ExceptionGuarder(localOptions).CallAllMethods(instance);
    }

    /// <inheritdoc/>
    public virtual Task PassthroughWithNoExceptions(
        object instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        return new ExceptionGuarder(localOptions).CallAllMethods(instance);
    }

    /// <summary>Attempts to test all methods.</summary>
    /// <param name="type">Type being tested.</param>
    /// <param name="localOptions">Configured options to use.</param>
    /// <param name="checker">Test to run.</param>
    private static async Task CreateInstanceAndTestMethodsAsync(
        Type type,
        TesterOptions localOptions,
        Func<object, Task> checker
    )
    {
        if (localOptions.IncludeInstanceMethods && !(type.IsAbstract && type.IsSealed))
        {
            object instance =
                (localOptions.InjectionValues.Length > 0)
                    ? localOptions.Randomizer.Inject(type, localOptions.InjectionValues)
                    : localOptions.Randomizer.Create(type);
            try
            {
                await checker.Invoke(instance).ConfigureAwait(false);
            }
            finally
            {
                await Disposer.CleanupAsync(instance).ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc/>
    public virtual void ProvidesTestClassCoverage(
        Assembly codeAssembly,
        Assembly testAssembly,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(codeAssembly, nameof(codeAssembly));
        ArgumentGuard.ThrowIfNull(testAssembly, nameof(testAssembly));

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        FrozenSet<string> testClasses = testAssembly.GetTypes().Select(t => t.Name).ToFrozenSet();

        localOptions.Asserter.IsEmpty(
            TypeDescriber
                .FindLoadedClassTypes(codeAssembly)
                .Where(t => !t.IsAbstract)
                .Where(t => TypeDescriber.IsVisible(t, testAssembly.GetName()))
                .Where(t =>
                {
                    IEnumerable<string> possibleNames;
                    if (t.IsGenericTypeDefinition)
                    {
                        string baseName = t.Name.Substring(
                            0,
                            t.Name.IndexOf("`", StringComparison.InvariantCulture)
                        );
                        possibleNames = localOptions.TestClassNameGenericSubstitutes.Select(sub =>
                            baseName + sub
                        );
                    }
                    else
                    {
                        possibleNames = [t.Name];
                    }
                    return possibleNames.All(name =>
                        !testClasses.Contains(name + localOptions.TestClassNameSuffix)
                    );
                })
                .Where(t => !localOptions.TestClassCoverageExceptions.Contains(t.Name)),
            $"Missing tests for classes from {codeAssembly} in {testAssembly}."
        );
    }

    /// <inheritdoc/>
    public virtual async Task ValidateRandomDataParameters(
        Assembly testAssembly,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(testAssembly, nameof(testAssembly));

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        IEnumerable<MethodInfo> testMethods = TypeDescriber
            .FindLoadedTypes(testAssembly)
            .Where(t => !t.IsGenericType)
            .SelectMany(t =>
                t.GetMethods(
                    BindingFlags.Instance
                        | BindingFlags.Static
                        | BindingFlags.Public
                        | BindingFlags.NonPublic
                        | BindingFlags.FlattenHierarchy
                )
            )
            .Where(m =>
                !m.IsGenericMethod && m.GetCustomAttributes(true).Any(a => a is IRandomDataMarker)
            );

        foreach (MethodInfo method in testMethods)
        {
            MethodCallWrapper? data = null;
            try
            {
                data = localOptions.Runner.CreateFor(method);
                foreach (object? item in data.Args)
                {
                    _ = localOptions.TestDisplayNameConverter.Invoke(item);
                }
            }
            catch (Exception e)
            {
                localOptions.Asserter.Fail(e, $"Randomization failed for method '{method}'");
            }
            finally
            {
                await Disposer.CleanupAsync(data?.Args).ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc/>
    public ITester WithOptions(TesterMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration, nameof(optionConfiguration));
        return new Tester(optionConfiguration.Invoke(Options));
    }
}
