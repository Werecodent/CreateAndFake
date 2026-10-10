using System.Collections;
using System.Text;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.ExtractorTool;

/// <summary>Extracted content of an object.</summary>
/// <param name="content"><inheritdoc cref="_content" path="/summary"/></param>
/// <param name="options"><inheritdoc cref="_options" path="/summary"/></param>
public sealed class ContentMap(ISet<object> content, ExtractorOptions options) : IContentMap
{
    /// <summary>Flattened object data.</summary>
    private readonly ISet<object> _content =
        content ?? throw new ArgumentNullException(nameof(content));

    /// <summary>Configured options used to extract the contents.</summary>
    private readonly ExtractorOptions _options =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc/>
    public IEnumerator<object> GetEnumerator()
    {
        return _content.GetEnumerator();
    }

    /// <inheritdoc/>
    public bool HasContent(object? item)
    {
        if (item == null)
        {
            return false;
        }

        return _content.Contains(item);
    }

    /// <inheritdoc/>
    public bool HasSharedContent(params IEnumerable<IContentMap> maps)
    {
        return FindSharedContent(maps).Any();
    }

    /// <inheritdoc/>
    public IEnumerable<object> FindSharedContent(params IEnumerable<IContentMap> maps)
    {
        return maps.SelectMany(m => m)
            .Intersect(this, _options.Valuer)
            .Where(d => !_options.UniqueIgnoredTypes.Contains(d.GetType()))
            .Where(d => !d.GetType().IsEnum)
            .Where(d =>
            {
                if (d is IEnumerable series)
                {
                    foreach (object item in series)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            });
    }

    /// <inheritdoc/>
    public IEnumerable<T> FindAll<T>()
    {
        return _content.OfType<T>();
    }

    /// <inheritdoc/>
    public IEnumerable<object> FindAll(Type type)
    {
        return _content.Where(t => t.GetType().Inherits(type));
    }

    /// <inheritdoc/>
    public IEnumerable<Difference> Compare(object? other, IValuer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        if (other is ContentMap map)
        {
            return valuer.Compare((_content, _options), (map._content, map._options));
        }
        else
        {
            return valuer.Compare(GetType(), other?.GetType());
        }
    }

    /// <inheritdoc/>
    public bool ValuesEqual(object? other, IValuer valuer)
    {
        return !Compare(other, valuer).Any();
    }

    /// <inheritdoc/>
    public int GetValueHash(IValuer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);
        return valuer.GetHashCode((_content, _options));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder text = new();

        text.AppendLine("ContentMap: {");
        foreach (object item in _content)
        {
            text.Append("    ").Append(item).AppendLine();
        }
        text.Append('}');

        return text.ToString();
    }
}
