global using DuplicatorMod = System.Func<
    CreateAndFake.Toolbox.DuplicatorTool.DuplicatorOptions,
    CreateAndFake.Toolbox.DuplicatorTool.DuplicatorOptions>;
using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Toolbox.DuplicatorTool;

/// <summary>Deep clones objects.</summary>
public interface IDuplicator
{
    /// <summary>Configured options for <c>this</c>.</summary>
    DuplicatorOptions Options { get; }

    /// <summary>Deep clones <paramref name="source"/>.</summary>
    /// <typeparam name="T"><c>Type</c> being cloned.</typeparam>
    /// <param name="source">Object to clone.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <returns>Clone of <paramref name="source"/>.</returns>
    /// <exception cref="NotSupportedException">If no hint supports cloning <paramref name="source"/>.</exception>
    /// <exception cref="InsufficientExecutionStackException">If infinite recursion occurs.</exception>
    [return: NotNullIfNotNull(nameof(source))]
    T Copy<T>(T source, DuplicatorMod? optionConfiguration = null);
}
