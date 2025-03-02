using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.DuplicatorTool.CopyHints;

/// <summary>Handles cloning collections for <see cref="IDuplicator"/> .</summary>

public sealed class CollectionCopyHint : CopyHint
{
    /// <summary>Special cases where the data needs to be reversed.</summary>
    private static readonly FrozenSet<Type> _ReverseCases
        = FrozenSet.ToFrozenSet(
        [
            typeof(ConcurrentStack<>),
            typeof(Stack<>),
            typeof(Stack)
        ]);

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, DuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator, nameof(duplicator));

        if (source is IEnumerable collection)
        {
            IEnumerable? result = Copy(collection, duplicator);
            if (result != null)
            {
                return new(result);
            }
            else
            {
                throw new NotSupportedException(
                    $"Collection '{source.GetType().FullName}' not supported by the duplicator. " +
                    "Create a hint to generate the type and pass it to the duplicator.");
            }
        }
        return CopyHintResult.None;
    }

    /// <inheritdoc/>
    private static IEnumerable? Copy(IEnumerable source, DuplicatorChainer duplicator)
    {
        Type type = source.GetType();
        Type? itemType = FindItemType(type);
        if (itemType == null)
        {
            return null;
        }

        Array contents = CopyContents(source, itemType, duplicator,
            _ReverseCases.Contains(type.AsGenericType() ?? type));

        return MakeCollection(contents, type, itemType, duplicator);
    }

    private static IEnumerable? MakeCollection(Array contents,
        Type collectionType, Type itemType, DuplicatorChainer duplicator)
    {
        if (collectionType.IsArray)
        {
            return contents;
        }
        else if (collectionType.AsGenericType() == typeof(Dictionary<,>))
        {
            dynamic result = Activator.CreateInstance(collectionType)!;
            foreach (dynamic item in contents)
            {
                result.Add(item.Key, item.Value);
            }
            return result;
        }

        ConstructorInfo? constructor = collectionType
            .GetConstructors()
            .Where(c => c.GetParameters().Length == 1)
            .FirstOrDefault(c => c.GetParameters().First().ParameterType.Inherits<IEnumerable>());

        if (constructor != null)
        {
            Type requiredArg = constructor.GetParameters().First().ParameterType;

            object? wrapped = requiredArg.IsInheritedBy(contents.GetType())
                ? contents
                : MakeCollection(contents, requiredArg, itemType, duplicator);

            if (wrapped != null)
            {
                return (IEnumerable)constructor.Invoke([wrapped]);
            }
        }
        return null;
    }

    private static Type? FindItemType(Type type)
    {
        Type[] args = type.IsGenericType ? type.GetGenericArguments() : [];
        switch (args.Length)
        {
            case 2:
                Type pair = typeof(KeyValuePair<,>).MakeGenericType(args);
                return type.Inherits(typeof(IEnumerable<>).MakeGenericType(pair))
                    ? pair
                    : null;
            case 1:
                return args.ElementAt(0);
            case 0:
                return type.GetElementType() ?? typeof(object);
            default:
                return null;
        }
    }

    /// <summary>Copies the contents of <paramref name="source"/>.</summary>
    /// <param name="source">Collection with contents to copy.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <param name="reverse">If the copy process should reverse the order of items from the enumerator.</param>
    /// <returns>The duplicate object.</returns>
    private static Array CopyContents(IEnumerable source, Type itemType, DuplicatorChainer duplicator, bool reverse)
    {
        List<object?> copy = [];

        IEnumerator enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            copy.Add(duplicator.Copy(enumerator.Current));
        }

        if (reverse)
        {
            copy.Reverse();
        }

        Array result = Array.CreateInstance(itemType, copy.Count);
        for (int i = 0; i < copy.Count; i++)
        {
            result.SetValue(copy[i], i);
        }
        return result;
    }
}
