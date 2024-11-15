using System.Collections;
using System.Reflection;
using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFake.Toolbox.MutatorTool;

/// <summary>Extracted content of an object.</summary>
/// <param name="content"><inheritdoc cref="_content" path="/summary"/></param>
public sealed class ContentMap(IDictionary<Type, ISet<object>> content)
{
    /// <summary>Scope used to search for inner types.</summary>
    private const BindingFlags _Scope
        = BindingFlags.Public
        | BindingFlags.NonPublic
        | BindingFlags.Instance;

    /// <summary>Types with too small of range for unique randomization.</summary>
    private static readonly HashSet<Type> _SmallTypes = [
        typeof(bool),
        typeof(char),
        typeof(byte)];

    /// <summary>Flattened object data.</summary>
    private readonly IDictionary<Type, ISet<object>> _content = content
        ?? throw new ArgumentNullException(nameof(content));

    /// <summary>Iterates all the extracted contents.</summary>
    /// <returns>The extracted contents.</returns>
    public IEnumerable<object> AllContent()
    {
        return _content.Values.SelectMany(x => x);
    }

    /// <summary>Determines if the map has <paramref name="item"/> in it.</summary>
    /// <param name="valuer">Comparison tool determining equality.</param>
    /// <param name="item">Content to check for.</param>
    /// <returns><c>true</c> if <c>this</c> has the item; <c>false</c> otherwise.</returns>
    public bool HasContent(IValuer valuer, object? item)
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
                .Any(i => valuer.Equals(item, i));
    }

    /// <summary>Determines if <c>this</c> has any content from <paramref name="maps"/> in it.</summary>
    /// <param name="valuer">Comparison tool determining equality.</param>
    /// <param name="maps">Content to compare <c>this</c> with.</param>
    /// <returns><c>true</c> if <c>this</c> has content from <paramref name="maps"/>; <c>false</c> otherwise.</returns>
    /// <remarks>Ignores types with too small of range for unique randomization.</remarks>
    public bool HasSharedContent(IValuer valuer, params IEnumerable<ContentMap> maps)
    {
        return FindSharedContent(valuer, maps).Any();
    }

    /// <summary>Finds content <c>this</c> shares with <paramref name="maps"/>.</summary>
    /// <param name="valuer">Comparison tool determining equality.</param>
    /// <param name="maps">Content to compare <c>this</c> with.</param>
    /// <returns>All shared content found.</returns>
    /// <remarks>Ignores types with too small of range for unique randomization.</remarks>
    public IEnumerable<object> FindSharedContent(IValuer valuer, params IEnumerable<ContentMap> maps)
    {
        return maps
            .SelectMany(m => m.AllContent())
            .Intersect(AllContent(), valuer)
            .Where(d => !_SmallTypes.Contains(d.GetType()))
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

    /// <inheritdoc cref="FlattenData"/>
    /// <returns>Extracted content of <paramref name="source"/>.</returns>
    public static ContentMap Extract(object? source)
    {
        Dictionary<Type, ISet<object>> data = [];
        FlattenData(null, source, data);
        return new ContentMap(data);
    }

    /// <summary>Finds data associated with <paramref name="source"/></summary>
    /// <param name="memberType">Field/Property type the <paramref name="source"/> is assigned to.</param>
    /// <param name="source">Instance being deconstructed.</param>
    /// <param name="foundData">Collection to populate with found data.</param>
    private static void FlattenData(Type? memberType, object? source, IDictionary<Type, ISet<object>> foundData)
    {
        if (source != null)
        {
            Type keyType = memberType ?? source.GetType();

            if (!foundData.TryGetValue(keyType, out ISet<object>? data))
            {
                data = new HashSet<object>();
                foundData.Add(keyType, data);
            }

            if (data.Add(source) && source.GetType() != typeof(string))
            {
                FlattenComplexData(source, foundData);
            }
        }
    }

    /// <summary>Finds nested data associated with <paramref name="source"/>.</summary>
    /// <inheritdoc cref="FlattenData"/>
    private static void FlattenComplexData(object source, IDictionary<Type, ISet<object>> foundData)
    {
        if (source is IDictionary map)
        {
            FlattenDictionaryData(map, foundData);
        }
        else if (source is IEnumerable values)
        {
            FlattenEnumerableData(values, foundData);
        }
        else if (!source.GetType().IsValueType)
        {
            FlattenInnerData(source, foundData);
        }
    }

    /// <inheritdoc cref="FlattenComplexData"/>
    private static void FlattenDictionaryData(IDictionary source, IDictionary<Type, ISet<object>> foundData)
    {
        Type type = source.GetType();

        Type[] mapArgs = type.IsGenericType
                ? type.GetGenericArguments()
                : [typeof(object), typeof(object)];

        foreach (DictionaryEntry item in source)
        {
            FlattenData(mapArgs[0], item.Key, foundData);
            FlattenData(mapArgs[1], item.Value, foundData);
        }
    }

    /// <inheritdoc cref="FlattenComplexData"/>
    private static void FlattenEnumerableData(IEnumerable source, IDictionary<Type, ISet<object>> foundData)
    {
        Type type = source.GetType();

        Type? arrayType = type.IsArray
                ? type.GetElementType()
                : type.IsGenericType
                ? type.GetGenericArguments()[0]
                : typeof(object);

        IEnumerator gen = source.GetEnumerator();
        while (gen.MoveNext())
        {
            FlattenData(arrayType, gen.Current, foundData);
        }
    }

    /// <summary>Finds member data inside <paramref name="source"/>.</summary>
    /// <inheritdoc cref="FlattenData"/>
    private static void FlattenInnerData(object source, IDictionary<Type, ISet<object>> foundData)
    {
        Type type = source.GetType();

        foreach (PropertyInfo property in type.GetProperties(_Scope).Where(p => p.CanRead))
        {
            FlattenData(property.PropertyType, property.GetValue(source), foundData);
        }

        foreach (FieldInfo field in type.GetFields(_Scope))
        {
            FlattenData(field.FieldType, field.GetValue(source), foundData);
        }
    }
}
