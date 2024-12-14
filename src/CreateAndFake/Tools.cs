using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.DuplicatorTool;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.MutatorTool;
using CreateAndFake.Toolbox.RandomizerTool;
using CreateAndFake.Toolbox.TesterTool;
using CreateAndFake.Toolbox.ValuerTool;

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

    /// <inheritdoc cref="ToolSet.Mutator"/>
    public static IMutator Mutator => Source.Mutator;

    /// <inheritdoc cref="ToolSet.Asserter"/>
    public static IAsserter Asserter => Source.Asserter;

    /// <inheritdoc cref="ToolSet.Duplicator"/>
    public static IDuplicator Duplicator => Source.Duplicator;

    /// <inheritdoc cref="ToolSet.Tester"/>
    public static Tester Tester => Source.Tester;
}
