namespace Werecodent.CreateAndFake.ExtractorTool;

/// <summary>Extracted content of an object.</summary>
public interface IAsyncContentMap
{
    /// <summary>Iterates all the extracted contents.</summary>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The extracted contents.</returns>
    IAsyncEnumerable<object> AllContentAsync(CancellationToken canceler);

    /// <summary>Determines if the map has <paramref name="item"/> in it.</summary>
    /// <param name="item">Content to check for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns><see langword="true"/> if <see langword="this"/> has the item, <see langword="false"/> otherwise.</returns>
    Task<bool> HasContentAsync(object? item, CancellationToken canceler);

    /// <summary>Determines if <see langword="this"/> has any content shared with other maps.</summary>
    /// <param name="map">Content to compare <see langword="this"/> with.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>
    ///     <see langword="true"/> if <see langword="this"/> has content shared with the maps,
    ///     <see langword="false"/> otherwise.</returns>
    /// <remarks>Ignores types with too small of range for unique randomization.</remarks>
    Task<bool> HasSharedContentAsync(IAsyncContentMap map, CancellationToken canceler);

    /// <param name="maps">Content to compare <see langword="this"/> with.</param>
    /// <inheritdoc cref="HasSharedContentAsync(IAsyncContentMap,CancellationToken)"/>
    Task<bool> HasSharedContentAsync(
        IEnumerable<IAsyncContentMap> maps,
        CancellationToken canceler
    );

    /// <summary>Finds content <see langword="this"/> shares with other maps.</summary>
    /// <param name="map">Content to compare <see langword="this"/> with.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>All shared content found.</returns>
    /// <remarks>Ignores types with too small of range for unique randomization.</remarks>
    IAsyncEnumerable<object> FindSharedContentAsync(
        IAsyncContentMap map,
        CancellationToken canceler
    );

    /// <param name="maps">Content to compare <see langword="this"/> with.</param>
    /// <inheritdoc cref="FindSharedContentAsync(IAsyncContentMap,CancellationToken)"/>
    IAsyncEnumerable<object> FindSharedContentAsync(
        IEnumerable<IAsyncContentMap> maps,
        CancellationToken canceler
    );

    /// <summary>Returns all possible <typeparamref name="T"/> instances.</summary>
    /// <typeparam name="T">Content type to find.</typeparam>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>All <typeparamref name="T"/> instances (including subclasses).</returns>
    IAsyncEnumerable<T> FindAllAsync<T>(CancellationToken canceler);

    /// <summary>Returns all possible <paramref name="type"/> instances.</summary>
    /// <param name="type">Content type to find.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>All <paramref name="type"/> instances (including subclasses).</returns>
    IAsyncEnumerable<object> FindAllAsync(Type type, CancellationToken canceler);
}
