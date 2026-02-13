using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc/>
public sealed class MutatorEngine : ToolEngine<IMutateHint>, IMutatorEngine
{
    /// <inheritdoc/>
    public object Variant(Type type, object? instance, IMutatorChainer chainer)
    {
        return VariantOf(type, [instance], chainer);
    }

    /// <inheritdoc/>
    public object VariantOf(Type type, IEnumerable<object?> instances, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(type, instances, chainer);

        ValuerOptions compareOptions = chainer.Options.Valuer.Options with
        {
            SkipAsyncValues = true,
        };

        bool isVariantCheck(object result)
        {
            if (
                instances.All(o =>
                    ArgumentGuard.IsAsynchronous(o)
                    || !chainer.Options.Valuer.Equals(result, o, _ => compareOptions)
                )
            )
            {
                return true;
            }
            else
            {
                Disposer.Cleanup(result);
                return false;
            }
        }

        try
        {
            return chainer
                .Options.CreateVariantAttemptLimit.StallUntil(
                    $"Create variant of type '{type}'",
                    () => chainer.Options.Randomizer.Create(type),
                    isVariantCheck
                )
                .Last();
        }
        catch (Exception e)
        {
            throw new ToolException($"Error creating a variant instance of type '{type}'.", e);
        }
    }

    /// <inheritdoc/>
    public object Unique(Type type, object? instance, IMutatorChainer chainer)
    {
        return UniqueOf(type, [instance], chainer);
    }

    /// <inheritdoc/>
    public object UniqueOf(Type type, IEnumerable<object?> instances, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(type, instances, chainer);

        IContentMap[] maps =
        [
            .. instances.Where(e => e != null).Select(e => chainer.Options.Extractor.Extract(e)),
        ];

        bool isUniqueCheck(object result)
        {
            if (!chainer.Options.Extractor.Extract(result).HasSharedContent(maps))
            {
                return true;
            }
            else
            {
                Disposer.Cleanup(result);
                return false;
            }
        }

        try
        {
            return chainer
                .Options.CreateUniqueAttemptLimit.StallUntil(
                    $"Create unique of type '{type}'",
                    () => chainer.Options.Randomizer.Create(type),
                    isUniqueCheck
                )
                .Last();
        }
        catch (Exception e)
        {
            throw new ToolException($"Error creating a unique instance of type '{type}'.", e);
        }
    }

    /// <inheritdoc/>
    public bool Modify(object? instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (instance == null)
        {
            return false;
        }

        MutateHintResult? result;
        try
        {
            result = SelectHints(chainer)
                .Select(h => h.TryModifying(instance, chainer))
                .FirstOrDefault(r => r?.HasData ?? false);
        }
        catch (Exception e)
        {
            throw new ToolException($"Error modifying instance of type '{instance.GetType()}'.", e);
        }

        if (result != null)
        {
            return result.Data;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{instance.GetType()}' not supported by the {nameof(IMutator)}. "
                    + $"Create a {nameof(IMutateHint)} to handle the {nameof(Type)}."
            );
        }
    }
}
