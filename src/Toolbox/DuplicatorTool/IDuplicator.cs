global using DuplicatorMod = System.Func<
    CreateAndFake.DuplicatorTool.DuplicatorOptions,
    CreateAndFake.DuplicatorTool.DuplicatorOptions
>;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.DuplicatorTool;

/// <summary>Deep clones objects.</summary>
public interface IDuplicator : ITool<DuplicatorOptions>
{
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> for the new tool.</param>
    /// <returns>The created tool.</returns>
    IDuplicator WithOptions(DuplicatorMod optionConfiguration);

    /// <summary>Deep clones <paramref name="source"/>.</summary>
    /// <typeparam name="T"><c>Type</c> being cloned.</typeparam>
    /// <param name="source">Object to clone.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    /// <returns>Clone of <paramref name="source"/>.</returns>
    /// <exception cref="NotSupportedException">If no hint supports cloning <paramref name="source"/>.</exception>
    /// <exception cref="InsufficientExecutionStackException">If infinite recursion occurs.</exception>
    [return: NotNullIfNotNull(nameof(source))]
    T Copy<T>(T source, DuplicatorMod? optionConfiguration = null);
}
