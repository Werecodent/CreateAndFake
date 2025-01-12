using System.Collections;
using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.DuplicatorTool.CopyHints;

/// <summary>Handles cloning immutable collection types for <see cref="IDuplicator"/> .</summary>
public class FrozenCollectionCopyHint : CopyHint
{
    /// <summary>Constructs frozen sets.</summary>
    private static readonly MethodInfo _SetMaker = typeof(FrozenSet)
        .GetMethod(nameof(FrozenSet.ToFrozenSet), BindingFlags.Public | BindingFlags.Static)!;

    /// <summary>Constructs frozen dictionaries.</summary>
    private static readonly MethodInfo _DictionaryMaker = typeof(FrozenDictionary)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(m => m.Name == nameof(FrozenDictionary.ToFrozenDictionary) && m.GetParameters().Length == 2);

    /// <summary>Copies generic item data.</summary>
    private static readonly MethodInfo _CopyContentsHelper = typeof(FrozenCollectionCopyHint)
        .GetMethod(nameof(CopyContentsHelper), BindingFlags.NonPublic | BindingFlags.Static)!;

    /// <inheritdoc/>
    protected internal override (bool, object?) TryCopy(object source, DuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));

        Type type = source.GetType();

        if (type.Inherits(typeof(FrozenSet<>)))
        {
            Type itemType = type.GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()
                .Single();

            return (true, _SetMaker
                .MakeGenericMethod(itemType)
                .Invoke(null, [_CopyContentsHelper
                    .MakeGenericMethod(itemType)
                    .Invoke(null, [source, duplicator]),
                    null]));
        }
        else if (type.Inherits(typeof(FrozenDictionary<,>)))
        {
            Type itemType = type.GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()
                .Single();

            return (true, _DictionaryMaker
                .MakeGenericMethod(itemType.GetGenericArguments())
                .Invoke(null, [_CopyContentsHelper
                    .MakeGenericMethod(itemType)
                    .Invoke(null, [source, duplicator]),
                    null]));
        }
        else
        {
            return (false, null);
        }
    }

    /// <summary>Copies the contents of <paramref name="source"/>.</summary>
    /// <param name="source">Collection with contents to copy.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <returns>The duplicate object.</returns>
    private static T?[] CopyContentsHelper<T>(IEnumerable<T?> source, DuplicatorChainer duplicator)
    {
        List<T?> copy = [];

        IEnumerator enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            copy.Add((T?)duplicator.Copy(enumerator.Current));
        }

        return [.. copy];
    }
}

