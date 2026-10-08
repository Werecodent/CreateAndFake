using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning collections for <see cref="IDuplicator"/> .</summary>
public sealed class CollectionCopyHint : CopyHint
{
    /// <summary>Special cases where the data needs to be reversed.</summary>
    private static readonly FrozenSet<Type> _ReverseCases = FrozenSet.ToFrozenSet([
        typeof(ConcurrentStack<>),
        typeof(Stack<>),
        typeof(Stack),
    ]);

    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.CollectionHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(IEnumerable)];

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator);

        if (source is IEnumerable collection)
        {
            IEnumerable? result = Copy(collection, duplicator);
            if (result != null)
            {
                return new(result);
            }
        }
        return CopyHintResult.None;
    }

    private static IEnumerable? Copy(IEnumerable source, IDuplicatorChainer duplicator)
    {
        Type type = source.GetType();

        Type itemType =
            GenericConverter.AsConcreteType(type, typeof(IEnumerable<>))?.GetGenericArguments()[0]
            ?? type.GetElementType()
            ?? typeof(object);

        Array contents = CopyContents(
            source,
            itemType,
            duplicator,
            _ReverseCases.Contains(GenericConverter.AsGenericBase(type) ?? type)
        );

        return MakeCollection(contents, type, itemType, duplicator);
    }

    private static IEnumerable? MakeCollection(
        Array contents,
        Type collectionType,
        Type itemType,
        IDuplicatorChainer duplicator
    )
    {
        if (collectionType.IsArray)
        {
            return contents;
        }
        else if (GenericConverter.AsGenericBase(collectionType) == typeof(Dictionary<,>))
        {
            // Constructor missing in .NET 4.8.
            dynamic result = Activator.CreateInstance(collectionType)!;
            foreach (dynamic item in contents)
            {
                result.Add(item.Key, item.Value);
            }
            return result;
        }

        foreach (
            ConstructorInfo constructor in TypeDescriber
                .For(collectionType)
                .Constructors.OnlyPublic.Where(c => c.GetParameters().Length == 1)
                .Where(c =>
                {
                    Type requiredArg = c.GetParameters()[0].ParameterType;
                    return requiredArg != collectionType && requiredArg.Inherits<IEnumerable>();
                })
        )
        {
            Type requiredArg = constructor.GetParameters()[0].ParameterType;

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

    /// <summary>Copies the contents of <paramref name="source"/>.</summary>
    /// <param name="source">Collection with contents to copy.</param>
    /// <param name="itemType">Collection item type.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <param name="reverse">If the copy process should reverse the order of items from the enumerator.</param>
    /// <returns>The duplicate object.</returns>
    /// <exception cref="IterationLimitException"></exception>
    private static Array CopyContents(
        IEnumerable source,
        Type itemType,
        IDuplicatorChainer duplicator,
        bool reverse
    )
    {
        List<object?> copy = [];

        int index = 0;
        foreach (object item in source)
        {
            ArgumentGuard.ThrowUponIterationLimit(
                index++,
                duplicator.Options.Valuer.Options.IterationLimit
            );
            copy.Add(duplicator.Copy(item));
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
