namespace CreateAndFake.ExtractorTool;

/// <summary>Extracted content of an object.</summary>
public interface IContentMap
{
    /// <summary>Iterates all the extracted contents.</summary>
    /// <returns>The extracted contents.</returns>
    IEnumerable<object> AllContent();

    /// <summary>Determines if the map has <paramref name="item"/> in it.</summary>
    /// <param name="item">Content to check for.</param>
    /// <returns><see langword="true"/> if <see langword="this"/> has the item, <see langword="false"/> otherwise.</returns>
    bool HasContent(object? item);

    /// <summary>Determines if <see langword="this"/> has any content from <paramref name="maps"/> in it.</summary>
    /// <param name="maps">Content to compare <see langword="this"/> with.</param>
    /// <returns><see langword="true"/> if <see langword="this"/> has content from <paramref name="maps"/>, <see langword="false"/> otherwise.</returns>
    /// <remarks>Ignores types with too small of range for unique randomization.</remarks>
    bool HasSharedContent(params IEnumerable<IContentMap> maps);

    /// <summary>Finds content <see langword="this"/> shares with <paramref name="maps"/>.</summary>
    /// <param name="maps">Content to compare <see langword="this"/> with.</param>
    /// <returns>All shared content found.</returns>
    /// <remarks>Ignores types with too small of range for unique randomization.</remarks>
    IEnumerable<object> FindSharedContent(params IEnumerable<IContentMap> maps);

    /// <summary>Returns all possible <typeparamref name="T"/> instances.</summary>
    /// <typeparam name="T">Content type to find.</typeparam>
    /// <returns>All <typeparamref name="T"/> instances (including subclasses).</returns>
    IEnumerable<T> FindAll<T>();

    /// <summary>Returns all possible <paramref name="type"/> instances.</summary>
    /// <param name="type">Content type to find.</param>
    /// <returns>All <paramref name="type"/> instances (including subclasses).</returns>
    IEnumerable<object> FindAll(Type type);
}
