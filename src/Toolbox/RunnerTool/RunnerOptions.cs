using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool;

namespace CreateAndFake.RunnerTool;

/// <summary>Configuration for controlling run behavior.</summary>
public record RunnerOptions : IToolOptions
{
    /// <summary>Handles randomization.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Handles object variance.</summary>
    public required IMutator Mutator { get; init; }

    /// <summary>Handles cloning.</summary>
    public required IDuplicator Duplicator { get; init; }

    /// <summary>Provides stubs.</summary>
    public required IFaker Faker { get; init; }

    /// <summary>Attaches <see cref="IReflectableType"/> when faking <see cref="Type"/>s.</summary>
    public bool InheritIReflectableTypeOnFakedType { get; init; } = false;
}