using CreateAndFake.Design.Tooling;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.RandomizerTool.Engine;

/// <summary>Priorities for <see cref="CreateHint"/>s.</summary>
public enum CreatePriority
{
    /// <summary>Priority for hints that won't automatically execute.</summary>
    /// <remarks>Such hints only work if given to the tool via <see cref="IToolOptions"/>.</remarks>
    Disabled = int.MinValue,

    /// <summary>Priority for custom hints that'll execute last.</summary>
    /// <remarks>Subtract from this priority for even lower priorities.</remarks>
    None = 0,

    /// <summary>Priority for <see cref="ObjectCreateHint"/>.</summary>
    ObjectHint = 1,

    /// <summary>Priority for <see cref="SubclassCreateHint"/>.</summary>
    SubclassHint = 2,

    /// <summary>Priority for <see cref="FakeCreateHint"/>.</summary>
    FakeHint = 3,

    /// <summary>Priority for <see cref="InjectedCreateHint"/>.</summary>
    InjectedHint = 4,

    /// <summary>Priority for <see cref="TaskCreateHint"/>.</summary>
    TaskHint = 5,

    /// <summary>Priority for <see cref="DelegateCreateHint"/>.</summary>
    DelegateHint = 6,

    /// <summary>Priority for <see cref="SpanCreateHint"/>.</summary>
    SpanHint = 7,

    /// <summary>Priority for <see cref="LegacyCollectionCreateHint"/>.</summary>
    LegacyCollectionHint = 8,

    /// <summary>Priority for <see cref="CollectionCreateHint"/>.</summary>
    CollectionHint = 9,

    /// <summary>Priority for <see cref="AsyncCollectionCreateHint"/>.</summary>
    AsyncCollectionHint = 10,

    /// <summary>Priority for <see cref="GenericCreateHint"/>.</summary>
    GenericHint = 11,

    /// <summary>Priority for <see cref="EnumCreateHint"/>.</summary>
    EnumHint = 12,

    /// <summary>Priority for <see cref="HandlerCreateHint"/>.</summary>
    HandlerHint = 13,

    /// <summary>Starting priority for custom hints that'll execute first.</summary>
    /// <remarks>Add to this priority for even higher priorities.</remarks>
    Highest = 14,
}
