global using ValuerMod = System.Func<
    CreateAndFake.Toolbox.ValuerTool.ValuerOptions,
    CreateAndFake.Toolbox.ValuerTool.ValuerOptions>;
using System.Collections;

namespace CreateAndFake.Toolbox.ValuerTool;

/// <summary>Compares objects by value via reflection if needed.</summary>
public interface IValuer : IEqualityComparer<object>, IEqualityComparer
{
    /// <summary>Configured options for <c>this</c>.</summary>
    ValuerOptions Options { get; }

    /// <summary>Finds the differences between <paramref name="expected"/> and <paramref name="actual"/>.</summary>
    /// <param name="expected">Object to compare with <paramref name="actual"/>.</param>
    /// <param name="actual">Potentially different object to compare against <paramref name="expected"/>.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <returns>Found differences between <paramref name="expected"/> and <paramref name="actual"/>.</returns>
    /// <exception cref="NotSupportedException">If no hint supports comparing the objects.</exception>
    /// <exception cref="InsufficientExecutionStackException">If infinite recursion occurs.</exception>
    IEnumerable<Difference> Compare(object? expected, object? actual, ValuerMod? optionConfiguration = null);

    /// <inheritdoc cref="Equals(object,object,ValuerMod)"/>
    new bool Equals(object? x, object? y);

    /// <summary>Determines if <paramref name="x"/> equals <paramref name="y"/> by value.</summary>
    /// <param name="x">Object to compare with <paramref name="y"/>.</param>
    /// <param name="y">Object to compare with <paramref name="x"/>.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="x"/> equals <paramref name="y"/> by value; <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="NotSupportedException">If no hint supports comparing the objects.</exception>
    /// <exception cref="InsufficientExecutionStackException">If infinite recursion occurs.</exception>
    bool Equals(object? x, object? y, ValuerMod? optionConfiguration = null);

    /// <inheritdoc cref="GetHashCode(object,ValuerMod)"/>
    new int GetHashCode(object? item);

    /// <summary>Computes an identifying hash code for <paramref name="item"/> based upon value.</summary>
    /// <param name="item">Object to generate a hash code for.</param>
    /// <returns>The value computed hash code for <paramref name="item"/>.</returns>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <exception cref="NotSupportedException">If no hint supports hashing the object.</exception>
    /// <exception cref="InsufficientExecutionStackException">If infinite recursion occurs.</exception>
    int GetHashCode(object? item, ValuerMod? optionConfiguration = null);
}
