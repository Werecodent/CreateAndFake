global using DuplicatorMod = System.Func<
    CreateAndFake.DuplicatorTool.DuplicatorOptions,
    CreateAndFake.DuplicatorTool.DuplicatorOptions
>;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool;

/// <summary>Deep clones objects.</summary>
public interface IDuplicator : IHintTool<DuplicatorOptions, ICopyHint>
{
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of Options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IDuplicator WithOptions(DuplicatorMod optionConfiguration);

    /// <summary>Deep clones <paramref name="source"/>.</summary>
    /// <typeparam name="T"><see cref="Type"/> being cloned.</typeparam>
    /// <param name="source">Object to clone.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>Clone of <paramref name="source"/>.</returns>
    /// <remarks>
    ///     Depending upon the <see cref="Type"/>, this may just
    ///     return <paramref name="source"/> if not cloneable.
    /// </remarks>
    /// <exception cref="UnsupportedException">
    ///     If no hint supports cloning <paramref name="source"/>.
    /// </exception>
    /// <exception cref="InsufficientExecutionStackException">
    ///     If infinite recursion occurs.
    /// </exception>
    [return: NotNullIfNotNull(nameof(source))]
    T Copy<T>(T source, DuplicatorMod? optionConfiguration = null);
}
