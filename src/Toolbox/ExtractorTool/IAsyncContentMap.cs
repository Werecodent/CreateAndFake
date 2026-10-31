using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.ExtractorTool;

/// <summary>Extracted content of an <see cref="object"/>.</summary>
public interface IAsyncContentMap : IAsyncEnumerable<object>, IValuerAsyncComparable
{
    /// <summary>Determines if the map has the given <paramref name="item"/> in it.</summary>
    /// <param name="item">Content to check for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>
    ///     <see langword="true"/> if <see langword="this"/> has the item,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    Task<bool> HasContentAsync(object? item, CancellationToken canceler);

    /// <summary>Determines if <see langword="this"/> has any content from the <paramref name="map"/> in it.</summary>
    /// <param name="map">Content to compare <see langword="this"/> with.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>
    ///     <see langword="true"/> if <see langword="this"/> has content shared with the maps,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Ignores types with too small of range for unique randomization
    ///     (<see cref="ExtractorOptions.IgnoredTypesForUniques"/>).
    /// </remarks>
    Task<bool> HasSharedContentAsync(IAsyncContentMap map, CancellationToken canceler);

    /// <summary>Determines if <see langword="this"/> has any content from the <paramref name="maps"/> in it.</summary>
    /// <param name="maps">Content to compare <see langword="this"/> with.</param>
    /// <inheritdoc cref="HasSharedContentAsync(IAsyncContentMap,CancellationToken)"/>
    Task<bool> HasSharedContentAsync(
        IEnumerable<IAsyncContentMap> maps,
        CancellationToken canceler
    );

    /// <summary>Finds content <see langword="this"/> shares with the <paramref name="map"/>.</summary>
    /// <param name="map">Content to compare <see langword="this"/> with.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>All shared content found.</returns>
    /// <remarks>
    ///     Ignores types with too small of range for unique randomization
    ///     (<see cref="ExtractorOptions.IgnoredTypesForUniques"/>).
    /// </remarks>
    IAsyncEnumerable<object> FindSharedContentAsync(
        IAsyncContentMap map,
        CancellationToken canceler
    );

    /// <summary>Finds content <see langword="this"/> shares with <paramref name="maps"/>.</summary>
    /// <param name="maps">Content to compare <see langword="this"/> with.</param>
    /// <inheritdoc cref="FindSharedContentAsync(IAsyncContentMap,CancellationToken)"/>
    IAsyncEnumerable<object> FindSharedContentAsync(
        IEnumerable<IAsyncContentMap> maps,
        CancellationToken canceler
    );

    /// <summary>Returns all possible <typeparamref name="T"/> instances.</summary>
    /// <typeparam name="T">Content <see cref="Type"/> to find.</typeparam>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>All <typeparamref name="T"/> instances (including subclasses).</returns>
    IAsyncEnumerable<T> FindAllAsync<T>(CancellationToken canceler);

    /// <summary>Returns all possible instances of the <paramref name="type"/>.</summary>
    /// <param name="type">Content <see cref="Type"/> to find.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>All instances or the <paramref name="type"/>(including subclasses).</returns>
    IAsyncEnumerable<object> FindAllAsync(Type type, CancellationToken canceler);
}
