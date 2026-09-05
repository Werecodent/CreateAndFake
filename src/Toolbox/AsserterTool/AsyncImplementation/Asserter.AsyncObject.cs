using System.Text;
using Werecodent.CreateAndFake.AsserterTool.AsyncCategories;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterAsyncObject
{
    /// <inheritdoc/>
    public Task IsAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ValuesEqualAsync(expected, actual, canceler, details);
    }

    /// <inheritdoc/>
    public Task IsAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ValuesEqualAsync(expected, actual, canceler, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public Task IsNotAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ValuesNotEqualAsync(expected, actual, canceler, details);
    }

    /// <inheritdoc/>
    public Task IsNotAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ValuesNotEqualAsync(expected, actual, canceler, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task ValuesEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ValuesEqualAsync(expected, actual, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ValuesEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        IList<Difference> differences = await AsyncSeriesHelper
            .ToListAsync(
                localOptions.Valuer.CompareAsync(expected, actual, canceler),
                localOptions.Valuer.Options.IterationLimit,
                canceler
            )
            .ConfigureAwait(false);

        if (differences.Count > 0)
        {
            throw new AssertException(
                $"Value equality failed for type '{GetTypeName(expected, actual)}'.",
                details,
                localOptions.Gen.InitialSeed,
                string.Join(Environment.NewLine, differences)
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task ValuesNotEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ValuesNotEqualAsync(expected, actual, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ValuesNotEqualAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (await localOptions.Valuer.EqualsAsync(expected, actual, canceler).ConfigureAwait(false))
        {
            throw new AssertException(
                $"Value inequality failed for type '{GetTypeName(expected, actual)}'.",
                details,
                localOptions.Gen.InitialSeed,
                expected?.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task AreUniqueAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    )
    {
        return AreUniqueAsync(expected, actual, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task AreUniqueAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        ReferenceNotEqual(expected, actual, details);

        int i = 0;
        StringBuilder contents = new();
        await foreach (
            object value in (
                await localOptions.Extractor.ExtractAsync(actual, canceler).ConfigureAwait(false)
            )
                .FindSharedContentAsync(
                    await localOptions
                        .Extractor.ExtractAsync(expected, canceler)
                        .ConfigureAwait(false),
                    canceler
                )
                .ConfigureAwait(false)
        )
        {
            _ = contents.Append('#').Append(i++).Append(':').Append(value).AppendLine();
        }

        if (i != 0)
        {
            throw new AssertException(
                $"Expected no shared content, but had '{i}' shared items.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }
}
