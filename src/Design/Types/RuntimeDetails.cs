using System.Reflection;
using CreateAndFake.Design.Comparisons;

namespace CreateAndFake.Design.Types;

/// <summary>Provides information only available at runtime.</summary>
public static class RuntimeDetails
{
    /// <summary>The used runtime <see cref="Type"/>.</summary>
    public static Type RuntimeType { get; } = typeof(Type).GetType();

    /// <summary>The used runtime <see cref="ConstructorInfo"/>.</summary>
    public static Type RuntimeConstructorInfoType { get; } =
        typeof(RuntimeData).GetConstructors()[0].GetType();

    /// <summary>The used runtime <see cref="MethodInfo"/>.</summary>
    public static Type RuntimeMethodInfoType { get; } =
        typeof(RuntimeData).GetMethod(nameof(RuntimeData.TestCompare))!.GetType();

    /// <summary>The used runtime <see cref="PropertyInfo"/>.</summary>
    public static Type RuntimePropertyInfoType { get; } =
        typeof(RuntimeData).GetProperty(nameof(RuntimeData.Data))!.GetType();

    /// <summary>The used runtime <see cref="FieldInfo"/>.</summary>
    /// <remarks>See <see cref="MdFieldInfoType"/> for <see langword="const"/>s.</remarks>
    public static Type RtFieldInfoType { get; } =
        typeof(RuntimeData).GetField(nameof(RuntimeData._data))!.GetType();

    /// <summary>The used runtime <see cref="FieldInfo"/> for <see langword="const"/>s.</summary>
    /// <remarks>Inherits the <see langword="return"/> from <see cref="RtFieldInfoType"/>.</remarks>
    public static Type MdFieldInfoType { get; } =
        typeof(RuntimeData).GetField(nameof(RuntimeData._Default))!.GetType();

    /// <summary>The used runtime <see cref="ParameterInfo"/>.</summary>
    public static Type RuntimeParameterInfoType { get; } =
        typeof(RuntimeData)
            .GetMethod(nameof(RuntimeData.TestCompare))!
            .GetParameters()[0]
            .GetType();

    /// <summary>The used runtime <see cref="Assembly"/>.</summary>
    public static Type RuntimeAssemblyType { get; } = Assembly.GetExecutingAssembly().GetType();

    /// <summary>All provided runtime <see cref="Type"/>s.</summary>
    public static IEnumerable<Type> RuntimeTypes { get; } =
    [
        RuntimeType,
        RuntimeConstructorInfoType,
        RuntimeMethodInfoType,
        RuntimePropertyInfoType,
        RtFieldInfoType,
        MdFieldInfoType,
        RuntimeParameterInfoType,
        RuntimeAssemblyType,
    ];

    /// <summary>Validates the runtime <see cref="Type"/> provider class works.</summary>
    /// <inheritdoc cref="ValueComparer.Equals(object?, object?)"/>
    internal static bool InnerClassValidation(string? x, string? y)
    {
        return new RuntimeData(x).TestCompare(new(y));
    }

    /// <summary>Provides concrete runtime <see cref="Type"/>s via inspection.</summary>
    /// <param name="data">Vehicle for providing members to inspect.</param>
    private sealed class RuntimeData(string? data)
    {
        /// <summary><see cref="MdFieldInfoType"/></summary>
        public const string _Default = "";

        /// <summary><see cref="RtFieldInfoType"/></summary>
        public readonly string _data = data ?? _Default;

        /// <summary><see cref="RuntimePropertyInfoType"/></summary>
        public string Data => _data;

        /// <summary><see cref="RuntimeMethodInfoType"/></summary>
        /// <param name="other"><see cref="RuntimeParameterInfoType"/></param>
        /// <returns>Results of comparison via <see cref="Data"/>.</returns>
        public bool TestCompare(RuntimeData other)
        {
            return Data.Equals(other.Data, StringComparison.Ordinal);
        }
    }
}
