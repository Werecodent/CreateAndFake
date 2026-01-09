using System.Text;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsyncObjectAsserter
{
    /// <inheritdoc/>
    public Task IsAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        string? details = null
    )
    {
        return IsAsync(expected, actual, canceler, Unconfigured, details);
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
        return IsNotAsync(expected, actual, canceler, Unconfigured, details);
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

        IList<Difference> differences = await AsyncEnumHelper
            .ToListAsync(localOptions.Valuer.CompareAsync(expected, actual), canceler)
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
    public virtual Task AreUniqueAsync(
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
        foreach (
            object value in localOptions
                .Extractor.Extract(actual)
                .FindSharedContent(localOptions.Extractor.Extract(expected))
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

        return Task.CompletedTask;
    }
}
