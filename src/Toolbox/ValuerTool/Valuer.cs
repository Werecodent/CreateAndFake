using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
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
        string? typeName = (expected ?? actual)?.GetType().Name;
        try
        {
            return
            [
                .. new ValuerChainer(Options, _engine).Compare(
                    expected,
                    actual,
                    optionConfiguration
                ),
            ];
        }
        catch (Exception e)
        {
            throw new ToolException($"Issue comparing type '{typeName}'.", e);
        }
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration = null
    )
    {
        string? typeName = (expected ?? actual)?.GetType().Name;
        try
        {
            return new ValuerChainer(Options, _engine).CompareAsync(
                expected,
                actual,
                optionConfiguration
            );
        }
        catch (Exception e)
        {
            throw new ToolException($"Issue comparing type '{typeName}'.", e);
        }
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item)
    {
        return GetHashCode(item, null);
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item, ValuerMod? optionConfiguration)
    {
        string? typeName = item?.GetType().Name;
        try
        {
            return new ValuerChainer(Options, _engine).GetHashCode(item, optionConfiguration);
        }
        catch (Exception e)
        {
            throw new ToolException($"Issue hashing type '{typeName}'.", e);
        }
    }

    /// <inheritdoc/>
    public async Task<int> GetHashCodeAsync(
        object? item,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        string? typeName = item?.GetType().Name;
        try
        {
            return await new ValuerChainer(Options, _engine)
                .GetHashCodeAsync(item, canceler, optionConfiguration)
                .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new ToolException($"Issue hashing type '{typeName}'.", e);
        }
    }

    /// <inheritdoc/>
    public new bool Equals(object? x, object? y)
    {
        return !Compare(x, y).Any();
    }

    /// <inheritdoc/>
    public bool Equals(object? x, object? y, ValuerMod? optionConfiguration)
    {
        return !Compare(x, y, optionConfiguration).Any();
    }

    /// <inheritdoc/>
    public async Task<bool> EqualsAsync(
        object? x,
        object? y,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        return !await AsyncEnumHelper
            .HasAnyAsync(CompareAsync(x, y, optionConfiguration), canceler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public IValuer WithOptions(ValuerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Valuer(optionConfiguration.Invoke(Options));
    }
}
