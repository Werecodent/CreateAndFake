using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design.Tooling;
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

    /// <summary>Option for which methods to include.</summary>
    public bool IncludeFinalize { get; init; } = false;

    /// <summary>Option for which methods to include.</summary>
    public bool IncludeDispose { get; init; } = false;

    /// <summary>Option for which methods to include.</summary>
    public bool IncludeStaticMethods { get; init; } = true;

    /// <summary>Option for which methods to include.</summary>
    public bool IncludeInstanceMethods { get; init; } = true;

    /// <summary>How long to wait for methods to complete.</summary>
    public TimeSpan Timeout { get; init; } = new(0, 0, 20);

    /// <summary>Values to inject into called methods.</summary>
    public ImmutableArray<object?> InjectionValues { get; init; } = [];
}
