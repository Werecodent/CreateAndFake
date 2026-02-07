using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;

namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc cref="IMutator"/>
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
        ArgumentGuard.ThrowIfNull(instances, chainer);
        try
        {
            return chainer
                .Options.VariantAttempts.StallUntil(
                    $"Create variant of type '{type}'",
                    () => chainer.Options.Randomizer.Create(type),
                    result =>
                    {
                        if (
                            instances.All(o =>
                                ArgumentGuard.IsAsynchronous(o)
                                || !chainer.Options.Valuer.Equals(
                                    result,
                                    o,
                                    opt => opt with { SkipAsyncValues = true }
                                )
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
                )
                .Last();
        }
        catch (TimeoutException e)
        {
            throw new ToolException($"Could not create different instance of type '{type}'.", e);
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
        ArgumentGuard.ThrowIfNull(instances, chainer);

        IContentMap[] maps =
        [
            .. instances.Where(e => e != null).Select(e => chainer.Options.Extractor.Extract(e)),
        ];
        try
        {
            return chainer
                .Options.VariantAttempts.StallUntil(
                    $"Create unique of type '{type}'",
                    () => chainer.Options.Randomizer.Create(type),
                    result =>
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
                )
                .Last();
        }
        catch (TimeoutException e)
        {
            throw new ToolException($"Could not create unique instance of type '{type}'.", e);
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

        MutateHintResult? result = SelectHints(chainer)
            .Select(h => h.TryModifying(instance, chainer))
            .FirstOrDefault(r => r.HasData);

        if (result != null)
        {
            return result.Data;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{instance.GetType()}' not supported by the mutator. "
                    + "Create a hint to generate the type and pass it to the mutator."
            );
        }
    }
}
