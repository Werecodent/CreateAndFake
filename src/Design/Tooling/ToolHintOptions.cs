using System.Collections.Immutable;

namespace CreateAndFake.Design.Tooling;

/// <inheritdoc/>
public abstract record ToolHintOptions<TSelf, THint> : IToolHintOptions<TSelf, THint>
    where TSelf : IToolHintOptions<TSelf, THint>
    where THint : IToolHint
{
    /// <inheritdoc/>
    public bool IncludeDefaultHints { get; init; } = true;

    /// <inheritdoc/>
    public ImmutableArray<THint> Hints { get; init; } = [];

    /// <inheritdoc/>
    public TSelf? NestedOptions { get; init; } = default;
}
