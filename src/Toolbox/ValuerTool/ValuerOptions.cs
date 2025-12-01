using System.Collections.Frozen;
using System.Globalization;
using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool;

/// <summary>Configuration for controlling comparison behavior.</summary>
public sealed record ValuerOptions : ToolHintOptions<ValuerOptions, CompareHint>
{
    /// <summary>Allows <see cref="IEquatable{T}"/> to handle comparisons if applicable.</summary>
    public bool UseEquatableComparisons { get; init; } = true;

    /// <summary>Triggers type checking for collections.</summary>
    /// <remarks>By default, collections are compared by contents and not the container type.</remarks>
    public bool CheckCollectionType { get; init; } = false;

    /// <summary>Excludes <see cref="SeededRandom.Seed"/> from comparison checks.</summary>
    public bool IgnoreCurrentRandomSeed { get; init; } = true;

    /// <summary>Types to use default equality/hashing.</summary>
    public FrozenSet<Type> FallbackTypes { get; init; } =
        FrozenSet.ToFrozenSet([
            typeof(CultureInfo),
            typeof(DateTimeFormatInfo),
            typeof(NumberFormatInfo),
            typeof(CompareInfo),
            /*typeof(MethodBase),
            typeof(MemberInfo),
            typeof(ConstructorInfo),
            typeof(string).GetConstructors()[0].GetType(),
            typeof(MethodInfo),
            typeof(string).GetMethods()[0].GetType(),
            typeof(PropertyInfo),
            typeof(string).GetProperties()[0].GetType(),
            typeof(FieldInfo),
            typeof(string).GetFields()[0].GetType(),
            typeof(ParameterInfo),
            typeof(string).GetMethods().SelectMany(m => m.GetParameters()).First().GetType(),*/
        ]);

    /// <summary>How long to wait for async comparisons to complete.</summary>
    public TimeSpan AsyncTimeout { get; init; } = new(0, 0, 5);

    /// <summary>If asynchronous values should be skipped in synchronous contexts instead of throwing.</summary>
    public bool SkipAsyncValues { get; init; } = false;

    /// <summary>If calculated value hashes should be included in equality comparisons.</summary>
    public bool IncludeValueHashInComparison { get; init; } = true;

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return ValueComparer.Use.GetHashCode(
            IncludeDefaultHints,
            Hints,
            UseEquatableComparisons,
            CheckCollectionType,
            IgnoreCurrentRandomSeed,
            FallbackTypes,
            AsyncTimeout,
            SkipAsyncValues
        );
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(ValuerOptions);
    }
}
