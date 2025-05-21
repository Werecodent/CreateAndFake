using System.Text;
using CreateAndFake.AsserterTool;
using CreateAndFake.AsyncAsserterTool.Categories;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.AsyncAsserterTool;

/// <inheritdoc cref="IAsyncAsserter"/>
public partial class AsyncAsserter : IAsyncObjectAsserter
{
    /// <inheritdoc/>
    public Task Is(object? expected, object? actual, string? details = null)
    {
        return Is(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public Task Is(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ValuesEqual(expected, actual, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public Task IsNot(object? expected, object? actual, string? details = null)
    {
        return IsNot(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public Task IsNot(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ValuesNotEqual(expected, actual, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task ValuesEqual(object? expected, object? actual, string? details = null)
    {
        return ValuesEqual(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ValuesEqual(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        Difference[] differences =
        [
            .. await localOptions.Valuer.CompareAsync(expected, actual).ConfigureAwait(false),
        ];
        if (differences.Length > 0)
        {
            throw new AssertException(
                $"Value equality failed for type '{GetTypeName(expected, actual)}'.",
                details,
                localOptions.Gen.InitialSeed,
                string.Join<Difference>(Environment.NewLine, differences)
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task ValuesNotEqual(object? expected, object? actual, string? details = null)
    {
        return ValuesNotEqual(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ValuesNotEqual(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (await localOptions.Valuer.EqualsAsync(expected, actual).ConfigureAwait(false))
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
    public virtual Task AreUnique(object? expected, object? actual, string? details = null)
    {
        return AreUnique(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task AreUnique(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        localOptions.Asserter.ReferenceNotEqual(expected, actual, details);

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
