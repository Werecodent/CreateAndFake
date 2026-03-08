using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting objects for <see cref="IExtractor"/>.</summary>
public sealed class ObjectExtractHint : ExtractHint<object>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ExtractPriority.ObjectHint;

    /// <inheritdoc/>
    protected override bool Extract(object source, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (chainer.AddFoundValue(source))
        {
            Type type = source.GetType();
            foreach (
                PropertyInfo property in (
                    chainer.Options.ExtractPrivateMembers
                        ? InheritanceTracker.For(type).AllProperties
                        : InheritanceTracker.For(type).GetPublicProperties()
                )
                    .Where(p => p.CanRead)
                    .Where(p => source is not Exception || p.Name != "HResult")
            )
            {
                _ = chainer.InnerExtract(property.GetValue(source));
            }
            foreach (
                FieldInfo field in chainer.Options.ExtractPrivateMembers
                    ? InheritanceTracker.For(type).AllFields
                    : InheritanceTracker.For(type).GetPublicFields()
            )
            {
                _ = chainer.InnerExtract(field.GetValue(source));
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
