using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning immutable collection types for <see cref="IDuplicator"/> .</summary>
public class ImmutableCollectionCopyHint : CopyHint
{
    /// <summary>Collections able to be randomized.</summary>
    private static readonly FrozenDictionary<Type, MethodInfo> _Collections = new Dictionary<
        Type,
        MethodInfo
    >()
    {
        { typeof(ImmutableList<>), FindCreateRangeBuilder(typeof(ImmutableList)) },
        { typeof(ImmutableArray<>), FindCreateRangeBuilder(typeof(ImmutableArray)) },
        { typeof(ImmutableQueue<>), FindCreateRangeBuilder(typeof(ImmutableQueue)) },
        { typeof(ImmutableStack<>), FindCreateRangeBuilder(typeof(ImmutableStack)) },
        { typeof(ImmutableHashSet<>), FindCreateRangeBuilder(typeof(ImmutableHashSet)) },
        { typeof(ImmutableSortedSet<>), FindCreateRangeBuilder(typeof(ImmutableSortedSet)) },
        { typeof(ImmutableDictionary<,>), FindCreateRangeBuilder(typeof(ImmutableDictionary)) },
        {
            typeof(ImmutableSortedDictionary<,>),
            FindCreateRangeBuilder(typeof(ImmutableSortedDictionary))
        },
    }.ToFrozenDictionary();

    /// <summary>Finds the static <c>CreateRange</c> method for a collection.</summary>
    /// <param name="type">Collection type to create.</param>
    /// <returns>Found create method.</returns>
    private static MethodInfo FindCreateRangeBuilder(Type type)
    {
        return type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m =>
                m.Name == "CreateRange"
                && m.GetParameters().Length == 1
                && m.GetParameters()[0].ParameterType.Inherits(typeof(IEnumerable<>))
            );
    }

    /// <summary>Copies generic item data.</summary>
    private static readonly MethodInfo _CopyContentsHelper =
        typeof(ImmutableCollectionCopyHint).GetMethod(
            nameof(CopyContentsHelper),
            BindingFlags.NonPublic | BindingFlags.Static
        )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.ImmutableCollectionHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => _Collections.Keys;

    /// <inheritdoc/>
    public override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source);

        Type type = source.GetType();
        Type? genericType = GenericTypeConverter.AsGenericBase(type);

        if (genericType != null && _Collections.TryGetValue(genericType, out MethodInfo? match))
        {
            Type[] args = type.GetGenericArguments();
            Type itemType =
                (args.Length != 1) ? typeof(KeyValuePair<,>).MakeGenericType(args) : args.Single();

            return new(
                match
                    .MakeGenericMethod(args)
                    .Invoke(
                        null,
                        [
                            _CopyContentsHelper
                                .MakeGenericMethod(itemType)
                                .Invoke(
                                    null,
                                    [source, duplicator, genericType == typeof(ImmutableStack<>)]
                                ),
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
    /// <param name="reverse">If the copy process should reverse the order of items from the enumerator.</param>
    /// <returns>The duplicate object.</returns>
    private static T?[] CopyContentsHelper<T>(
        IEnumerable<T?> source,
        IDuplicatorChainer duplicator,
        bool reverse
    )
    {
        List<T?> copy = [];

        foreach (T? item in source)
        {
            copy.Add(duplicator.Copy(item));
        }

        if (reverse)
        {
            copy.Reverse();
        }

        return [.. copy];
    }
}
