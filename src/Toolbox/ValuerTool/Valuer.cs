using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool;

/// <inheritdoc cref="IValuer"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Valuer(ValuerOptions options) : IValuer
{
    /// <summary>Handles hint based comparisons.</summary>
    private static readonly ValuerEngine _engine = new();

    /// <inheritdoc/>
    public ValuerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _engine.SupportedTypes;

    /// <inheritdoc/>
    public IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration = null
    )
    {
        return new ValuerChainer(Options, _engine).Compare(expected, actual, optionConfiguration);
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        return HandleCompareAsync(expected, actual, optionConfiguration, canceler);
    }

    /// <inheritdoc cref="CompareAsync"/>
    private async IAsyncEnumerable<Difference> HandleCompareAsync(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        ValuerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        using CancellationTokenSource timeoutSource =
            CancellationTokenSource.CreateLinkedTokenSource(canceler);

        timeoutSource.CancelAfter(localOptions.AsyncTimeout);

        await foreach (
            Difference diff in new ValuerChainer(Options, _engine)
                .CompareAsync(
                    expected,
                    actual,
                    timeoutSource.Token,
                    (optionConfiguration != null) ? _ => localOptions : null
                )
                .ConfigureAwait(false)
        )
        {
            yield return diff;
        }
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item)
    {
        return new ValuerChainer(Options, _engine).GetHashCode(item);
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item, ValuerMod? optionConfiguration)
    {
        return new ValuerChainer(Options, _engine).GetHashCode(item, optionConfiguration);
    }

    /// <inheritdoc/>
    public async Task<int> GetHashCodeAsync(
        object? item,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        ValuerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        using CancellationTokenSource timeoutSource =
            CancellationTokenSource.CreateLinkedTokenSource(canceler);

        timeoutSource.CancelAfter(localOptions.AsyncTimeout);

        return await new ValuerChainer(Options, _engine)
            .GetHashCodeAsync(
                item,
                timeoutSource.Token,
                (optionConfiguration != null) ? _ => localOptions : null
            )
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public new bool Equals(object? x, object? y)
    {
        return new ValuerChainer(Options, _engine).Equals(x, y);
    }

    /// <inheritdoc/>
    public bool Equals(object? x, object? y, ValuerMod? optionConfiguration)
    {
        return new ValuerChainer(Options, _engine).Equals(x, y, optionConfiguration);
    }

    /// <inheritdoc/>
    public async Task<bool> EqualsAsync(
        object? x,
        object? y,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        ValuerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        using CancellationTokenSource timeoutSource =
            CancellationTokenSource.CreateLinkedTokenSource(canceler);

        timeoutSource.CancelAfter(localOptions.AsyncTimeout);

        return await new ValuerChainer(Options, _engine)
            .EqualsAsync(
                x,
                y,
                timeoutSource.Token,
                (optionConfiguration != null) ? _ => localOptions : null
            )
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public IEqualityComparer<T> ToComparer<T>()
    {
        return new ByValuerComparer<T>(this);
    }

    /// <inheritdoc/>
    public IValuer WithOptions(ValuerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Valuer(optionConfiguration.Invoke(Options));
    }
}
