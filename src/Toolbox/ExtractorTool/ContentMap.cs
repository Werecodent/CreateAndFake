using System.Collections;
using System.Text;
using CreateAndFake.Design.Types;

namespace CreateAndFake.ExtractorTool;

/// <summary>Extracted content of an object.</summary>
/// <param name="content"><inheritdoc cref="_content" path="/summary"/></param>
/// <param name="options"><inheritdoc cref="_options" path="/summary"/></param>
public sealed class ContentMap(IDictionary<Type, ISet<object>> content, ExtractorOptions options)
    : IContentMap
{
    /// <summary>Flattened object data.</summary>
    private readonly IDictionary<Type, ISet<object>> _content =
        content ?? throw new ArgumentNullException(nameof(content));

    /// <summary>Configured options used to extract the contents.</summary>
    private readonly ExtractorOptions _options =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<object> AllContent()
    {
        return _content.Values.SelectMany(x => x);
    }

    /// <inheritdoc/>
    public bool HasContent(object? item)
    {
        if (item == null)
        {
            return false;
        }

        Type itemType = item.GetType();

        return itemType.IsValueType || itemType == typeof(string)
            ? _content.Values.Any(p => p.Contains(item))
            : _content
                .Keys.Where(k => k.Inherits(itemType))
                .SelectMany(k => _content[k])
                .Any(i => _options.Valuer.Equals(item, i));
    }

    /// <inheritdoc/>
    public bool HasSharedContent(params IEnumerable<IContentMap> maps)
    {
        return FindSharedContent(maps).Any();
    }

    /// <inheritdoc/>
    public IEnumerable<object> FindSharedContent(params IEnumerable<IContentMap> maps)
    {
        return maps.SelectMany(m => m.AllContent())
            .Intersect(AllContent(), _options.Valuer)
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
        return _content.Keys.Where(t => t.Inherits<T>()).SelectMany(t => _content[t]).OfType<T>();
    }

    /// <inheritdoc/>
    public IEnumerable<object> FindAll(Type type)
    {
        return _content
            .Keys.Where(t => t.Inherits(type))
            .SelectMany(t => _content[t])
            .Where(t => t.GetType().Inherits(type));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder text = new();

        text.AppendLine("ContentMap: {");
        foreach (KeyValuePair<Type, ISet<object>> set in _content)
        {
            string type = GenericConverter.ExpandName(set.Key);

            foreach (object item in set.Value)
            {
                text.Append("    ").Append(type).Append(", ").AppendLine(item?.ToString());
            }
        }
        text.Append('}');

        return text.ToString();
    }
}
