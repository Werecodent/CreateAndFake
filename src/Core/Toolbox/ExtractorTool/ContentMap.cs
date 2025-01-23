using System.Collections;

namespace CreateAndFake.Toolbox.ExtractorTool;

/// <summary>Extracted content of an object.</summary>
/// <param name="content"><inheritdoc cref="_content" path="/summary"/></param>
/// <param name="options"><inheritdoc cref="_options" path="/summary"/></param>
public sealed class ContentMap(IDictionary<Type, ISet<object>> content, ExtractorOptions options)
{
    /// <summary>Flattened object data.</summary>
    private readonly IDictionary<Type, ISet<object>> _content = content
        ?? throw new ArgumentNullException(nameof(content));

    /// <summary>Configured options used to extract the contents.</summary>
    private readonly ExtractorOptions _options = options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Iterates all the extracted contents.</summary>
    /// <returns>The extracted contents.</returns>
    public IEnumerable<object> AllContent()
    {
        return _content.Values.SelectMany(x => x);
    }

    /// <summary>Determines if the map has <paramref name="item"/> in it.</summary>
    /// <param name="item">Content to check for.</param>
    /// <returns><c>true</c> if <c>this</c> has the item; <c>false</c> otherwise.</returns>
    public bool HasContent(object? item)
    {
        if (item == null)
        {
            return false;
        }

        Type itemType = item.GetType();

        return itemType.IsValueType || itemType == typeof(string)
            ? _content.Values
                .Any(p => p.Contains(item))
            : _content.Keys
                .Where(k => k.Inherits(itemType))
                .SelectMany(k => _content[k])
                .Any(i => _options.Valuer.Equals(item, i));
    }

    /// <summary>Determines if <c>this</c> has any content from <paramref name="maps"/> in it.</summary>
    /// <param name="maps">Content to compare <c>this</c> with.</param>
    /// <returns><c>true</c> if <c>this</c> has content from <paramref name="maps"/>; <c>false</c> otherwise.</returns>
    /// <remarks>Ignores types with too small of range for unique randomization.</remarks>
    public bool HasSharedContent(params IEnumerable<ContentMap> maps)
    {
        return FindSharedContent(maps).Any();
    }

    /// <summary>Finds content <c>this</c> shares with <paramref name="maps"/>.</summary>
    /// <param name="maps">Content to compare <c>this</c> with.</param>
    /// <returns>All shared content found.</returns>
    /// <remarks>Ignores types with too small of range for unique randomization.</remarks>
    public IEnumerable<object> FindSharedContent(params IEnumerable<ContentMap> maps)
    {
        return maps
            .SelectMany(m => m.AllContent())
            .Intersect(AllContent(), _options.Valuer)
            .Where(d => !_options.UniqueIgnoredTypes.Contains(d.GetType()))
            .Where(d => !d.GetType().IsEnum)
            .Where(d => !(d is IEnumerable series && !series.GetEnumerator().MoveNext()));
    }

    /// <summary>Returns all possible <typeparamref name="T"/> instances.</summary>
    /// <typeparam name="T">Content type to find.</typeparam>
    /// <returns>All <typeparamref name="T"/> instances (including subclasses).</returns>
    public IEnumerable<T> FindAll<T>()
    {
        return _content.Keys
            .Where(t => t.Inherits<T>())
            .SelectMany(t => _content[t])
            .OfType<T>();
    }

    /// <summary>Returns all possible <paramref name="type"/> instances.</summary>
    /// <param name="type">Content type to find.</param>
    /// <returns>All <paramref name="type"/> instances (including subclasses).</returns>
    public IEnumerable<object> FindAll(Type type)
    {
        return _content.Keys
            .Where(t => t.Inherits(type))
            .SelectMany(t => _content[t])
            .Where(t => t.GetType().Inherits(type));
    }
}
