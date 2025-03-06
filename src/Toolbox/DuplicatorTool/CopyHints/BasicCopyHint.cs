using System.Runtime.Serialization;
using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;

#pragma warning disable SYSLIB0050 // 'IObjectReference' is obsolete: Still needed for compatibility.

namespace CreateAndFake.DuplicatorTool.CopyHints;

/// <summary>Handles cloning basic types for <see cref="IDuplicator"/> .</summary>
public sealed class BasicCopyHint : CopyHint
{
    /// <summary>Specific types to control via this hint.</summary>
    private static readonly HashSet<Type> _SupportedTypes = [typeof(string), typeof(object)];

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, DuplicatorChainer duplicator)
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
