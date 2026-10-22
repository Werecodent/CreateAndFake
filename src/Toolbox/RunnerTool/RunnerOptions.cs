using System.Collections.Frozen;
using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.RandomizerTool;

namespace Werecodent.CreateAndFake.RunnerTool;

/// <summary>Configuration for manipulating <see cref="IRunner"/> behavior.</summary>
public sealed record RunnerOptions : ToolOptions<RunnerOptions>
{
    /// <summary>Identifies the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = nameof(Runner);

    /// <inheritdoc cref="IRandomizer"/>
    public required IRandomizer Randomizer { get; init; }

    /// <inheritdoc cref="IMutator"/>
    public required IMutator Mutator { get; init; }

    /// <inheritdoc cref="IDuplicator"/>
    public required IDuplicator Duplicator { get; init; }

    /// <inheritdoc cref="IFaker"/>
    public required IFaker Faker { get; init; }

    /// <summary>Names of methods to skip when running all methods on an instance.</summary>
    /// <remarks>
    ///     Useful for preventing execution of methods that leave
    ///     the <see cref="object"/> in a bad state for other methods.
    /// </remarks>
    /// <value><c>["Finalize", "Dispose", "DisposeAsync"]</c></value>
    [ConfigurableOption]
    public FrozenSet<string> MethodsToIgnore { get; init; } =
        FrozenSet.ToFrozenSet(["Finalize", "Dispose", "DisposeAsync"]);

    /// <summary>Flag to include running methods declared on the <see cref="object"/> <see langword="class"/>.</summary>
    /// <remarks>Useful in only running introduced methods when <see langword="false"/>.</remarks>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeBaseObjectMethods { get; init; } = false;

    /// <summary>How long to wait for methods to complete before timing out.</summary>
    /// <remarks>Used to gracefully end execution instead of stalling endlessly.</remarks>
    /// <value><c>00:00:50.0000000</c></value>
    [ConfigurableOption]
    public TimeSpan RunTimeout { get; init; } = new(0, 0, 50);

    /// <summary>Values to inject into called methods.</summary>
    /// <remarks>
    ///     When generating randomized parameter data, these values will be used instead of randomized instances
    ///     when applicable. Per method, each value will only be used once and prioritized in the order given.
    ///     See: <seealso cref="IRandomizer.Inject"/>
    /// </remarks>
    /// <value><c>[]</c></value>
    public ImmutableArray<object?> InjectionValues { get; init; } = [];

    /// <summary>Derives options from <see langword="this"/> overridden by <paramref name="config"/>'s values.</summary>
    /// <param name="config">Configuration with the potential overrides to use.</param>
    /// <returns>The created options.</returns>
    internal RunnerOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
        if (section == null)
        {
            return this;
        }

        return this with
        {
            IncludeBaseObjectMethods = Config.GetValue(section, IncludeBaseObjectMethods),
            MethodsToIgnore = Config.GetSet(section, MethodsToIgnore),
            RunTimeout = Config.GetValue(section, RunTimeout),
        };
    }
}
