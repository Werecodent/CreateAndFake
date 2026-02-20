using System.Collections.Frozen;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
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
        ArgumentGuard.ThrowIfNull(type);

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        if (localOptions.DisableNullRefExceptionTests)
        {
            return;
        }

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
        ArgumentGuard.ThrowIfNull(codeAssembly, testAssembly);

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
    public virtual async Task VerifyToolSetIntegrity(
        ToolSet tools,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(tools);

        TesterOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        Dictionary<Type, Exception> failures = [];

        foreach (
            Type type in Tools
                .Randomizer.SupportedTypes.Where(t =>
                    !localOptions.IntegrityIgnorableTypes.Contains(t)
                )
                .Distinct()
        )
        {
            try
            {
                await VerifyToolSetIntegrity(tools, type, canceler).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                failures.Add(type, e);
            }
        }
        tools.Asserter.IsEmpty(failures);
    }

    /// <summary>Verifies the <paramref name="type"/> works with the tools.</summary>
    /// <param name="type">The <see cref="Type"/> to test with.</param>
    /// <inheritdoc cref="VerifyToolSetIntegrity(ToolSet,CancellationToken,TesterMod)"/>
    private static async Task VerifyToolSetIntegrity(
        ToolSet tools,
        Type type,
        CancellationToken canceler
    )
    {
        string failMessage = "Behavior did not work for type '" + type.FullName + "'.";
        object? original = null,
            variant = null,
            dupe = null;
        try
        {
            original = tools.Randomizer.Create(type);
            dupe = tools.Duplicator.Copy(original);

            await tools
                .Asserter.ValuesEqualAsync(original, dupe, canceler, failMessage)
                .ConfigureAwait(false);

            if (
                TypeDescriber.GetAllProperties(type).Any() || TypeDescriber.GetAllFields(type).Any()
            )
            {
                variant = tools.Mutator.Variant(type, original);

                await tools
                    .Asserter.ValuesNotEqualAsync(original, variant, canceler, failMessage)
                    .ConfigureAwait(false);

                if (tools.Mutator.Modify(original))
                {
                    await tools
                        .Asserter.ValuesNotEqualAsync(dupe, original, canceler)
                        .ConfigureAwait(false);
                }
            }

            if (
                tools.Faker.Supports(type)
                && !type.Inherits<IDisposable>()
                && !type.Inherits<IToolOptions>()
            )
            {
                _ = tools.Faker.Mock(type);
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
