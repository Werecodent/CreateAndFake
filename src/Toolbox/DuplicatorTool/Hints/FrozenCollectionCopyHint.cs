using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning immutable collection types for <see cref="IDuplicator"/> .</summary>
public class FrozenCollectionCopyHint : CopyHint
{
    /// <summary>Constructs frozen sets.</summary>
    private static readonly MethodInfo _SetMaker = typeof(FrozenSet).GetMethod(
        nameof(FrozenSet.ToFrozenSet),
        BindingFlags.Public | BindingFlags.Static
    )!;

    /// <summary>Constructs frozen dictionaries.</summary>
    private static readonly MethodInfo _DictionaryMaker = typeof(FrozenDictionary)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(m => m.Name == nameof(FrozenDictionary.ToFrozenDictionary))
        .Single(m => m.GetParameters().Length == 2);

    /// <summary>Copies generic item data.</summary>
    private static readonly MethodInfo _CopyContentsHelper =
        typeof(FrozenCollectionCopyHint).GetMethod(
            nameof(CopyContentsHelper),
            BindingFlags.NonPublic | BindingFlags.Static
        )!;

    /// <inheritdoc/>
    public override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));

        Type type = source.GetType();

        if (type.Inherits(typeof(FrozenSet<>)))
        {
            Type itemType = type.GetInterfaces()
                .Single(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                )
                .GetGenericArguments()
                .Single();

            return new(
                _SetMaker
                    .MakeGenericMethod(itemType)
                    .Invoke(
                        null,
                        [
                            _CopyContentsHelper
                                .MakeGenericMethod(itemType)
                                .Invoke(null, [source, duplicator]),
                            null,
                        ]
                    )
            );
        }
        else if (type.Inherits(typeof(FrozenDictionary<,>)))
        {
            Type itemType = type.GetInterfaces()
                .Single(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                )
                .GetGenericArguments()
                .Single();

            return new(
                _DictionaryMaker
                    .MakeGenericMethod(itemType.GetGenericArguments())
                    .Invoke(
                        null,
                        [
                            _CopyContentsHelper
                                .MakeGenericMethod(itemType)
                                .Invoke(null, [source, duplicator]),
                            null,
                        ]
                    )
            );
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    /// <summary>Copies the contents of <paramref name="source"/>.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">Collection with contents to copy.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <returns>The duplicate object.</returns>
    private static T?[] CopyContentsHelper<T>(IEnumerable<T?> source, IDuplicatorChainer duplicator)
    {
        List<T?> copy = [];

        foreach (T? item in source)
        {
            copy.Add(duplicator.Copy(item));
        }

        return [.. copy];
    }
}
