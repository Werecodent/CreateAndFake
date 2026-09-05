using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.FakerTool.Hints;

namespace Werecodent.CreateAndFake.FakerTool.Engine;

/// <summary>Priorities for <see cref="IFakeHint"/>s.</summary>
public enum FakePriority
{
    /// <summary>Priority for hints that won't automatically execute.</summary>
    /// <remarks>Such hints only work if given to the tool via <see cref="IToolOptions"/>.</remarks>
    Disabled = int.MinValue,

    /// <summary>Priority for custom hints that'll execute last.</summary>
    /// <remarks>Subtract from this priority for even lower priorities.</remarks>
    None = 0,

    /// <summary>Priority for <see cref="ObjectFakeHint"/>.</summary>
    ObjectHint = 1,

    /// <summary>Priority for <see cref="AsyncDisposableFakeHint"/>.</summary>
    AsyncDisposableHint = 2,

    /// <summary>Priority for <see cref="DisposableFakeHint"/>.</summary>
    DisposableHint = 3,

    /// <summary>Starting priority for custom hints that'll execute first.</summary>
    /// <remarks>Add to this priority for even higher priorities.</remarks>
    Highest = 4,
}
