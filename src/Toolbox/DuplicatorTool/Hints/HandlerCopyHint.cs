using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.DuplicatorTool.Handlers;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning common types for <see cref="IDuplicator"/> .</summary>
public sealed class HandlerCopyHint : CopyHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly ICopyHandler[] _Copiers = [new RefCopyHandler(typeof(TypeDescriber))];

    private static readonly IDictionary<Type, ICopyHandler> _CopiersByType =
        TypeSupporter.GroupBySupportedType(
            _Copiers
                .Concat(SelfCopyHandlers.Handlers)
                .Concat(SystemCopyHandlers.Handlers)
                .Concat(ReflectionCopyHandlers.Handlers)
                .Concat(LegacyCollectionCopyHandlers.Handlers)
        );

    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.HandlerHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => _CopiersByType.Keys;

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator);

        if (
            source != null
            && _CopiersByType.TryGetValue(source.GetType(), out ICopyHandler? copier)
        )
        {
            return new(copier.CopySupported(source, duplicator));
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}
