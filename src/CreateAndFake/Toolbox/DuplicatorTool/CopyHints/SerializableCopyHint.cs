using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

namespace CreateAndFake.Toolbox.DuplicatorTool.CopyHints;

/// <summary>Handles cloning <see cref="ISerializable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class SerializableCopyHint : CopyHint
{
    /// <summary>Scope used to search for inner types.</summary>
    private const BindingFlags _Scope
        = BindingFlags.Public
        | BindingFlags.NonPublic
        | BindingFlags.Instance;

    /// <inheritdoc/>
    protected internal override (bool, object?) TryCopy(object source, DuplicatorChainer duplicator)
    {
        if (source is ISerializable)
        {
            HashSet<object> knownData = [];
            FlattenData(source, knownData, duplicator);

            DataContractSerializer serializer = new(source.GetType(), knownData.Select(d => d.GetType()).Distinct());

            using MemoryStream stream = new();

            serializer.WriteObject(stream, source);
            _ = stream.Seek(0, SeekOrigin.Begin);
            return (true, serializer.ReadObject(stream));
        }
        else
        {
            return (false, null);
        }
    }

    /// <summary>Finds data associated with <paramref name="source"/></summary>
    /// <param name="source">Instance being serialized.</param>
    /// <param name="foundData">Set to populate with found data.</param>
    /// <param name="duplicator">Handles cloning child values.</param>
    private static void FlattenData(object? source, HashSet<object> foundData, DuplicatorChainer duplicator)
    {
        if (source != null
            && foundData.Add(source)
            && !duplicator.Options.SerializableTypes.Contains(source.GetType()))
        {
            FlattenComplexData(source, foundData, duplicator);
        }
    }

    /// <summary>Finds nested data associated with <paramref name="source"/>.</summary>
    /// <param name="source">Instance being serialized.</param>
    /// <param name="foundData">Set to populate with found data.</param>
    /// <param name="duplicator">Handles cloning child values.</param>
    private static void FlattenComplexData(object source, HashSet<object> foundData, DuplicatorChainer duplicator)
    {
        if (source is IDictionary map)
        {
            foreach (DictionaryEntry item in map)
            {
                FlattenData(item.Key, foundData, duplicator);
                FlattenData(item.Value, foundData, duplicator);
            }
        }
        else if (source is IEnumerable values)
        {
            IEnumerator gen = values.GetEnumerator();
            while (gen.MoveNext())
            {
                FlattenData(gen.Current, foundData, duplicator);
            }
        }
        else if (!source.GetType().IsValueType)
        {
            FlattenInnerData(source, foundData, duplicator);
        }
    }

    /// <summary>Finds member data inside <paramref name="source"/>.</summary>
    /// <param name="source">Instance being serialized.</param>
    /// <param name="foundData">Set to populate with found data.</param>
    /// <param name="duplicator">Handles cloning child values.</param>
    private static void FlattenInnerData(object source, HashSet<object> foundData, DuplicatorChainer duplicator)
    {
        Type type = source.GetType();

        foreach (PropertyInfo property in type.GetProperties(_Scope).Where(p => p.CanRead))
        {
            FlattenData(property.GetValue(source), foundData, duplicator);
        }

        foreach (FieldInfo field in type.GetFields(_Scope))
        {
            FlattenData(field.GetValue(source), foundData, duplicator);
        }
    }
}
