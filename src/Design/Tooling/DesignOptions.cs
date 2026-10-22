using Werecodent.CreateAndFake.Design.Properties;
using Werecodent.CreateAndFake.Design.Randomization;

namespace Werecodent.CreateAndFake.Design.Tooling;

/// <summary>Settings for manipulating shared tool behavior.</summary>
public record DesignOptions
{
    /// <summary>First seed used to begin generating values for seeded randomization.</summary>
    /// <remarks>
    ///     Useful for recreating the same sequence of values from randomization (beware parallelism). If not set,
    ///     the <see cref="IRandom.InitialSeed"/> on <see cref="IToolOptions.Gen"/> will be a generated value instead.
    /// </remarks>
    /// <value><see langword="null"/></value>
    [ConfigurableOption]
    public int? Seed { get; init; } = null;

    /// <inheritdoc cref="DesignDefaults.IterationLimit"/>
    [ConfigurableOption]
    public int IterationLimit { get; init; } = DesignDefaults.IterationLimit;

    /// <inheritdoc cref="DesignDefaults.IncludeInfinityAndNaNGeneration"/>
    [ConfigurableOption]
    public bool IncludeInfinityAndNaNGeneration { get; init; } =
        DesignDefaults.IncludeInfinityAndNaNGeneration;
}
