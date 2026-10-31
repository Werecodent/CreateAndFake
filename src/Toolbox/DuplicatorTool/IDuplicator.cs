global using DuplicatorMod = System.Func<
    Werecodent.CreateAndFake.DuplicatorTool.DuplicatorOptions,
    Werecodent.CreateAndFake.DuplicatorTool.DuplicatorOptions
>;
using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool;

/// <summary>Tool to deep clone <see cref="object"/>s.</summary>
/// <remarks>Useful for tracking mutations by comparing against the created clones.</remarks>
public interface IDuplicator : IHintTool<DuplicatorOptions, ICopyHint>
{
    /// <summary>Creates a new tool with the <paramref name="optionConfiguration"/> changes applied.</summary>
    /// <param name="optionConfiguration">Modifications of this tool's options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IDuplicator WithOptions(DuplicatorMod optionConfiguration);

    /// <summary>Deep clones <paramref name="source"/>.</summary>
    /// <typeparam name="T"><see cref="Type"/> being cloned.</typeparam>
    /// <param name="source">Object to clone.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>Clone of <paramref name="source"/>.</returns>
    /// <remarks>
    ///     Depending upon the <see cref="Type"/>, this may just return <paramref name="source"/> if not cloneable.
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="DuplicatorOptions.VerifyEqualityForClones"/></item>
    ///     </list>
    /// </remarks>
    [return: NotNullIfNotNull(nameof(source))]
    T Copy<T>(T source, DuplicatorMod? optionConfiguration = null);
}
