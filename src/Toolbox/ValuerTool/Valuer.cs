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
        return new ValuerChainer(Options, _engine).CompareAsync(
            expected,
            actual,
            canceler,
            optionConfiguration
        );
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
    public Task<int> GetHashCodeAsync(
        object? item,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        return new ValuerChainer(Options, _engine).GetHashCodeAsync(
            item,
            canceler,
            optionConfiguration
        );
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
    public Task<bool> EqualsAsync(
        object? x,
        object? y,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        return new ValuerChainer(Options, _engine).EqualsAsync(x, y, canceler, optionConfiguration);
    }

    /// <inheritdoc/>
    public IValuer WithOptions(ValuerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Valuer(optionConfiguration.Invoke(Options));
    }
}
