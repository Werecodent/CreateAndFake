using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.TesterTool;

/// <summary>Handles generic resolution.</summary>
internal static class GenericFixer
{
    /// <summary>Defines any generics in a method.</summary>
    /// <param name="method">Method to fix.</param>
    /// <param name="options"></param>
    /// <returns>Method with all generics defined.</returns>
    internal static MethodInfo FixMethod(MethodInfo method, TesterOptions options)
    {
        ArgumentGuard.ThrowIfNull(method, nameof(method));
        ArgumentGuard.ThrowIfNull(options, nameof(options));

        return method.IsGenericMethodDefinition
            ? method.MakeGenericMethod(
                [.. method.GetGenericArguments().Select(arg => CreateArg(arg, method, options))]
            )
            : method;
    }

    /// <summary>Creates a concrete arg type from the given generic arg.</summary>
    /// <param name="type">Generic arg to create.</param>
    /// <param name="method">Method with the generics.</param>
    /// <param name="options"></param>
    /// <returns>Created arg <c>Type</c>.</returns>
    private static Type CreateArg(Type type, MethodInfo method, TesterOptions options)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

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
            arg = options.Gen.NextItem(ValueRandom.ValueTypes);
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
                $"Creating generic arguments of type '{type}' for method '{method}' [Retry]",
                () =>
                    Limiter.Few.StallUntil(
                        $"Trying arguments of type '{type}' for method '{method}' [Stall]",
                        () => arg = CreateArgViaConstraint(constraints, options),
                        isValidArg
                    )
            );
        }

        return arg;
    }

    /// <summary>Creates an arg type from the given constraints.</summary>
    /// <param name="constraints">Constraints limiting the arg type.</param>
    /// <param name="options"></param>
    /// <returns>Created arg <c>Type</c>.</returns>
    private static Type CreateArgViaConstraint(Type[] constraints, TesterOptions options)
    {
        Type constraint = options.Gen.NextItem(constraints);

        object sample = options.Randomizer.Create(constraint);
        Type result = sample.GetType();
        Disposer.Cleanup(sample);
        return result;
    }
}
