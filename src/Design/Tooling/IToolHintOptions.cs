using System.Collections.Immutable;

namespace Werecodent.CreateAndFake.Design.Tooling;

/// <summary>Settings for manipulating <see cref="IHintTool{T,T}"/> behavior.</summary>
/// <typeparam name="TSelf">Self-reference <see cref="Type"/>.</typeparam>
/// <typeparam name="THint">Hint <see cref="Type"/> used by the associated Tool.</typeparam>
public interface IToolHintOptions<TSelf, THint> : IToolOptions
    where TSelf : IToolHintOptions<TSelf, THint>
    where THint : IToolHint
{
    /// <summary>
    ///     Maximum number of nested tool method calls from <typeparamref name="THint"/>s allowed before failing.
    /// </summary>
    /// <remarks>Helps prevent infinite loops from custom <typeparamref name="THint"/>s.</remarks>
    [ConfigurableOption]
    int MaxRecursionDepth { get; }

    /// <summary>If the default framework <typeparamref name="THint"/>s are enabled.</summary>
    /// <remarks>
    ///     Useful for testing custom <typeparamref name="THint"/>s. Associated Tool behavior will
    ///     completely rely upon custom <typeparamref name="THint"/>s if set to <see langword="false"/>.
    /// </remarks>
    [ConfigurableOption]
    bool IncludeFrameworkHints { get; }

    /// <summary>If found custom <typeparamref name="THint"/>s are automatically enabled.</summary>
    /// <remarks>
    ///     Useful for reverting custom <typeparamref name="THint"/> behavior in the associated Tool.
    ///     The <typeparamref name="THint"/>s declared are used automatically unless set to <see langword="false"/>.
    /// </remarks>
    [ConfigurableOption]
    bool IncludeFoundHints { get; }

    /// <summary>Additional hints to be used by the tool.</summary>
    /// <remarks>These hints are used in the order given before any other hint.</remarks>
    ImmutableArray<THint> Hints { get; }

    /// <summary>Options to use when working on child values during recursion.</summary>
    /// <remarks>If <see langword="null"/>, the current options are utilized.</remarks>
    TSelf? NestedOptions { get; }
}
