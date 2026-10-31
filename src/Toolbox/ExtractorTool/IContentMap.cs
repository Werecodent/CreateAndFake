using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.ExtractorTool;

/// <summary>Extracted content of an <see cref="object"/>.</summary>
public interface IContentMap : IEnumerable<object>, IValuerComparable
{
    /// <summary>Determines if the map has the given <paramref name="item"/> in it.</summary>
    /// <param name="item">Content to check for.</param>
    /// <returns>
    ///     <see langword="true"/> if <see langword="this"/> has the <paramref name="item"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    bool HasContent(object? item);

    /// <summary>Determines if <see langword="this"/> has any content from the <paramref name="maps"/> in it.</summary>
    /// <param name="maps">Content to compare <see langword="this"/> with.</param>
    /// <returns>
    ///     <see langword="true"/> if <see langword="this"/> has content from any of the other <paramref name="maps"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Ignores types with too small of range for unique randomization
    ///     (<see cref="ExtractorOptions.IgnoredTypesForUniques"/>).
    /// </remarks>
    bool HasSharedContent(params IEnumerable<IContentMap> maps);

    /// <summary>Finds content <see langword="this"/> shares with <paramref name="maps"/>.</summary>
    /// <param name="maps">Content to compare <see langword="this"/> with.</param>
    /// <returns>All shared content found.</returns>
    /// <remarks>
    ///     Ignores types with too small of range for unique randomization
    ///     (<see cref="ExtractorOptions.IgnoredTypesForUniques"/>).
    /// </remarks>
    IEnumerable<object> FindSharedContent(params IEnumerable<IContentMap> maps);

    /// <summary>Returns all possible <typeparamref name="T"/> instances.</summary>
    /// <typeparam name="T">Content <see cref="Type"/> to find.</typeparam>
    /// <returns>All <typeparamref name="T"/> instances (including subclasses).</returns>
    IEnumerable<T> FindAll<T>();

    /// <summary>Returns all possible instances of the <paramref name="type"/>.</summary>
    /// <param name="type">Content <see cref="Type"/> to find.</param>
    /// <returns>All instances of the <paramref name="type"/>(including subclasses).</returns>
    IEnumerable<object> FindAll(Type type);
}
