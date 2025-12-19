using System.Text;
using CreateAndFake.AsserterTool;
using CreateAndFake.AsyncAsserterTool.Categories;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.AsyncAsserterTool;

/// <inheritdoc cref="IAsyncAsserter"/>
public partial class AsyncAsserter : IAsyncObjectAsserter
{
    /// <inheritdoc/>
    public Task IsAsync(object? expected, object? actual, string? details = null)
    {
        return IsAsync(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public Task IsAsync(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ValuesEqualAsync(expected, actual, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public Task IsNotAsync(object? expected, object? actual, string? details = null)
    {
        return IsNotAsync(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public Task IsNotAsync(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ValuesNotEqualAsync(expected, actual, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task ValuesEqualAsync(object? expected, object? actual, string? details = null)
    {
        return ValuesEqualAsync(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ValuesEqualAsync(
        object? expected,
        object? actual,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        IList<Difference> differences = await AsyncEnumHelper
            .ToListAsync(localOptions.Valuer.CompareAsync(expected, actual))
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
        string? details = null
    )
    {
        return ValuesNotEqualAsync(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ValuesNotEqualAsync(
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
    public virtual Task AreUniqueAsync(object? expected, object? actual, string? details = null)
    {
        return AreUniqueAsync(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task AreUniqueAsync(
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
