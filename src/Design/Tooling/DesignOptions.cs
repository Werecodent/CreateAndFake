using Werecodent.CreateAndFake.Design.Properties;
using Werecodent.CreateAndFake.Design.Randomization;

namespace Werecodent.CreateAndFake.Design.Tooling;

/// <summary>Settings for manipulating shared <see cref="ITool{T}"/> behavior.</summary>
public record DesignOptions
{
    /// <inheritdoc cref="IRandom.InitialSeed"/>
    /// <value><see langword="null"/></value>
    [ConfigurableOption]
    public int? Seed { get; init; } = null;

    /// <inheritdoc cref="DesignDefaults.IterationLimit"/>
    /// <value><c>19999</c></value>
    [ConfigurableOption]
    public int IterationLimit { get; init; } = DesignDefaults.IterationLimit;

    /// <summary><inheritdoc cref="DesignDefaults.IncludeInfinityAndNaNGeneration"/></summary>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool IncludeInfinityAndNaNGeneration { get; init; } =
        DesignDefaults.IncludeInfinityAndNaNGeneration;
}
