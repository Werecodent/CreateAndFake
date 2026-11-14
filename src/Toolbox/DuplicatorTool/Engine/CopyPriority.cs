using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;

namespace Werecodent.CreateAndFake.DuplicatorTool.Engine;

/// <summary>Priorities for <see cref="CopyHint"/>s.</summary>
public enum CopyPriority
{
    /// <summary>Priority for hints that won't automatically execute.</summary>
    /// <remarks>Such hints only work if given to the tool via <see cref="IToolOptions"/>.</remarks>
    Disabled = int.MinValue,

    /// <summary>Priority for custom hints that'll execute last.</summary>
    /// <remarks>Subtract from this priority for even lower priorities.</remarks>
    None = 0,

    /// <summary>Priority for <see cref="ObjectCopyHint"/>.</summary>
    ObjectHint = 1,

    /// <summary>Priority for <see cref="SerializableCopyHint"/>.</summary>
    SerializableHint = 2,

    /// <summary>Priority for <see cref="CloneableCopyHint"/>.</summary>
    CloneableHint = 3,

    /// <summary>Priority for <see cref="StreamCopyHint"/>.</summary>
    StreamHint = 4,

    /// <summary>Priority for <see cref="CollectionCopyHint"/>.</summary>
    CollectionHint = 5,

    /// <summary>Priority for <see cref="ImmutableCollectionCopyHint"/>.</summary>
    ImmutableCollectionHint = 6,

    /// <summary>Priority for <see cref="FrozenCollectionCopyHint"/>.</summary>
    FrozenCollectionHint = 7,

    /// <summary>Priority for <see cref="AsyncCollectionCopyHint"/>.</summary>
    AsyncCollectionHint = 8,

    /// <summary>Priority for <see cref="AsyncDesignCopyHint"/>.</summary>
    AsyncDesignHint = 9,

    /// <summary>Priority for <see cref="BasicCopyHint"/>.</summary>
    BasicHint = 10,

    /// <summary>Priority for <see cref="DuplicatableCopyHint"/>.</summary>
    DuplicatableHint = 11,

    /// <summary>Priority for <see cref="DeepCloneableCopyHint"/>.</summary>
    DeepCloneableHint = 12,

    /// <summary>Priority for <see cref="ValueTaskCopyHint"/>.</summary>
    ValueTaskHint = 13,

    /// <summary>Priority for <see cref="TaskCopyHint"/>.</summary>
    TaskHint = 14,

    /// <summary>Priority for <see cref="HandlerCopyHint"/>.</summary>
    HandlerHint = 15,

    /// <summary>Starting priority for custom hints that'll execute first.</summary>
    /// <remarks>Add to this priority for even higher priorities.</remarks>
    Highest = 16,
}
