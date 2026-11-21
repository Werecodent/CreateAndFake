using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.RandomizerTool;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.TesterTool;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake;

/// <summary>Holds implementations of all reflection tools.</summary>
public interface IToolSet
{
    /// <inheritdoc cref="IRandom"/>
    IRandom Gen { get; }

    /// <inheritdoc cref="IValuer"/>
    IValuer Valuer { get; }

    /// <inheritdoc cref="IFaker"/>
    IFaker Faker { get; }

    /// <inheritdoc cref="IRandomizer"/>
    IRandomizer Randomizer { get; }

    /// <inheritdoc cref="IExtractor"/>
    IExtractor Extractor { get; }

    /// <inheritdoc cref="IMutator"/>
    IMutator Mutator { get; }

    /// <inheritdoc cref="IAsserter"/>
    IAsserter Asserter { get; }

    /// <inheritdoc cref="IDuplicator"/>
    IDuplicator Duplicator { get; }

    /// <inheritdoc cref="IRunner"/>
    IRunner Runner { get; }

    /// <inheritdoc cref="ITester"/>
    ITester Tester { get; }
}
