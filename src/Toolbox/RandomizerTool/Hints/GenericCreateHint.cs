using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing generic types for <see cref="IRandomizer"/>.</summary>
public sealed class GenericCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        if (type?.IsGenericTypeDefinition ?? false)
        {
            return new(Create(type, randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <returns>The randomized instance.</returns>
    /// <inheritdoc cref="CreateHint.TryCreate"/>
    private static object Create(Type type, IRandomizerChainer randomizer)
    {
        return randomizer.Create(
            type.MakeGenericType([
                .. type.GetGenericArguments().Select(a => CreateArg(a, type, randomizer)),
            ]),
            type
        );
    }

    /// <summary>Creates a concrete arg type from the given generic arg.</summary>
    /// <param name="type">Generic arg to create.</param>
    /// <param name="parent">Base <see cref="Type"/> being created.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Created arg <see cref="Type"/>.</returns>
    internal static Type CreateArg(Type type, Type parent, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(type);
        ArgumentGuard.ThrowIfNull(randomizer);

        bool newNeeded = type.GenericParameterAttributes.HasFlag(
            GenericParameterAttributes.DefaultConstructorConstraint
        );

        Type arg;
        if (
            type.GenericParameterAttributes.HasFlag(
                GenericParameterAttributes.NotNullableValueTypeConstraint
            )
        )
        {
            arg = randomizer.Options.Gen.NextItem(ValueRandom.SupportedTypes);
        }
        else if (newNeeded)
        {
            arg = typeof(object);
        }
        else
        {
            arg = typeof(string);
        }

        Type[] constraints =
        [
            .. type.GetGenericParameterConstraints()
                .Select(t => t.ContainsGenericParameters ? t.GetGenericTypeDefinition() : t),
        ];

        bool isValidArg()
        {
            return constraints.All(c =>
                    arg.Inherits(c) || (arg.IsValueType && c == typeof(ValueType))
                ) && (!newNeeded || arg.GetConstructor(Type.EmptyTypes) != null || arg.IsValueType);
        }

        if (!isValidArg())
        {
            _ = Limiter.Few.Retry(
                $"Creating generic arguments of type '{type}' for type '{parent}' [Retry]",
                () =>
                    Limiter.Few.StallUntil(
                        $"Trying arguments of type '{type}' for type '{parent}' [Stall]",
                        () => arg = CreateArgViaConstraint(constraints, parent, randomizer),
                        isValidArg
                    )
            );
        }

        return arg;
    }

    /// <summary>Creates an arg type from the given constraints.</summary>
    /// <param name="constraints">Constraints limiting the arg type.</param>
    /// <param name="parent">Base <see cref="Type"/> being created.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Created arg <see cref="Type"/>.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static Type CreateArgViaConstraint(
        Type[] constraints,
        Type parent,
        IRandomizerChainer randomizer
    )
    {
        Type constraint = randomizer.Options.Gen.NextItem(constraints);
        if (parent == constraint)
        {
            return randomizer.Options.Gen.NextItemOrDefault(
                    TypeDescriber.FindLoadedSubclasses(parent)
                )
                ?? throw new InvalidOperationException(
                    $"Cannot create '{parent}' due to self-reference and no visible subclasses."
                );
        }
        else
        {
            object sample = randomizer.Create(constraint);
            Type result = sample.GetType();
            Disposer.Cleanup(sample);
            return result;
        }
    }
}
