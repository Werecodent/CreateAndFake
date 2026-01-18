using System.Collections.Immutable;

namespace CreateAndFake.Design.Tooling;

/// <inheritdoc/>
/// <typeparam name="TSelf">Self reference type.</typeparam>
/// <typeparam name="THint">Type used to provide hints for the tool.</typeparam>
public interface IToolHintOptions<TSelf, THint> : IToolOptions
    where TSelf : IToolHintOptions<TSelf, THint>
    where THint : IToolHint
{
    /// <summary>Limits recursion for the chainer.</summary>
    [ConfigurableOption]
    int MaxHintRecursion { get; }

    /// <summary>If the default set of hints should be used by the tool.</summary>
    bool IncludeDefaultHints { get; }

    /// <summary>Custom handlers used for specific types.</summary>
    ImmutableArray<THint> Hints { get; }

    /// <summary>Options to use when working on child values.</summary>
    TSelf? NestedOptions { get; }
}
