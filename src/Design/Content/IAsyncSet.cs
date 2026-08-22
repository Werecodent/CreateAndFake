namespace Werecodent.CreateAndFake.Design.Content;

/// <summary>Collection using asynchronously calculated hashes and equality.</summary>
/// <typeparam name="T"><inheritdoc cref="IAsyncEnumerable{T}" path="/typeparam[@name='T']"/></typeparam>
public interface IAsyncSet<T> : IAsyncEnumerable<T>
{
    /// <summary>Tries to add <paramref name="item"/> to the set.</summary>
    /// <param name="item">Instance to add.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns><see langword="true"/> if added, <see langword="false"/> if it's already been added.</returns>
    Task<bool> AddAsync(T item, CancellationToken canceler);

    /// <summary>Determines if <paramref name="key"/> is in the set as a key.</summary>
    /// <param name="key">Instance to check for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns><see langword="true"/> if found, <see langword="false"/> otherwise.</returns>
    Task<bool> ContainsKeyAsync(int key, CancellationToken canceler);

    /// <summary>Determines if <paramref name="item"/> is in the set.</summary>
    /// <param name="item">Instance to check for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns><see langword="true"/> if found, <see langword="false"/> otherwise.</returns>
    Task<bool> ContainsAsync(T item, CancellationToken canceler);

    /// <summary>Determines if the <paramref name="entry"/> is in the set.</summary>
    /// <param name="entry">Instance to check for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns><see langword="true"/> if found, <see langword="false"/> otherwise.</returns>
    Task<bool> ContainsAsync(KeyValuePair<int, T> entry, CancellationToken canceler);

    /// <summary>Enumerates all contents with their value hash.</summary>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The current contents.</returns>
    /// <remarks>Optimization for utilizing the 'find' methods.</remarks>
    IAsyncEnumerable<KeyValuePair<int, T>> ByHashesAsync(CancellationToken canceler);

    /// <summary>Finds common items with the <paramref name="collection"/>.</summary>
    /// <param name="collection">Set to check.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>Each common item.</returns>
    IAsyncEnumerable<T> FindMatchesInAsync(IAsyncSet<T> collection, CancellationToken canceler);

    /// <summary>Finds items missing that are present in <paramref name="collection"/>.</summary>
    /// <param name="collection">Set to check.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>Each missing item.</returns>
    IAsyncEnumerable<T> FindMissingFromAsync(IAsyncSet<T> collection, CancellationToken canceler);
}
