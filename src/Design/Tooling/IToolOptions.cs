using Werecodent.CreateAndFake.Design.Randomization;

namespace Werecodent.CreateAndFake.Design.Tooling;

/// <summary>Settings for manipulating <see cref="ITool{T}"/> behavior.</summary>
public interface IToolOptions
{
    /// <inheritdoc cref="DesignOptions"/>
    /// <remarks>These options are shared among all tools from a configured toolset.</remarks>
    DesignOptions Design { get; init; }

    /// <summary>Value generator used for base randomization.</summary>
    /// <remarks>Provides access to the <see cref="IRandom.InitialSeed"/>.</remarks>
    IRandom Gen { get; init; }
}
