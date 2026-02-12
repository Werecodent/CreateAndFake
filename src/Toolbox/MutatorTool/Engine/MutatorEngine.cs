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
                .Options.VariantAttempts.StallUntil(
                    $"Create variant of type '{type}'",
                    () => chainer.Options.Randomizer.Create(type),
                    isVariantCheck
                )
                .Last();
        }
        catch (Exception e)
        {
            throw WrapError(type, "create a variant", e);
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
                .Options.VariantAttempts.StallUntil(
                    $"Create unique of type '{type}'",
                    () => chainer.Options.Randomizer.Create(type),
                    isUniqueCheck
                )
                .Last();
        }
        catch (Exception e)
        {
            throw WrapError(type, "create a unique", e);
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
            throw WrapError(instance.GetType(), "modify", e);
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

    /// <summary>Adds details to encountered exceptions during mutation.</summary>
    /// <param name="type">Relevant <see cref="Type"/> causing the issue.</param>
    /// <param name="method">Method type that failed.</param>
    /// <param name="e">Encountered exception.</param>
    /// <returns>Exception to throw.</returns>
    private static ToolException WrapError(Type type, string method, Exception e)
    {
        Exception error =
            (e is AggregateException agg && agg.InnerExceptions.Count == 1)
                ? agg.InnerException ?? e
                : e;

        string message;
        if (error is InsufficientExecutionStackException)
        {
            message = $"Ran into infinite generation trying to {method} instance of type '{type}'.";
        }
        else if (error is TimeoutException)
        {
            message = $"Could not {method} instance of type '{type}' within the limit.";
        }
        else
        {
            message = $"Encountered issue trying to {method} instance of type '{type}'.";
        }
        return new ToolException(message, error);
    }
}
