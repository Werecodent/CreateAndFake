using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Hints;

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

    /// <inheritdoc/>
    protected override async Task<bool> ExtractAsync(
        object source,
        IExtractorChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (await chainer.AddFoundValueAsync(source, canceler).ConfigureAwait(false))
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
                _ = await chainer
                    .InnerExtractAsync(property.GetValue(source), canceler)
                    .ConfigureAwait(false);
            }
            foreach (
                FieldInfo field in chainer.Options.ExtractPrivateMembers
                    ? TypeDescriber.For(type).Fields.All
                    : TypeDescriber.For(type).Fields.OnlyPublic
            )
            {
                _ = await chainer
                    .InnerExtractAsync(field.GetValue(source), canceler)
                    .ConfigureAwait(false);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
