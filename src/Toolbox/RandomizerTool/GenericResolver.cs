using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.RandomizerTool.Handlers;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.RandomizerTool;

/// <summary>Fulfills generics for generic type definitions.</summary>
public static class GenericResolver
{
    /// <summary>All possible <see cref="Type"/>s that have default constructors.</summary>
    private static readonly FrozenSet<Type> _HasDefaultConstructor = InheritanceTracker
        .For<object>()
        .FindLoadedSubclasses()
        .Where(t => t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null)
        .ToFrozenSet();

    /// <summary>Generic choices when there are no constraints.</summary>
    private static readonly ImmutableArray<Type> _DefaultGenerics =
    [
        typeof(int),
        typeof(double),
        typeof(string),
        typeof(TimeSpan),
    ];

    /// <summary>Defines the <paramref name="method"/> with randomized generics.</summary>
    /// <param name="method">Potential generic method definition to concretely define.</param>
    /// <param name="randomizer">Handles randomization</param>
    /// <returns>The <paramref name="method"/> with generics specified.</returns>
    /// <exception cref="UnsupportedException">If no possible generics are found.</exception>
    [return: NotNullIfNotNull(nameof(method))]
    public static MethodInfo? OfConcrete(MethodInfo? method, IRandomizer randomizer)
    {
        if (method?.IsGenericMethodDefinition ?? false)
        {
            return CreateConcreteGenerics(method, randomizer).FirstOrDefault()
                ?? throw new UnsupportedException($"Could not craft generic '{method}'.");
        }
        else
        {
            return method;
        }
    }

    /// <summary>Defines the <paramref name="type"/> with randomized generics.</summary>
    /// <param name="type">Potential generic type definition to concretely define.</param>
    /// <param name="randomizer">Handles randomization.</param>
    /// <returns>The <paramref name="type"/> with generics specified.</returns>
    /// <exception cref="UnsupportedException">If no possible generics are found.</exception>
    [return: NotNullIfNotNull(nameof(type))]
    public static Type? OfConcrete(Type? type, IRandomizerChainer randomizer)
    {
        if (type?.IsGenericTypeDefinition ?? false)
        {
            return CreateConcreteGenerics(type, randomizer).FirstOrDefault()
                ?? throw new UnsupportedException(
                    $"Could not craft generic '{TypeDescriber.ExpandedName(type)}'."
                );
        }
        else
        {
            return type;
        }
    }

    /// <param name="method">Generic method definition to concretely define.</param>
    /// <exception cref="ArgumentException">
    ///     If <paramref name="method"/> is not a generic definition.
    /// </exception>
    /// <inheritdoc cref="CraftGenerics"/>
    [return: NotNullIfNotNull(nameof(method))]
    public static IEnumerable<MethodInfo> CreateConcreteGenerics(
        MethodInfo method,
        IRandomizer randomizer
    )
    {
        ArgumentGuard.ThrowIfNull(method, randomizer);

        if (!method.IsGenericMethodDefinition)
        {
            throw new ArgumentException(
                $"Type '{method}' was not a generic method definition.",
                nameof(method)
            );
        }

        return CraftGenerics(
            method.GetGenericArguments(),
            _ => true,
            method.MakeGenericMethod,
            randomizer
        );
    }

    /// <param name="type">Generic type definition to concretely define.</param>
    /// <exception cref="ArgumentException">
    ///     If <paramref name="type"/> is not a generic definition.
    /// </exception>
    /// <inheritdoc cref="CraftGenerics"/>
    public static IEnumerable<Type> CreateConcreteGenerics(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(type, randomizer);

        if (!type.IsGenericTypeDefinition)
        {
            throw new ArgumentException(
                $"Type '{TypeDescriber.ExpandedName(type)}' was not a generic type definition.",
                nameof(type)
            );
        }

        if (!CollectionCreateHint.PotentialCollections.Contains(type.GetGenericTypeDefinition()))
        {
            List<Type> directImplementations =
            [
                .. FindSubclasses(type).Where(t => !randomizer.AlreadyCreated(t)),
            ];

            if (directImplementations.Count != 0)
            {
                return directImplementations;
            }
        }

        return CraftGenerics(
            type.GetGenericArguments(),
            t => !randomizer.AlreadyCreated(t),
            type.MakeGenericType,
            randomizer
        );
    }

