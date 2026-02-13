using System.Runtime.Serialization;
using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;

#pragma warning disable SYSLIB0050 // 'IObjectReference' is obsolete: Still needed for compatibility.

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning basic types for <see cref="IDuplicator"/>.</summary>
public sealed class BasicCopyHint : CopyHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.BasicHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [];

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source);

        Type type = source.GetType();
        if (type.IsEnum || type.IsPrimitive || type.Inherits<IObjectReference>())
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
