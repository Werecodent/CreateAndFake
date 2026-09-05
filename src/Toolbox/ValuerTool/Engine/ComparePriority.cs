using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.ValuerTool.Engine;

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

    /// <summary>Priority for <see cref="StructuralEquatableHint"/>.</summary>
    StructuralEquatableHint = 9,

    /// <summary>Priority for <see cref="EnumerableCompareHint"/>.</summary>
    EnumerableHint = 10,

    /// <summary>Priority for <see cref="SetCompareHint"/>.</summary>
    SetHint = 11,

    /// <summary>Priority for <see cref="DictionaryCompareHint"/>.</summary>
    DictionaryHint = 12,

    /// <summary>Priority for <see cref="FakedCompareHint"/>.</summary>
    FakedHint = 13,

    /// <summary>Priority for <see cref="DefaultEqualityCompareHint"/>.</summary>
    DefaultEqualityHint = 14,

    /// <summary>Priority for <see cref="AsyncEnumerableCompareHint"/>.</summary>
    AsyncEnumerableHint = 15,

    /// <summary>Priority for <see cref="ValueTaskCompareHint"/>.</summary>
    ValueTaskHint = 16,

    /// <summary>Priority for <see cref="TaskCompareHint"/>.</summary>
    TaskHint = 17,

    /// <summary>Priority for <see cref="HandlerCompareHint"/>.</summary>
    HandlerHint = 18,

    /// <summary>Starting priority for custom hints that'll execute first.</summary>
    /// <remarks>Add to this priority for even higher priorities.</remarks>
    Highest = 19,
}
