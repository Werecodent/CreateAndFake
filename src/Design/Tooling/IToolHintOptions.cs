using System.Collections.Immutable;

namespace Werecodent.CreateAndFake.Design.Tooling;

/// <summary>Configuration for manipulating <see cref="IHintTool{T,T}"/> behavior.</summary>
/// <typeparam name="TSelf">Self-reference <see cref="Type"/>.</typeparam>
/// <typeparam name="THint">Hint <see cref="Type"/> used by the <see cref="IHintTool{T,T}"/>.</typeparam>
public interface IToolHintOptions<TSelf, THint> : IToolOptions
    where TSelf : IToolHintOptions<TSelf, THint>
    where THint : IToolHint
{
    /// <summary>Limits recursion for the tool.</summary>
    /// <remarks>Helps prevent infinite loops from custom <typeparamref name="THint"/>s.</remarks>
    [ConfigurableOption]
    int MaxHintRecursion { get; }

    /// <summary>If the default framework <typeparamref name="THint"/>s are enabled.</summary>
    /// <remarks>
    ///     Tool behavior will rely completely upon custom
    ///     <typeparamref name="THint"/>s if set to <see langword="false"/>.
    /// </remarks>
    [ConfigurableOption]
    bool IncludeFrameworkHints { get; }

    /// <summary>If found custom <typeparamref name="THint"/>s are automatically enabled.</summary>
    /// <remarks>
    ///     The <typeparamref name="THint"/>s in all loaded assemblies
    ///     are used automatically unless set to <see langword="false"/>.
    /// </remarks>
    [ConfigurableOption]
    bool IncludeFoundHints { get; }

    /// <summary>Additional hints to be used by the tool.</summary>
    /// <remarks>These hints are used in the order specified before any other hint.</remarks>
    ImmutableArray<THint> Hints { get; }

    /// <summary>Options to use when working on child values during recursion.</summary>
    /// <remarks>If <see langword="null"/>, the previous options are utilized.</remarks>
    TSelf? NestedOptions { get; }
}
