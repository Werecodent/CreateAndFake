using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public partial class Asserter(AsserterOptions options) : IAsserter
{
    /// <inheritdoc/>
    public AsserterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Default option configuration to use.</summary>
    protected AsserterMod? Unconfigured { get; } = null;

    /// <summary>Merges <see cref="Options"/> with <paramref name="optionConfiguration"/>.</summary>
    /// <param name="optionConfiguration">Provided modifications of <see cref="Options"/> to merge.</param>
    /// <returns>The merged options to use.</returns>
    protected AsserterOptions ApplyConfiguration(AsserterMod? optionConfiguration)
    {
        return optionConfiguration?.Invoke(Options) ?? Options;
    }

    /// <inheritdoc/>
    public IAsserter WithOptions(AsserterMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Asserter(optionConfiguration.Invoke(Options));
    }

    /// <inheritdoc/>
    public virtual void Pass() { }

    /// <inheritdoc/>
    public virtual void Pass(AsserterMod? optionConfiguration)
    {
        _ = ApplyConfiguration(optionConfiguration);
    }

    /// <inheritdoc/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public virtual void Fail(string? details = null)
    {
        Fail(Unconfigured, details);
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    public virtual void Fail(AsserterMod? optionConfiguration, string? details = null)
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        throw new AssertException("Test failed.", details, localOptions.Gen.InitialSeed);
    }

    /// <inheritdoc/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public virtual void Fail(object? content, string? details = null)
    {
        Fail(content, Unconfigured, details);
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    public virtual void Fail(
        object? content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        throw new AssertException(
            "Test failed.",
            details,
            localOptions.Gen.InitialSeed,
            content?.ToString()
        );
    }

    /// <inheritdoc/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public virtual void Fail(Exception? exception, string? details = null)
    {
        Fail(exception, Unconfigured, details);
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    public virtual void Fail(
        Exception? exception,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        throw new AssertException("Test failed.", details, localOptions.Gen.InitialSeed, exception);
    }

    /// <inheritdoc/>
    public virtual void CheckAll(params IEnumerable<Action> cases)
    {
        if (cases == null)
        {
            return;
        }

        List<Exception?> errors = [];
        foreach (Action test in cases)
        {
            try
            {
                test.Invoke();
                errors.Add(null);
            }
            catch (Exception e)
            {
                errors.Add(e);
            }
        }

        if (errors.Exists(e => e != null))
        {
            throw new AggregateException(
                "Cases failed: "
                    + string.Join(
                        ", ",
                        Enumerable.Range(0, errors.Count).Where(i => errors[i] != null)
                    )
                    + " -",
                errors.Where(e => e != null).Select(e => e!)
            );
        }
    }

    /// <summary>Finds a suitable <see cref="Type"/> name to use for assertion messages.</summary>
    /// <param name="expected">Instance being compared to <paramref name="actual"/>.</param>
    /// <param name="actual">Instance to run assertion checks with.</param>
    /// <returns>The <see cref="Type"/> name to use if found, <see langword="null"/> otherwise.</returns>
    private static string? GetTypeName(object? expected, object? actual)
    {
        return GenericConverter.ExpandName(expected ?? actual);
    }
}
