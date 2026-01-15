using System.Reflection;
using System.Runtime.Serialization;
using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;
using CreateAndFake.DuplicatorTool.Engine;

#pragma warning disable SYSLIB0050 // 'IObjectReference' is obsolete: Still needed for compatibility.

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning basic types for <see cref="IDuplicator"/> .</summary>
public sealed class BasicCopyHint : CopyHint
{
    /// <summary>Specific types to control via this hint.</summary>
    private static readonly HashSet<Type> _SupportedTypes =
    [
        typeof(string),
        typeof(object),
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

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));

        Type type = source.GetType();
        if (
            type.IsPrimitive
            || type.IsEnum
            || ValueRandom.ValueTypes.Contains(type)
            || _SupportedTypes.Contains(type)
            || type.Inherits<IObjectReference>()
        )
        {
            return new(source);
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}

#pragma warning restore SYSLIB0050 // 'IObjectReference' is obsolete
