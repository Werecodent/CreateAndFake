using Werecodent.CreateAndFake.Design.Randomization;

namespace Werecodent.CreateAndFake.Design.Tooling;

/// <summary>Settings for manipulating <see cref="ITool{T}"/> behavior.</summary>
public interface IToolOptions
{
    /// <summary>Notes.</summary>
    DesignOptions Design { get; init; }

    /// <summary>Value generator used for base randomization.</summary>
    /// <remarks>Provides access to the <see cref="IRandom.InitialSeed"/>.</remarks>
    IRandom Gen { get; init; }
}