    /// <summary>Generates possible generics.</summary>
    /// <typeparam name="T"><see cref="Type"/> of generics to generate.</typeparam>
    /// <param name="genericArguments">Generic arguments to populate.</param>
    /// <param name="argValidityCheck">Condition to force all arguments to fulfill.</param>
    /// <param name="maker">Method to generate a generic from specified arguments.</param>
    /// <param name="randomizer">Handles sequence randomization.</param>
    /// <returns>The generated generics.</returns>
    /// <remarks>Initial generic arguments are randomized, then sequentially applied.</remarks>
    private static IEnumerable<T> CraftGenerics<T>(
        Type[] genericArguments,
        Func<Type, bool> argValidityCheck,
        Func<Type[], T> maker,
        IRandomizer randomizer
    )
        where T : class
    {
        List<List<Type>> possibleArgsPerArg =
        [
            .. genericArguments.Select(t =>
                randomizer
                    .Options.Gen.NextSequence(FindPossibleArgs(t).Where(argValidityCheck))
                    .ToList()
            ),
        ];

        foreach (int[] indexes in FindAllIndexes(possibleArgsPerArg))
        {
            Type[] nextArgSet = new Type[genericArguments.Length];
            for (int i = 0; i < nextArgSet.Length; i++)
            {
                nextArgSet[i] = possibleArgsPerArg[i][indexes[i]];
            }

            T? nextGeneric = null;
            try
            {
                nextGeneric = maker.Invoke(nextArgSet);
            }
            catch
            {
                // Try next generic upon failure.
            }

            if (nextGeneric != null)
            {
                yield return nextGeneric;
            }
        }
    }

    /// <summary>
    ///     Finds all possible index combinations for the <paramref name="possibleArgsPerArg"/>.
    /// </summary>
    /// <param name="possibleArgsPerArg">Collection of possible items for each position.</param>
    /// <returns>Every possible index combination.</returns>
    /// <remarks>Performance optimized: Returned array is reused for each yielded result.</remarks>
    private static IEnumerable<int[]> FindAllIndexes(IEnumerable<List<Type>> possibleArgsPerArg)
    {
        int[] argLengths = [.. possibleArgsPerArg.Select(a => a.Count)];
        int[] currentIndexes = [.. Enumerable.Repeat(0, argLengths.Length)];

        if (argLengths.Any(v => v == 0))
        {
            yield break;
        }

        int lastAddedIndex = currentIndexes.Length;
        while (lastAddedIndex >= 0)
        {
            yield return currentIndexes;

            for (int i = currentIndexes.Length - 1; i >= 0; i--)
            {
                currentIndexes[i] += 1;
                if (currentIndexes[i] < argLengths[i])
                {
                    break;
                }
                else
                {
                    lastAddedIndex = i - 1;
                    currentIndexes[i] = 0;
                }
            }
        }
    }

    /// <summary>Finds possible <see cref="Type"/>s to fulfill a generic argument.</summary>
    /// <param name="type">Generic argument to match.</param>
    /// <returns>The possible <see cref="Type"/>s.</returns>
    private static IEnumerable<Type> FindPossibleArgs(Type type)
    {
        ArgumentGuard.ThrowIfNull(type);

        IEnumerable<Type> possibleTypes = _DefaultGenerics;

        Type[] constraints =
        [
            .. type.GetGenericParameterConstraints()
                .Where(t => t != typeof(ValueType))
                .Select(t => TypeDescriber.AsGenericBase(t) ?? t),
        ];
        if (constraints.Length != 0)
        {
            HashSet<Type> constraintCompatibles = [.. FindSubclasses(constraints[0])];
            foreach (IEnumerable<Type> set in constraints.Skip(1).Select(FindSubclasses))
            {
                constraintCompatibles.IntersectWith(set);
            }
            possibleTypes = constraintCompatibles;
        }

        if (
            type.GenericParameterAttributes.HasFlag(
                GenericParameterAttributes.NotNullableValueTypeConstraint
            )
        )
        {
            possibleTypes = possibleTypes.Where(t => t.IsValueType);
        }

        if (
            type.GenericParameterAttributes.HasFlag(
                GenericParameterAttributes.DefaultConstructorConstraint
            )
        )
        {
            possibleTypes = possibleTypes.Where(_HasDefaultConstructor.Contains);
        }

        return possibleTypes;
    }

    /// <summary>Finds usable subclasses for the <paramref name="type"/>.</summary>
    /// <param name="type"><see cref="Type"/> to find subclasses for.</param>
    /// <returns>The found subclasses.</returns>
    private static IEnumerable<Type> FindSubclasses(Type type)
    {
        if (type == typeof(Exception))
        {
            return ExceptionCreateHandlers.PotentialExceptions;
        }
        else
        {
            return InheritanceTracker.For(type).FindLoadedSubclasses();
        }
    }
}
