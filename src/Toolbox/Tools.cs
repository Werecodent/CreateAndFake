using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Randomization;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake;

/// <summary>Manages implementations of all reflection tools.</summary>
public static class Tools
{
    /// <summary>Manages currently used tools globally.</summary>
    /// <remarks>Should only be modified in module initializer once.</remarks>
    public static ToolSet Source { get; set; } = ToolSet.DefaultSet;

    /// <inheritdoc cref="ToolSet.Gen"/>
    public static IRandom Gen => Source.Gen;

    /// <inheritdoc cref="ToolSet.Valuer"/>
    public static IValuer Valuer => Source.Valuer;

    /// <inheritdoc cref="ToolSet.Faker"/>
    public static IFaker Faker => Source.Faker;

    /// <inheritdoc cref="ToolSet.Randomizer"/>
    public static IRandomizer Randomizer => Source.Randomizer;

    /// <inheritdoc cref="ToolSet.Extractor"/>
    public static IExtractor Extractor => Source.Extractor;

    /// <inheritdoc cref="ToolSet.Mutator"/>
    public static IMutator Mutator => Source.Mutator;

    /// <inheritdoc cref="ToolSet.Asserter"/>
    public static IAsserter Asserter => Source.Asserter;

    /// <inheritdoc cref="ToolSet.Duplicator"/>
    public static IDuplicator Duplicator => Source.Duplicator;

    /// <inheritdoc cref="ToolSet.Runner"/>
    public static IRunner Runner => Source.Runner;

    /// <inheritdoc cref="ToolSet.Tester"/>
    public static ITester Tester => Source.Tester;
}
