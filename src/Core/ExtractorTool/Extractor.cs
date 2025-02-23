using System.Collections;
using System.Collections.Frozen;
using System.Reflection;

namespace CreateAndFake.ExtractorTool;

/// <inheritdoc cref="IExtractor"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public sealed class Extractor(ExtractorOptions options) : IExtractor
{
    /// <inheritdoc cref="ExtractorOptions.ContentEndTypes"/>
    private static readonly FrozenSet<Type> _ContentEndTypes = FrozenSet.ToFrozenSet([
        Assembly.GetExecutingAssembly().GetType(),
        typeof(Type).GetType(),
        typeof(ParameterInfo),
        typeof(PropertyInfo),
        typeof(MemberInfo),
        typeof(MethodInfo),
        typeof(FieldInfo),
        typeof(Assembly),
        typeof(string),
        typeof(Type)]);

    /// <inheritdoc/>
    public ExtractorOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public ContentMap Extract(object? source, ExtractorMod? optionConfiguration = null)
    {
        ExtractorOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        Dictionary<Type, ISet<object>> data = [];
        FlattenData(null, source, data, localOptions);
        return new ContentMap(data, localOptions);
    }

    /// <summary>Finds data associated with <paramref name="source"/></summary>
    /// <param name="memberType">Field/Property type the <paramref name="source"/> is assigned to.</param>
    /// <param name="source">Instance being deconstructed.</param>
    /// <param name="foundData">Collection to populate with found data.</param>
    private static void FlattenData(
        Type? memberType,
        object? source,
        IDictionary<Type, ISet<object>> foundData,
        ExtractorOptions options)
    {
        if (source != null)
        {
            Type keyType = memberType ?? source.GetType();
            try
            {
                if (!foundData.TryGetValue(keyType, out ISet<object>? data))
                {
                    data = new HashSet<object>(options.Valuer);
                    foundData.Add(keyType, data);
                }

                if (data.Add(source)
                    && !keyType.Inherits<Delegate>()
                    && !options.ContentEndTypes.Contains(keyType)
                    && !_ContentEndTypes.Contains(keyType))
                {
                    FlattenComplexData(source, foundData, options);
                }
            }
            catch (InsufficientExecutionStackException e)
            {
                throw new InsufficientExecutionStackException(
                    $"Ran into infinite generation trying to extract type '{keyType}'.", e);
            }
        }
    }

    /// <summary>Finds nested data associated with <paramref name="source"/>.</summary>
    /// <inheritdoc cref="FlattenData"/>
    private static void FlattenComplexData(
        object source,
        IDictionary<Type, ISet<object>> foundData,
        ExtractorOptions options)
    {
        if (source is IDictionary map)
        {
            FlattenDictionaryData(map, foundData, options);
        }
        else if (source is IEnumerable values)
        {
            FlattenEnumerableData(values, foundData, options);
        }
        else if (!source.GetType().IsValueType)
        {
            FlattenInnerData(source, foundData, options);
        }
    }

    /// <inheritdoc cref="FlattenComplexData"/>
    private static void FlattenDictionaryData(
        IDictionary source,
        IDictionary<Type, ISet<object>> foundData,
        ExtractorOptions options)
    {
        Type type = source.GetType();

        Type[] mapArgs = type.IsGenericType
                ? type.GetGenericArguments()
                : [typeof(object), typeof(object)];

        foreach (DictionaryEntry item in source)
        {
            FlattenData(mapArgs[0], item.Key, foundData, options);
            FlattenData(mapArgs[1], item.Value, foundData, options);
        }
    }

    /// <inheritdoc cref="FlattenComplexData"/>
    private static void FlattenEnumerableData(
        IEnumerable source,
        IDictionary<Type, ISet<object>> foundData,
        ExtractorOptions options)
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
            FlattenData(arrayType, gen.Current, foundData, options);
        }
    }

    /// <summary>Finds member data inside <paramref name="source"/>.</summary>
    /// <inheritdoc cref="FlattenData"/>
    private static void FlattenInnerData(
        object source,
        IDictionary<Type, ISet<object>> foundData,
        ExtractorOptions options)
    {
        Type type = source.GetType();

        BindingFlags scope = options.ExtractPrivateMembers
            ? BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic
            : BindingFlags.Public | BindingFlags.Instance;

        foreach (PropertyInfo property in type.GetProperties(scope).Where(p => p.CanRead))
        {
            FlattenData(property.PropertyType, property.GetValue(source), foundData, options);
        }

        foreach (FieldInfo field in type.GetFields(scope))
        {
            FlattenData(field.FieldType, field.GetValue(source), foundData, options);
        }
    }
}
