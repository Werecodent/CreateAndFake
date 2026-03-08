using System.Collections.Frozen;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.Design.Types;
using CreateAndFake.ExtractorTool;
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
    public virtual void VerifyJsonSerialization<T>(TesterMod? optionConfiguration = null)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        VerifyJsonSerialization(localOptions.Randomizer.Create<T>(), _ => localOptions);
    }

    /// <inheritdoc/>
    public virtual void VerifyJsonSerialization<T>(
        T instance,
        TesterMod? optionConfiguration = null
    )
    {
        VerifyJsonSerialization(typeof(T), instance, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual void VerifyJsonSerialization(
        object instance,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(instance);
        VerifyJsonSerialization(instance.GetType(), instance, optionConfiguration);
    }

    /// <param name="type">The <paramref name="instance"/> <see cref="Type"/> for testing.</param>
    /// <inheritdoc cref="VerifyJsonSerialization(object,TesterMod)"/>
    private void VerifyJsonSerialization(
        Type type,
        object? instance,
        TesterMod? optionConfiguration
    )
    {
        DataContractJsonSerializer serializer = new(type);
        VerifySerialization(type, instance, serializer, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual void VerifyXmlSerialization<T>(TesterMod? optionConfiguration = null)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        VerifyXmlSerialization(localOptions.Randomizer.Create<T>(), _ => localOptions);
    }

    /// <inheritdoc/>
    public virtual void VerifyXmlSerialization<T>(T instance, TesterMod? optionConfiguration = null)
    {
        VerifyXmlSerialization(typeof(T), instance, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual void VerifyXmlSerialization(
        object instance,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(instance);
        VerifyXmlSerialization(instance.GetType(), instance, optionConfiguration);
    }

    /// <param name="type">The <paramref name="instance"/> <see cref="Type"/> for testing.</param>
    /// <inheritdoc cref="VerifyXmlSerialization(object,TesterMod)"/>
    private void VerifyXmlSerialization(Type type, object? instance, TesterMod? optionConfiguration)
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        IContentMap contents = localOptions.Extractor.Extract(instance);
        DataContractSerializer serializer = new(
            type,
            contents.AllContent().Select(d => d.GetType()).Distinct()
        );

        VerifySerialization(type, instance, serializer, _ => localOptions);
    }

    /// <param name="type">The <paramref name="instance"/> <see cref="Type"/> for testing.</param>
    /// <inheritdoc cref="VerifyXmlSerialization(Type,object,TesterMod)"/>
    private void VerifySerialization(
        Type type,
        object? instance,
        XmlObjectSerializer serializer,
        TesterMod? optionConfiguration
    )
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        using MemoryStream stream = new();
        object? result;
        try
        {
            serializer.WriteObject(stream, instance);
            _ = stream.Seek(0, SeekOrigin.Begin);
            result = serializer.ReadObject(stream);
        }
        catch (Exception e) when (e is SerializationException or InvalidDataContractException)
        {
            throw new SerializationException(
                $"Ran into problem trying to serialize type '{TypeDescriber.ExpandedName(type)}'.",
                e
            );
        }

        localOptions.Asserter.Is(
            result,
            instance,
            $"Instance of type '{TypeDescriber.ExpandedName(type)}' did not deserialize with the same values."
        );
    }

    /// <inheritdoc/>
    public virtual Task PreventsNullRefExceptionAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return PreventsNullRefExceptionAsync(typeof(T), canceler, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual async Task PreventsNullRefExceptionAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(type);

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        if (localOptions.DisableNullRefExceptionTests)
        {
            return;
        }

        NullGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            await checker
                .PreventsNullRefExceptionOnConstructorsAsync(type, true, canceler)
                .ConfigureAwait(false);
        }

        await CreateInstanceAndTestMethodsAsync(
                type,
                localOptions,
                checker.PreventsNullRefExceptionOnMethodsAsync,
                canceler
            )
            .ConfigureAwait(false);

        if (localOptions.IncludeStaticMethods)
        {
            await checker
                .PreventsNullRefExceptionOnStaticsAsync(type, true, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public virtual async Task PreventsNullRefExceptionAsync<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(instance);

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        if (localOptions.DisableNullRefExceptionTests)
        {
            return;
        }

        NullGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            await checker
                .PreventsNullRefExceptionOnConstructorsAsync(typeof(T), false, canceler)
                .ConfigureAwait(false);
        }
        if (localOptions.IncludeInstanceMethods)
        {
            await checker
                .PreventsNullRefExceptionOnMethodsAsync(instance, canceler)
                .ConfigureAwait(false);
        }
        if (localOptions.IncludeStaticMethods)
        {
            await checker
                .PreventsNullRefExceptionOnStaticsAsync(typeof(T), false, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public virtual Task PreventsParameterMutationAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return PreventsParameterMutationAsync(typeof(T), canceler, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual async Task PreventsParameterMutationAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(type);

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        if (localOptions.DisableParameterMutationTests)
        {
            return;
        }

        MutationGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            await checker
                .PreventsMutationOnConstructorsAsync(type, true, canceler)
                .ConfigureAwait(false);
        }

        await CreateInstanceAndTestMethodsAsync(
                type,
                localOptions,
                checker.PreventsMutationOnMethodsAsync,
                canceler
            )
            .ConfigureAwait(false);

        if (localOptions.IncludeStaticMethods)
        {
            await checker
                .PreventsMutationOnStaticsAsync(type, true, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public virtual async Task PreventsParameterMutationAsync<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(instance);

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        if (localOptions.DisableParameterMutationTests)
        {
            return;
        }

        MutationGuarder checker = new(localOptions);

        if (localOptions.IncludeConstructors)
        {
            await checker
                .PreventsMutationOnConstructorsAsync(typeof(T), false, canceler)
                .ConfigureAwait(false);
        }
        if (localOptions.IncludeInstanceMethods)
        {
            await checker.PreventsMutationOnMethodsAsync(instance, canceler).ConfigureAwait(false);
        }
        if (localOptions.IncludeStaticMethods)
        {
            await checker
                .PreventsMutationOnStaticsAsync(typeof(T), false, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public virtual Task PassthroughWithNoExceptionsAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        if (localOptions.DisablePassthroughTests)
        {
            return Task.CompletedTask;
        }

        object instance = localOptions.Randomizer.Create<Injected<T>>()!.Dummy!;
        return new ExceptionGuarder(localOptions).CallAllMethodsAsync(instance, canceler);
    }

    /// <inheritdoc/>
    public virtual Task PassthroughWithNoExceptionsAsync(
        object instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        if (localOptions.DisablePassthroughTests)
        {
            return Task.CompletedTask;
        }

        return new ExceptionGuarder(localOptions).CallAllMethodsAsync(instance, canceler);
    }

    /// <summary>Attempts to test all methods.</summary>
    /// <param name="type">Type being tested.</param>
    /// <param name="localOptions">Configured options to use.</param>
    /// <param name="checker">Test to run.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    private static async Task CreateInstanceAndTestMethodsAsync(
        Type type,
        TesterOptions localOptions,
        Func<object, CancellationToken, Task> checker,
        CancellationToken canceler
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
                await checker.Invoke(instance, canceler).ConfigureAwait(false);
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
        ArgumentGuard.ThrowIfNull(codeAssembly, testAssembly);

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        FrozenSet<string> testClasses = testAssembly.GetTypes().Select(t => t.Name).ToFrozenSet();

        localOptions.Asserter.IsEmpty(
            TypeDescriber
                .FindLoadedClassTypes(codeAssembly)
                .Where(t => !t.IsAbstract || t.IsSealed)
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
                .Where(t => !localOptions.TestClassCoverageExceptions.Contains(t.Name))
                .Where(t =>
                    !t.Namespace!.StartsWith(
                        "Coverlet.Core.Instrumentation.Tracker",
                        StringComparison.Ordinal
                    )
                ),
            $"Missing tests for classes from {codeAssembly} in {testAssembly}."
        );
    }

    /// <inheritdoc/>
    public virtual async Task ValidateRandomDataParametersAsync(
        Assembly testAssembly,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(testAssembly);

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
                data = localOptions.Runner.CreateFor(method, canceler);
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
    public virtual Task VerifyToolSetIntegrityAsync(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return VerifyToolSetSupportAsync(
            Enumerable
                .Empty<Type>()
                .Concat(Tools.Randomizer.SupportedTypes)
                .Concat(Tools.Duplicator.SupportedTypes)
                .Concat(Tools.Extractor.SupportedTypes)
                .Concat(Tools.Mutator.SupportedTypes)
                .Concat(Tools.Valuer.SupportedTypes),
            canceler,
            optionConfiguration
        );
    }

    /// <inheritdoc/>
    public virtual async Task VerifyToolSetSupportAsync(
        IEnumerable<Type> types,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(types);

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        Type[] testTypes =
        [
            .. types.Where(t => !localOptions.IntegrityIgnorableTypes.Contains(t)).Distinct(),
        ];

        Dictionary<Type, Exception> failures = [];
        for (int i = 0; i < testTypes.Length; i++)
        {
            try
            {
                await VerifyToolSetSupportAsync(testTypes[i], localOptions, canceler)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                failures.Add(testTypes[i], e);
            }
        }
        localOptions.Asserter.IsEmpty(failures, "Not all types were supported as expected.");
    }

    /// <summary>
    ///     Validates the <paramref name="type"/> is fully
    ///     compatible with the framework as configured.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to test with.</param>
    /// <inheritdoc
    ///     cref="VerifyToolSetSupportAsync(IEnumerable{Type},CancellationToken,TesterMod)"/>
    private static async Task VerifyToolSetSupportAsync(
        Type type,
        TesterOptions localOptions,
        CancellationToken canceler
    )
    {
        object? original = null,
            variant = null,
            dupe = null;
        try
        {
            original = localOptions.Randomizer.Create(type);
            dupe = localOptions.Duplicator.Copy(original);

            string failMessage =
                "Behavior did not work for type '"
                + TypeDescriber.ExpandedName(type)
                + $"' randomized to '{TypeDescriber.ExpandedName(original)}'.";

            await localOptions
                .Asserter.ValuesEqualAsync(
                    original,
                    dupe,
                    canceler,
                    failMessage + " Cloned data was not equal."
                )
                .ConfigureAwait(false);

            if (
                type.IsAbstract
                || InheritanceTracker.For(type).IsMutable()
                || InheritanceTracker.For(type).HasInitializableOnlyState()
                || (
                    !type.IsSealed
                    && InheritanceTracker.For(type).FindLoadedSubclasses().Skip(1).Any()
                )
            )
            {
                variant = localOptions.Mutator.Variant(type, original);

                await localOptions
                    .Asserter.ValuesNotEqualAsync(
                        original,
                        variant,
                        canceler,
                        failMessage + " Variant data was still equal."
                    )
                    .ConfigureAwait(false);
            }

            if (localOptions.Mutator.Modify(original))
            {
                await localOptions
                    .Asserter.ValuesNotEqualAsync(
                        dupe,
                        original,
                        canceler,
                        failMessage + " Modified data was still equal."
                    )
                    .ConfigureAwait(false);
            }

            if (
                localOptions.Faker.Supports(type)
                && !type.Inherits<IDisposable>()
                && !type.Inherits<IToolOptions>()
            )
            {
                _ = localOptions.Faker.Mock(type);
            }
        }
        finally
        {
            await Disposer.CleanupAsync(original, variant, dupe).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public ITester WithOptions(TesterMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Tester(optionConfiguration.Invoke(Options));
    }
}
