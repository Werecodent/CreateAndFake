using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
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
                PropertyInfo property in TypeDescriber
                    .GetAllProperties(type, !chainer.Options.ExtractPrivateMembers)
                    .Where(p => p.CanRead)
                    .Where(p => source is not Exception || p.Name != "HResult")
            )
            {
                _ = chainer.InnerExtract(property.GetValue(source));
            }
            foreach (
                FieldInfo field in TypeDescriber.GetAllFields(
                    type,
                    !chainer.Options.ExtractPrivateMembers
                )
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
