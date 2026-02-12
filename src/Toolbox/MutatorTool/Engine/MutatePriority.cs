using CreateAndFake.Design.Tooling;
using CreateAndFake.MutatorTool.Hints;

namespace CreateAndFake.MutatorTool.Engine;

/// <summary>Priorities for <see cref="IMutateHint"/>s.</summary>
public enum MutatePriority
{
    /// <summary>Priority for hints that won't automatically execute.</summary>
    /// <remarks>Such hints only work if given to the tool via <see cref="IToolOptions"/>.</remarks>
    Disabled = int.MinValue,

    /// <summary>Priority for custom hints that'll execute last.</summary>
    /// <remarks>Subtract from this priority for even lower priorities.</remarks>
    None = 0,

    /// <summary>Priority for <see cref="ImmutableEnumerableMutateHint"/>.</summary>
    ImmutableEnumerableHint = 1,

    /// <summary>Priority for <see cref="LegacyCollectionMutateHint"/>.</summary>
    LegacyCollectionHint = 2,

    /// <summary>Priority for <see cref="CollectionMutateHint"/>.</summary>
    CollectionHint = 3,

    /// <summary>Priority for <see cref="ListMutateHint"/>.</summary>
    ListHint = 4,

    /// <summary>Priority for <see cref="DictionaryMutateHint"/>.</summary>
    DictionaryHint = 5,

    /// <summary>Priority for <see cref="ObjectMutateHint"/>.</summary>
    ObjectHint = 6,

    /// <summary>Priority for <see cref="HandlerMutateHint"/>.</summary>
    HandlerHint = 7,

    /// <summary>Starting priority for custom hints that'll execute first.</summary>
    /// <remarks>Add to this priority for even higher priorities.</remarks>
    Highest = 8,
}
