using System.Collections.Frozen;
using System.Collections.Immutable;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RunnerTool;
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

    /// <summary>Handles common test scenarios.</summary>
    public required IAsserter Asserter { get; init; }

    /// <summary>Handles method generation.</summary>
    public required IRunner Runner { get; init; }

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
    public bool IncludeStaticMethods { get; init; } = true;

    /// <summary>If internal members are included when running tests on classes.</summary>
    [ConfigurableOption]
    public bool IncludeInternals { get; init; } = true;

    /// <summary>Common suffix attached to class names to name the test classes.</summary>
    [ConfigurableOption]
    public string TestClassNameSuffix { get; init; } = "Tests";

    /// <summary>Possible strings replacing generics in a type name for coverage tests.</summary>
    [ConfigurableOption]
    public ImmutableArray<string> TestClassNameGenericSubstitutes { get; init; } = ["", "_T_"];

    /// <summary>Method used to convert parameters to a test name.</summary>
    public Func<object?, string> TestDisplayNameConverter { get; init; } = o => o?.ToString() ?? "";

    /// <summary>Types to ignore for test class coverage tests.</summary>
    [ConfigurableOption]
    public FrozenSet<string> TestClassCoverageExceptions { get; init; } =
        FrozenSet.ToFrozenSet<string>([]);

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
    public bool DisableParameterMutationTests { get; init; } = false;

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
            Limiter = section.GetValue(nameof(Limiter), Limiter),
            InjectionValues = section.GetValue(nameof(InjectionValues), InjectionValues),
            IncludeConstructors = section.GetValue(
                nameof(IncludeConstructors),
                IncludeConstructors
            ),
            IncludeInstanceMethods = section.GetValue(
                nameof(IncludeInstanceMethods),
                IncludeInstanceMethods
            ),
            IncludeStaticMethods = section.GetValue(
                nameof(IncludeStaticMethods),
                IncludeStaticMethods
            ),
            IncludeInternals = section.GetValue(nameof(IncludeInternals), IncludeInternals),
            TestClassNameSuffix = section.GetValue(
                nameof(TestClassNameSuffix),
                TestClassNameSuffix
            ),
            TestClassNameGenericSubstitutes =
                GetSectionList<string>(section, nameof(TestClassNameGenericSubstitutes))
                    ?.ToImmutableArray()
                ?? TestClassNameGenericSubstitutes,
            TestDisplayNameConverter = section.GetValue(
                nameof(TestDisplayNameConverter),
                TestDisplayNameConverter
            ),
            TestClassCoverageExceptions =
                GetSectionList<string>(section, nameof(TestClassCoverageExceptions))?.ToFrozenSet()
                ?? TestClassCoverageExceptions,
            MethodsToIgnore =
                GetSectionList<string>(section, nameof(MethodsToIgnore))?.ToFrozenSet()
                ?? MethodsToIgnore,
            IgnoreAllExceptions = section.GetValue(
                nameof(IgnoreAllExceptions),
                IgnoreAllExceptions
            ),
            DisableNullRefExceptionTests = section.GetValue(
                nameof(DisableNullRefExceptionTests),
                DisableNullRefExceptionTests
            ),
            DisableParameterMutationTests = section.GetValue(
                nameof(DisableParameterMutationTests),
                DisableParameterMutationTests
            ),
        };
    }

    /// <summary>Deserializes a list from the configuration.</summary>
    /// <typeparam name="T">Object type for the list.</typeparam>
    /// <param name="config">Root configuration section for the options.</param>
    /// <param name="sectionName">Name of the subsection representing the list.</param>
    /// <returns>The deserialized list if present, null otherwise.</returns>
    private static List<T>? GetSectionList<T>(IConfigurationSection config, string sectionName)
    {
        IConfigurationSection section = config.GetSection(sectionName);
        if (section.Exists())
        {
            List<T> bindResult = [];
            section.Bind(bindResult);
            return bindResult;
        }
        else
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(TesterOptions);
    }
}
