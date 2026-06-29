using CreateAndFake.Design.Tooling;

namespace CreateAndFake.FakerTool.Engine;

/// <summary>Priorities for <see cref="IFakeHint"/>s.</summary>
public enum FakePriority
{
    /// <summary>Priority for hints that won't automatically execute.</summary>
    /// <remarks>Such hints only work if given to the tool via <see cref="IToolOptions"/>.</remarks>
    Disabled = int.MinValue,

    /// <summary>Priority for custom hints that'll execute last.</summary>
    /// <remarks>Subtract from this priority for even lower priorities.</remarks>
    None = 0,

    /// <summary>Starting priority for custom hints that'll execute first.</summary>
    /// <remarks>Add to this priority for even higher priorities.</remarks>
    Highest = 1,
}
