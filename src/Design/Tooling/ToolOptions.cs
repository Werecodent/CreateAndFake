using Werecodent.CreateAndFake.Design.Randomization;

namespace Werecodent.CreateAndFake.Design.Tooling;

/// <inheritdoc cref="IToolOptions"/>
public abstract record ToolOptions<TSelf> : IToolOptions
    where TSelf : ToolOptions<TSelf>
{
    /// <inheritdoc/>
    public required DesignOptions Design { get; init; }

    /// <inheritdoc/>
    public required IRandom Gen { get; init; }
}
