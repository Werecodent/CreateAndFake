using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning basic types for <see cref="IDuplicator"/> .</summary>
public sealed class TypeInfoCopyHint : CopyHint
{
    /// <summary>Specific types to control via this hint.</summary>
    private static readonly HashSet<Type> _SupportedTypes =
    [
        typeof(MemberInfo),
        typeof(MethodBase),
        typeof(Type),
        typeof(Type).GetType(),
        typeof(ConstructorInfo),
        typeof(string).GetConstructors()[0].GetType(),
        typeof(MethodInfo),
        typeof(string).GetMethods()[0].GetType(),
        typeof(PropertyInfo),
        typeof(string).GetProperties()[0].GetType(),
        typeof(FieldInfo),
        typeof(string).GetFields()[0].GetType(),
        typeof(ParameterInfo),
        typeof(string).GetMethods().SelectMany(m => m.GetParameters()).First().GetType(),
    ];

    /// <summary>Types that the hint can copy.</summary>
    internal static IEnumerable<Type> SupportedTypes { get; } = _SupportedTypes.ToFrozenSet();

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));

        Type type = source.GetType();
        if (_SupportedTypes.Any(t => type.Inherits(t)))
        {
            return new(source);
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}
