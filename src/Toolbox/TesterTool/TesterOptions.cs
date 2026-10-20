using System.Collections.Frozen;
using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.RandomizerTool;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.TesterTool;

/// <summary>Configuration for manipulating <see cref="ITester"/> behavior.</summary>
public sealed record TesterOptions : ToolOptions<TesterOptions>
{
    /// <summary>Identifies the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = nameof(Tester);

    /// <inheritdoc cref="IRandomizer"/>
    public required IRandomizer Randomizer { get; init; }

    /// <inheritdoc cref="IDuplicator"/>
    public required IDuplicator Duplicator { get; init; }

    /// <inheritdoc cref="IExtractor"/>
    public required IExtractor Extractor { get; init; }

    /// <inheritdoc cref="IFaker"/>
    public required IFaker Faker { get; init; }

    /// <inheritdoc cref="IMutator"/>
    public required IMutator Mutator { get; init; }

    /// <inheritdoc cref="IAsserter"/>
    public required IAsserter Asserter { get; init; }

    /// <inheritdoc cref="IRunner"/>
    public required IRunner Runner { get; init; }

    /// <inheritdoc cref="IValuer"/>
    public required IValuer Valuer { get; init; }

    /// <summary>Retries tests if timeout is reached.</summary>
    /// <value><c>Limiter.Few</c></value>
    [ConfigurableOption]
    public Limiter Limiter { get; init; } = Limiter.Few;

    /// <summary>Values to inject into called methods.</summary>
    /// <value><c>[]</c></value>
    public ImmutableArray<object?> InjectionValues { get; init; } = [];

    /// <summary>If constructors are included when running tests on classes.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeConstructors { get; init; } = true;

    /// <summary>If class methods are included when running tests on classes.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeInstanceMethods { get; init; } = true;

    /// <summary>If static methods are included when running tests on classes.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeStaticMethods { get; init; } = true;

    /// <summary>If internal members are included when running tests on classes.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeInternals { get; init; } = true;

    /// <summary>If only methods directly declared on the type (not inherited) are tested.</summary>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool OnlyDeclaredMethods { get; init; } = false;

    /// <summary>If empty series should throw when canceled before iteration.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool ThrowsUponCancelWithEmptySeries { get; init; } = true;

    /// <summary>Common suffix attached to class names to name the test classes.</summary>
    /// <value><c>["Tests", "TestBase"]</c></value>
    [ConfigurableOption]
    public ImmutableArray<string> TestClassNameSuffixes { get; init; } = ["Tests", "TestBase"];

    /// <summary>Possible strings replacing generics in a type name for coverage tests.</summary>
    /// <value><c>["", "_T_"]</c></value>
    [ConfigurableOption]
    public ImmutableArray<string> TestClassNameGenericSubstitutes { get; init; } = ["", "_T_"];

    /// <summary>Types to ignore for test class coverage tests.</summary>
    /// <value><c>[]</c></value>
    [ConfigurableOption]
    public FrozenSet<string> TestClassCoverageExceptions { get; init; } = [];

    /// <summary>Allowed starts for test method names.</summary>
    /// <value><c>[]</c></value>
    [ConfigurableOption]
    public FrozenSet<string> TestMethodNameAllowedTargets { get; init; } = [];

    /// <summary>Names of methods to skip when running tests on classes.</summary>
    /// <value><c>["Finalize", "Dispose", "DisposeAsync", "PrintMembers", "GetObjectData"]</c></value>
    [ConfigurableOption]
    public FrozenSet<string> MethodsToIgnore { get; init; } =
        FrozenSet.ToFrozenSet([
            "Finalize",
            "Dispose",
            "DisposeAsync",
            "PrintMembers",
            "GetObjectData",
        ]);

    /// <summary>If all inner exceptions are ignored when running tests on classes.</summary>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool IgnoreAllExceptions { get; init; } = false;

    /// <summary>Exceptions that are safe to ignore when running tests on classes.</summary>
    /// <value><c>[]</c></value>
    public FrozenSet<Type> IgnorableExceptions { get; init; } = FrozenSet.ToFrozenSet<Type>([]);

    /// <summary>If all PreventsNullRefException tests immediately pass instead.</summary>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool DisableNullRefExceptionTests { get; init; } = false;

    /// <summary>If all PreventsParameterMutation tests immediately pass instead.</summary>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool DisableParameterMutationTests { get; init; } = false;

    /// <summary>If all PassthroughWithNoExceptions tests immediately pass instead.</summary>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool DisablePassthroughTests { get; init; } = false;

    /// <summary><see cref="Type"/>s to be excluded for the framework integrity test.</summary>
    /// <value><c>[]</c></value>
    public FrozenSet<Type> IntegrityIgnorableTypes { get; init; } = FrozenSet.ToFrozenSet<Type>([]);

    /// <summary>Derives options from <see langword="this"/> overridden by <paramref name="config"/>'s values.</summary>
    /// <param name="config">Configuration with the potential overrides to use.</param>
    /// <returns>The created options.</returns>
    internal TesterOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
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
            DisablePassthroughTests = Config.GetValue(section, DisablePassthroughTests),
            IncludeInstanceMethods = Config.GetValue(section, IncludeInstanceMethods),
            TestClassNameSuffixes = Config.GetArray(section, TestClassNameSuffixes),
            IncludeStaticMethods = Config.GetValue(section, IncludeStaticMethods),
            IgnoreAllExceptions = Config.GetValue(section, IgnoreAllExceptions),
            IncludeConstructors = Config.GetValue(section, IncludeConstructors),
            OnlyDeclaredMethods = Config.GetValue(section, OnlyDeclaredMethods),
            IncludeInternals = Config.GetValue(section, IncludeInternals),
            InjectionValues = Config.GetValue(section, InjectionValues),
            MethodsToIgnore = Config.GetSet(section, MethodsToIgnore),
            Limiter = Config.GetValue(section, Limiter),
            ThrowsUponCancelWithEmptySeries = Config.GetValue(
                section,
                ThrowsUponCancelWithEmptySeries
            ),
            TestClassNameGenericSubstitutes = Config.GetArray(
                section,
                TestClassNameGenericSubstitutes
            ),
        };
    }
}
