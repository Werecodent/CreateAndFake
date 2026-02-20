global using ValuerMod = System.Func<
    CreateAndFake.ValuerTool.ValuerOptions,
    CreateAndFake.ValuerTool.ValuerOptions
>;
using System.Collections;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool;

/// <summary>Compares objects by value via reflection if needed.</summary>
public interface IValuer
    : IHintTool<ValuerOptions, ICompareHint>,
        IEqualityComparer<object>,
        IEqualityComparer
{
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of Options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IValuer WithOptions(ValuerMod optionConfiguration);

    /// <summary>Finds the differences between <paramref name="expected"/> and <paramref name="actual"/>.</summary>
    /// <param name="expected">Object to compare with <paramref name="actual"/>.</param>
    /// <param name="actual">Potentially different object to compare against <paramref name="expected"/>.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>Found differences between <paramref name="expected"/> and <paramref name="actual"/>.</returns>
    /// <exception cref="NotSupportedException">If no hint supports comparing the objects.</exception>
    /// <exception cref="InsufficientExecutionStackException">If infinite recursion occurs.</exception>
    IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration = null
    );

    /// <inheritdoc cref="Compare"/>
    IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    );

    /// <inheritdoc cref="Equals(object,object,ValuerMod)"/>
    new bool Equals(object? x, object? y);

    /// <summary>Determines if <paramref name="x"/> equals <paramref name="y"/> by value.</summary>
    /// <param name="x">Object to compare with <paramref name="y"/>.</param>
    /// <param name="y">Object to compare with <paramref name="x"/>.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="x"/> equals <paramref name="y"/> by value, <see langword="false"/> otherwise.
    /// </returns>
    /// <exception cref="NotSupportedException">If no hint supports comparing the objects.</exception>
    /// <exception cref="InsufficientExecutionStackException">If infinite recursion occurs.</exception>
    bool Equals(object? x, object? y, ValuerMod? optionConfiguration);

    /// <inheritdoc cref="Equals(object,object,ValuerMod)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<bool> EqualsAsync(
        object? x,
        object? y,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    );

    /// <inheritdoc cref="GetHashCode(object,ValuerMod)"/>
    new int GetHashCode(object? item);

    /// <summary>Computes an identifying hash code for <paramref name="item"/> based upon value.</summary>
    /// <param name="item">Object to generate a hash code for.</param>
    /// <returns>The value computed hash code for <paramref name="item"/>.</returns>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <exception cref="NotSupportedException">If no hint supports hashing the object.</exception>
    /// <exception cref="InsufficientExecutionStackException">If infinite recursion occurs.</exception>
    int GetHashCode(object? item, ValuerMod? optionConfiguration);

    /// <inheritdoc cref="GetHashCode(object,ValuerMod)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<int> GetHashCodeAsync(
        object? item,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    );

    /// <summary>...</summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IEqualityComparer<T> ToComparer<T>();
}
