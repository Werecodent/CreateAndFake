using CreateAndFake.Design.Randomization;

namespace CreateAndFake.Design.Tooling;

/// <summary>Configuration for manipulating <see cref="ITool{T}"/> behavior.</summary>
public interface IToolOptions
{
    /// <summary>Value generator used for base randomization.</summary>
    /// <remarks>Provides access to the <see cref="IRandom.InitialSeed"/>.</remarks>
    IRandom Gen { get; init; }
}
