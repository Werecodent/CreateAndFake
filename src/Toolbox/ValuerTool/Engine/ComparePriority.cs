using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.ValuerTool.Engine;

/// <summary>Priorities for <see cref="CompareHint"/>s.</summary>
public enum ComparePriority
{
    /// <summary>Priority for hints that won't automatically execute.</summary>
    /// <remarks>Such hints only work if given to the tool via <see cref="IToolOptions"/>.</remarks>
    Disabled = int.MinValue,

    /// <summary>Priority for custom hints that'll execute last.</summary>
    /// <remarks>Subtract from this priority for even lower priorities.</remarks>
    None = 0,

    /// <summary>Priority for <see cref="StatelessCompareHint"/>.</summary>
    StatelessHint = 1,

    /// <summary>Priority for <see cref="PrivateObjectCompareHint"/>.</summary>
    PrivateObjectHint = 2,

    /// <summary>Priority for <see cref="PublicObjectCompareHint"/>.</summary>
    PublicObjectHint = 3,

    /// <summary>Priority for <see cref="EquatableCompareHint"/>.</summary>
    EquatableHint = 4,

    /// <summary>Priority for <see cref="ValuerEquatableCompareHint"/>.</summary>
    ValuerEquatableHint = 5,

    /// <summary>Priority for <see cref="ValuerComparableCompareHint"/>.</summary>
    ValuerComparableHint = 6,

    /// <summary>Priority for <see cref="ValuerAsyncComparableCompareHint"/>.</summary>
    ValuerAsyncComparableHint = 7,

    /// <summary>Priority for <see cref="ValueEquatableCompareHint"/>.</summary>
    ValueEquatableHint = 8,

    /// <summary>Priority for <see cref="EnumerableCompareHint"/>.</summary>
    EnumerableHint = 9,

    /// <summary>Priority for <see cref="DictionaryCompareHint"/>.</summary>
    DictionaryHint = 10,

    /// <summary>Priority for <see cref="FakedCompareHint"/>.</summary>
    FakedHint = 11,

    /// <summary>Priority for <see cref="DefaultEqualityCompareHint"/>.</summary>
    DefaultEqualityHint = 12,

    /// <summary>Priority for <see cref="AsyncEnumerableCompareHint"/>.</summary>
    AsyncEnumerableHint = 13,

    /// <summary>Priority for <see cref="TaskCompareHint"/>.</summary>
    TaskHint = 14,

    /// <summary>Priority for <see cref="HandlerCompareHint"/>.</summary>
    HandlerHint = 15,

    /// <summary>Starting priority for custom hints that'll execute first.</summary>
    /// <remarks>Add to this priority for even higher priorities.</remarks>
    Highest = 16,
}
