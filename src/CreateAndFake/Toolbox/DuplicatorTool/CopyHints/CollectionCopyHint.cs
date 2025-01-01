using System.Collections;
using System.Collections.Concurrent;
using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.DuplicatorTool.CopyHints;

/// <summary>Handles cloning collections for <see cref="IDuplicator"/> .</summary>

public sealed class CollectionCopyHint : CopyHint<IEnumerable>
{
    /// <summary>Special cases where the data needs to be reversed.</summary>
    private static readonly Type[] _ReverseCases =
    [
        typeof(ConcurrentStack<>),
        typeof(Stack<>),
        typeof(Stack)
    ];

    /// <inheritdoc/>
    protected override IEnumerable Copy(IEnumerable source, DuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));
        ArgumentGuard.ThrowIfNull(duplicator, nameof(duplicator));

        if (source.GetType().IsArray)
        {
            return CopyContents(source, duplicator);
        }
#if LEGACY // Constructor missing in .NET full.
        else if (source.GetType().AsGenericType() == typeof(Dictionary<,>))
        {
            dynamic result = Activator.CreateInstance(source.GetType());
            foreach (dynamic item in CopyContents(source, duplicator))
            {
                result.Add(item.Key, item.Value);
            }
            return result;
        }
#endif
        else
        {
            return (IEnumerable)Activator.CreateInstance(source.GetType(), CopyContents(source, duplicator))!;
        }
    }

    /// <summary>Copies the contents of <paramref name="source"/>.</summary>
    /// <param name="source">Collection with contents to copy.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <returns>Clone of <paramref name="source"/>'s elements.</returns>
    private static IEnumerable CopyContents(IEnumerable source, DuplicatorChainer duplicator)
    {
        Type type = source.GetType();
        Type? genericType = type.AsGenericType();

        object?[] data = CopyContentsHelper(source, duplicator, _ReverseCases.Contains(genericType ?? type));

        if (genericType != null)
        {
            Type[] args = type.GetGenericArguments();
            Type itemType = (args.Length != 1)
                ? typeof(KeyValuePair<,>).MakeGenericType(args)
                : args.Single();

            return ArrayCast(itemType, data);
        }
        else if (type.IsArray)
        {
            return ArrayCast(type.GetElementType()!, data);
        }
        else
        {
            return data;
        }
    }

    /// <summary>Convert <paramref name="data"/> to <paramref name="elementType"/> array.</summary>
    /// <param name="elementType">Array type to create.</param>
    /// <param name="data">Array to convert.</param>
    /// <returns>The converted array.</returns>
    private static Array ArrayCast(Type elementType, object?[] data)
    {
        Array result = Array.CreateInstance(elementType, data.Length);
        for (int i = 0; i < data.Length; i++)
        {
            result.SetValue(data[i], i);
        }
        return result;
    }

    /// <summary>Copies the contents of <paramref name="source"/>.</summary>
    /// <param name="source">Collection with contents to copy.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <param name="reverse">If the copy process should reverse the order of items from the enumerator.</param>
    /// <returns>The duplicate object.</returns>
    private static object?[] CopyContentsHelper(IEnumerable source, DuplicatorChainer duplicator, bool reverse)
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

        return [.. copy];
    }
}
