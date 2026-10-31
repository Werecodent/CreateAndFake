global using ValuerMod = System.Func<
    Werecodent.CreateAndFake.ValuerTool.ValuerOptions,
    Werecodent.CreateAndFake.ValuerTool.ValuerOptions
>;
using System.Collections;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool;

#pragma warning disable CA1068 // Overloaded parameter last.

/// <summary>Tool to compare <see cref="object"/>s by value via reflection if needed.</summary>
/// <remarks>
///     Useful for checking equality between <see cref="object"/>s without having to override
///     <see cref="Equals(object,object)"/> and <see cref="GetHashCode(object)"/> manually.
/// </remarks>
public interface IValuer
    : IHintTool<ValuerOptions, ICompareHint>,
        IAsyncEqualityComparer<object>,
        IEqualityComparer<object>,
        IEqualityComparer
{
    /// <summary>Creates a new tool with the <paramref name="optionConfiguration"/> changes applied.</summary>
    /// <param name="optionConfiguration">Modifications of this tool's options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IValuer WithOptions(ValuerMod optionConfiguration);

    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="ValuerOptions.SkipAsyncValues"/></item>
    ///         <item><see cref="ValuerOptions.CheckCollectionType"/></item>
    ///         <item><see cref="ValuerOptions.DefaultEqualityTypes"/></item>
    ///         <item><see cref="ValuerOptions.UseEquatableComparisons"/></item>
    ///         <item><see cref="ValuerOptions.IgnoreCurrentRandomSeed"/></item>
    ///         <item><see cref="ValuerOptions.IncludeValueHashInComparison"/></item>
    ///     </list>
    /// </remarks>
    /// <inheritdoc cref="CompareAsync"/>
    IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration = null
    );

    /// <summary>Finds the differences between <paramref name="expected"/> and <paramref name="actual"/>.</summary>
    /// <param name="expected">Object to compare with <paramref name="actual"/>.</param>
    /// <param name="actual">Potentially different object to compare against <paramref name="expected"/>.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>Found differences between <paramref name="expected"/> and <paramref name="actual"/>.</returns>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="ValuerOptions.AsyncTimeout"/></item>
    ///         <item><see cref="ValuerOptions.CheckCollectionType"/></item>
    ///         <item><see cref="ValuerOptions.DefaultEqualityTypes"/></item>
    ///         <item><see cref="ValuerOptions.UseEquatableComparisons"/></item>
    ///         <item><see cref="ValuerOptions.IgnoreCurrentRandomSeed"/></item>
    ///         <item><see cref="ValuerOptions.IncludeValueHashInComparison"/></item>
    ///     </list>
    /// </remarks>
    /// <inheritdoc cref="Compare"/>
    IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    );

    /// <inheritdoc cref="Equals(object,object,ValuerMod)"/>
    new bool Equals(object? x, object? y);

    /// <inheritdoc cref="EqualsAsync(object,object,CancellationToken,ValuerMod)"/>
    bool Equals(object? x, object? y, ValuerMod? optionConfiguration);

    /// <inheritdoc cref="EqualsAsync(object,object,CancellationToken,ValuerMod)"/>
    new Task<bool> EqualsAsync(object? x, object? y, CancellationToken canceler);

    /// <summary>Determines if <paramref name="x"/> equals <paramref name="y"/> by value.</summary>
    /// <param name="x">Object to compare with <paramref name="y"/>.</param>
    /// <param name="y">Object to compare with <paramref name="x"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="x"/> equals <paramref name="y"/> by value,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    /// <inheritdoc cref="CompareAsync"/>
    Task<bool> EqualsAsync(
        object? x,
        object? y,
        CancellationToken canceler,
        ValuerMod? optionConfiguration
    );

    /// <inheritdoc cref="GetHashCode(object,ValuerMod)"/>
    new int GetHashCode(object? item);

    /// <inheritdoc cref="GetHashCodeAsync(object,CancellationToken,ValuerMod)"/>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="ValuerOptions.SkipAsyncValues"/></item>
    ///         <item><see cref="ValuerOptions.DefaultEqualityTypes"/></item>
    ///         <item><see cref="ValuerOptions.UseEquatableComparisons"/></item>
    ///         <item><see cref="ValuerOptions.IgnoreCurrentRandomSeed"/></item>
    ///     </list>
    /// </remarks>
    int GetHashCode(object? item, ValuerMod? optionConfiguration);

    /// <inheritdoc cref="GetHashCodeAsync(object,CancellationToken,ValuerMod)"/>
    new Task<int> GetHashCodeAsync(object? item, CancellationToken canceler);

    /// <summary>Computes an identifying hash code for <paramref name="item"/> based upon value.</summary>
    /// <param name="item">Object to generate a hash code for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>The value computed hash code for <paramref name="item"/>.</returns>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="ValuerOptions.AsyncTimeout"/></item>
    ///         <item><see cref="ValuerOptions.DefaultEqualityTypes"/></item>
    ///         <item><see cref="ValuerOptions.UseEquatableComparisons"/></item>
    ///         <item><see cref="ValuerOptions.IgnoreCurrentRandomSeed"/></item>
    ///     </list>
    /// </remarks>
    Task<int> GetHashCodeAsync(
        object? item,
        CancellationToken canceler,
        ValuerMod? optionConfiguration
    );

    /// <summary>Provides a comparer that uses this tool as the source for the comparisons.</summary>
    /// <typeparam name="T"><see cref="Type"/> to compare.</typeparam>
    /// <returns>The comparer to use.</returns>
    IEqualityComparer<T> ToComparer<T>();

    /// <summary>Provides a comparer that uses this tool as the source for the comparisons.</summary>
    /// <typeparam name="T"><see cref="Type"/> to compare.</typeparam>
    /// <returns>The comparer to use.</returns>
    IAsyncEqualityComparer<T> ToAsyncComparer<T>();
}

#pragma warning restore
