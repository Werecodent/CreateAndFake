using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ExtractorTool.Hints;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <summary>Priorities for <see cref="ExtractHint"/>s.</summary>
public enum ExtractPriority
{
    /// <summary>Priority for hints that won't automatically execute.</summary>
    /// <remarks>Such hints only work if given to the tool via <see cref="IToolOptions"/>.</remarks>
    Disabled = int.MinValue,

    /// <summary>Priority for custom hints that'll execute last.</summary>
    /// <remarks>Subtract from this priority for even lower priorities.</remarks>
    None = 0,

    /// <summary>Priority for <see cref="ObjectExtractHint"/>.</summary>
    ObjectHint = 1,

    /// <summary>Priority for <see cref="StreamExtractHint"/>.</summary>
    StreamHint = 2,

    /// <summary>Priority for <see cref="TaskExtractHint"/>.</summary>
    TaskHint = 3,

    /// <summary>Priority for <see cref="DelegateExtractHint"/>.</summary>
    DelegateHint = 4,

    /// <summary>Priority for <see cref="EnumerableExtractHint"/>.</summary>
    EnumerableHint = 5,

    /// <summary>Priority for <see cref="DictionaryExtractHint"/>.</summary>
    DictionaryHint = 6,

    /// <summary>Priority for <see cref="EndingExtractHint"/>.</summary>
    EndingHint = 7,

    /// <summary>Priority for <see cref="HandlerExtractHint"/>.</summary>
    HandlerHint = 8,

    /// <summary>Starting priority for custom hints that'll execute first.</summary>
    /// <remarks>Add to this priority for even higher priorities.</remarks>
    Highest = 9,
}
