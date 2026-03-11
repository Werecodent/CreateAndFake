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
                        ? TypeDescriber.For(type).Properties.All
                        : TypeDescriber.For(type).Properties.OnlyPublic
                )
                    .Where(p => p.CanRead)
                    .Where(p => source is not Exception || p.Name != "HResult")
            )
            {
                _ = chainer.InnerExtract(property.GetValue(source));
            }
            foreach (
                FieldInfo field in chainer.Options.ExtractPrivateMembers
                    ? TypeDescriber.For(type).Fields.All
                    : TypeDescriber.For(type).Fields.OnlyPublic
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
