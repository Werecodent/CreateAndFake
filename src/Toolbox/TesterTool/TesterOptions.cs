using System.Collections.Frozen;
using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.AsserterTool;
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

    /// <summary>Values to inject into called methods.</summary>
    /// <remarks>
    ///     When generating randomized parameter data, these values will be used instead of randomized instances
    ///     when applicable. Per method, each value will only be used once and prioritized in the order given.
    ///     See: <seealso cref="IRandomizer.Inject"/>
    /// </remarks>
    /// <value><c>[]</c></value>
    public ImmutableArray<object?> InjectionValues { get; init; } = [];

    /// <summary>If constructors are included when running tests on classes.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeConstructors { get; init; } = true;

    /// <summary>If <see langword="class"/> methods are included when running tests on classes.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeInstanceMethods { get; init; } = true;

    /// <summary>If <see langword="static"/> methods are included when running tests on classes.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeStaticMethods { get; init; } = true;

    /// <summary>If <see langword="internal"/> members are included when running tests on classes.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeInternals { get; init; } = true;

    /// <summary>If only methods directly declared on the <see cref="Type"/> (not inherited) are tested.</summary>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool OnlyDeclaredMethods { get; init; } = false;

    /// <summary>If empty series should throw when canceled before iteration.</summary>
    /// <remarks>A preference for how <see langword="async"/> methods should be implemented.</remarks>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool ThrowsUponCancelWithEmptySeries { get; init; } = true;

    /// <summary>Common suffixes attached to class names used to name the test classes.</summary>
    /// <remarks>Used to correlate classes with their test classes for coverage tests.</remarks>
    /// <value><c>["Tests", "TestBase"]</c></value>
    [ConfigurableOption]
    public ImmutableArray<string> TestClassNameSuffixes { get; init; } = ["Tests", "TestBase"];

    /// <summary>Possible <see cref="string"/>s replacing generics in a <see cref="Type"/>'s name.</summary>
    /// <remarks>Used to help properly correlate classes with their test classes for coverage tests.</remarks>
    /// <value><c>["", "_T_"]</c></value>
    [ConfigurableOption]
    public ImmutableArray<string> TestClassNameGenericSubstitutes { get; init; } = ["", "_T_"];

    /// <summary>Names of the <see cref="Type"/>s to ignore for coverage tests.</summary>
    /// <value><c>[]</c></value>
    [ConfigurableOption]
    public FrozenSet<string> ClassCoverageExclusions { get; init; } = [];

    /// <summary>Allowed targets for test method names.</summary>
    /// <remarks>These prefixes are in addition to the normally allowed test names.</remarks>
    /// <value><c>[]</c></value>
    [ConfigurableOption]
    public FrozenSet<string> AllowedTestNamePrefixes { get; init; } = [];

    /// <summary>Names of methods to skip when running tests on classes.</summary>
    /// <remarks>
    ///     Useful for preventing execution of methods that should be tested
    ///     manually or leave an instance in a bad state for other methods.
    /// </remarks>
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

    /// <summary>If all thrown <see cref="Exception"/>s are ignored when running tests on classes.</summary>
    /// <remarks>Alternative to specifying them individually via <see cref="IgnorableExceptions"/>.</remarks>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool IgnoreAllExceptions { get; init; } = false;

    /// <summary>Exceptions that are safe to ignore when running tests on classes.</summary>
    /// <remarks>Ignored when <see cref="IgnoreAllExceptions"/> is set to <see langword="true"/>.</remarks>
    /// <value><c>[]</c></value>
    [ConfigurableOption]
    public FrozenSet<Type> IgnorableExceptions { get; init; } = FrozenSet.ToFrozenSet<Type>([]);

    /// <summary>Flag for disabling PreventsNullRefException tests.</summary>
    /// <remarks>Useful for debugging. Causes the tests to immediately pass instead of running.</remarks>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool DisableNullRefExceptionTests { get; init; } = false;

    /// <summary>Flag for disabling PreventsParameterMutation tests.</summary>
    /// <remarks>Useful for debugging. Causes the tests to immediately pass instead of running.</remarks>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool DisableParameterMutationTests { get; init; } = false;

    /// <summary>Flag for disabling PassthroughWithNoExceptions tests.</summary>
    /// <remarks>Useful for debugging. Causes the tests to immediately pass instead of running.</remarks>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool DisablePassthroughTests { get; init; } = false;

    /// <summary><see cref="Type"/>s to be excluded for VerifyToolSetIntegrity tests.</summary>
    /// <remarks>Useful for ignoring known incompatible <see cref="Type"/>s.</remarks>
    /// <value><c>[typeof(IConfiguration), typeof(IConfigurationSection)]</c></value>
    [ConfigurableOption]
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
            DisablePassthroughTests = Config.GetValue(section, DisablePassthroughTests),
            IntegrityIgnorableTypes = Config.GetTypes(section, IntegrityIgnorableTypes),
            AllowedTestNamePrefixes = Config.GetSet(section, AllowedTestNamePrefixes),
            ClassCoverageExclusions = Config.GetSet(section, ClassCoverageExclusions),
            IncludeInstanceMethods = Config.GetValue(section, IncludeInstanceMethods),
            TestClassNameSuffixes = Config.GetArray(section, TestClassNameSuffixes),
            IncludeStaticMethods = Config.GetValue(section, IncludeStaticMethods),
            IgnoreAllExceptions = Config.GetValue(section, IgnoreAllExceptions),
            IncludeConstructors = Config.GetValue(section, IncludeConstructors),
            OnlyDeclaredMethods = Config.GetValue(section, OnlyDeclaredMethods),
            IgnorableExceptions = Config.GetTypes(section, IgnorableExceptions),
            IncludeInternals = Config.GetValue(section, IncludeInternals),
            InjectionValues = Config.GetValue(section, InjectionValues),
            MethodsToIgnore = Config.GetSet(section, MethodsToIgnore),
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
