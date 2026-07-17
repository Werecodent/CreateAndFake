using System.Collections.Frozen;
using System.Collections.Immutable;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.Properties;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.ValuerTool;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.TesterTool;

/// <summary>Configuration for controlling automated testing behavior.</summary>
public sealed record TesterOptions : IToolOptions
{
    /// <inheritdoc/>
    public required IRandom Gen { get; init; }

    /// <summary>Creates objects and populates them with random values.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Deep clones objects.</summary>
    public required IDuplicator Duplicator { get; init; }

    /// <summary><inheritdoc cref="IExtractor"/></summary>
    public required IExtractor Extractor { get; init; }

    /// <summary><inheritdoc cref="IFaker"/></summary>
    public required IFaker Faker { get; init; }

    /// <summary><inheritdoc cref="IMutator"/></summary>
    public required IMutator Mutator { get; init; }

    /// <summary>Handles common test scenarios.</summary>
    public required IAsserter Asserter { get; init; }

    /// <summary>Handles method generation.</summary>
    public required IRunner Runner { get; init; }

    /// <summary>Handles value comparisons.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>Retries tests if timeout is reached.</summary>
    [ConfigurableOption]
    public Limiter Limiter { get; init; } = Limiter.Few;

    /// <summary>Values to inject into called methods.</summary>
    public ImmutableArray<object?> InjectionValues { get; init; } = [];

    /// <summary>If constructors are included when running tests on classes.</summary>
    [ConfigurableOption]
    public bool IncludeConstructors { get; init; } = true;

    /// <summary>If class methods are included when running tests on classes.</summary>
    [ConfigurableOption]
    public bool IncludeInstanceMethods { get; init; } = true;

    /// <summary>If static methods are included when running tests on classes.</summary>
    [ConfigurableOption]
    public bool IncludeStaticMethods { get; init; } = false;

    /// <summary>If internal members are included when running tests on classes.</summary>
    [ConfigurableOption]
    public bool IncludeInternals { get; init; } = true;

    /// <summary>Common suffix attached to class names to name the test classes.</summary>
    [ConfigurableOption]
    public ImmutableArray<string> TestClassNameSuffixes { get; init; } = ["Tests", "TestBase"];

    /// <summary>Possible strings replacing generics in a type name for coverage tests.</summary>
    [ConfigurableOption]
    public ImmutableArray<string> TestClassNameGenericSubstitutes { get; init; } = ["", "_T_"];

    /// <summary>Method used to convert parameters to a test name.</summary>
    public Func<object?, string> TestDisplayNameConverter { get; init; } = o => o?.ToString() ?? "";

    /// <summary>Types to ignore for test class coverage tests.</summary>
    [ConfigurableOption]
    public FrozenSet<string> TestClassCoverageExceptions { get; init; } = [];

    /// <summary>Allowed starts for test method names.</summary>
    [ConfigurableOption]
    public FrozenSet<string> TestMethodNameAllowedTargets { get; init; } = [];

    /// <summary>Names of methods to skip when running tests on classes.</summary>
    [ConfigurableOption]
    public FrozenSet<string> MethodsToIgnore { get; init; } =
        FrozenSet.ToFrozenSet(["Finalize", "Dispose", "DisposeAsync", "PrintMembers"]);

    /// <summary>If all inner exceptions are ignored when running tests on classes.</summary>
    [ConfigurableOption]
    public bool IgnoreAllExceptions { get; init; } = false;

    /// <summary>Exceptions that are safe to ignore when running tests on classes.</summary>
    public FrozenSet<Type> IgnorableExceptions { get; init; } = FrozenSet.ToFrozenSet<Type>([]);

    /// <summary>If all PreventsNullRefException tests immediately pass instead.</summary>
    [ConfigurableOption]
    public bool DisableNullRefExceptionTests { get; init; } = false;

    /// <summary>If all PreventsParameterMutation tests immediately pass instead.</summary>
    [ConfigurableOption]
    public bool DisableParameterMutationTests { get; init; } = true;

    /// <summary>If all PassthroughWithNoExceptions tests immediately pass instead.</summary>
    [ConfigurableOption]
    public bool DisablePassthroughTests { get; init; } = false;

    /// <summary><see cref="Type"/>s to be excluded for the framework integrity test.</summary>
    public FrozenSet<Type> IntegrityIgnorableTypes { get; init; } =
        FrozenSet.ToFrozenSet([typeof(IConfiguration), typeof(IConfigurationSection)]);

    /// <summary>
    ///     Creates options from <see langword="this"/>
    ///     overridden with values from <paramref name="config"/>.
    /// </summary>
    /// <param name="config">Configuration with overrides to use.</param>
    /// <returns>The created options.</returns>
    internal TesterOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(nameof(Tester));
        if (section == null)
        {
            return this;
        }

        return this with
        {
            DisableParameterMutationTests = Config.GetValue(section, DisableParameterMutationTests),
            DisableNullRefExceptionTests = Config.GetValue(section, DisableNullRefExceptionTests),
            TestMethodNameAllowedTargets = Config.GetSet(section, TestMethodNameAllowedTargets),
            TestClassCoverageExceptions = Config.GetSet(section, TestClassCoverageExceptions),
            TestDisplayNameConverter = Config.GetValue(section, TestDisplayNameConverter),
            DisablePassthroughTests = Config.GetValue(section, DisablePassthroughTests),
            IncludeInstanceMethods = Config.GetValue(section, IncludeInstanceMethods),
            TestClassNameSuffixes = Config.GetArray(section, TestClassNameSuffixes),
            IncludeStaticMethods = Config.GetValue(section, IncludeStaticMethods),
            IgnoreAllExceptions = Config.GetValue(section, IgnoreAllExceptions),
            IncludeConstructors = Config.GetValue(section, IncludeConstructors),
            IncludeInternals = Config.GetValue(section, IncludeInternals),
            InjectionValues = Config.GetValue(section, InjectionValues),
            MethodsToIgnore = Config.GetSet(section, MethodsToIgnore),
            Limiter = Config.GetValue(section, Limiter),
            TestClassNameGenericSubstitutes = Config.GetArray(
                section,
                TestClassNameGenericSubstitutes
            ),
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(TesterOptions);
    }
}
