using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public partial class Asserter(AsserterOptions options) : IAsserter
{
    /// <inheritdoc/>
    public AsserterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => [];

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
    public virtual void Pass() { }

    /// <inheritdoc/>
    public virtual void Pass(AsserterMod? optionConfiguration)
    {
        _ = ApplyConfiguration(optionConfiguration);
    }

    /// <inheritdoc/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public virtual void Fail(string? details = null, string? content = null)
    {
        Fail(Unconfigured, details, content);
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    public virtual void Fail(
        AsserterMod? optionConfiguration,
        string? details = null,
        string? content = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        throw new AssertException("Test failed.", details, localOptions.Gen.InitialSeed, content);
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

    /// <summary>Finds a suitable <see cref="Type"/> name to use for assertion messages.</summary>
    /// <param name="expected">Instance being compared to <paramref name="actual"/>.</param>
    /// <param name="actual">Instance to run assertion checks with.</param>
    /// <returns>The <see cref="Type"/> name to use if found, <see langword="null"/> otherwise.</returns>
    private static string? GetTypeName(object? expected, object? actual)
    {
        return ExpandTypeName((expected ?? actual)?.GetType());
    }

    /// <summary>Builds <see cref="Type"/> name with generic argument names.</summary>
    /// <param name="type"><see cref="Type"/> to describe.</param>
    /// <returns>The built name.</returns>
    private static string? ExpandTypeName(Type? type)
    {
        if (type?.IsGenericType == true)
        {
            return string.Concat(
                type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.InvariantCulture)),
                "<",
                string.Join(",", type.GetGenericArguments().Select(ExpandTypeName)),
                ">"
            );
        }
        else
        {
            return type?.Name;
        }
    }

    /// <inheritdoc/>
    public IAsserter WithOptions(AsserterMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration, nameof(optionConfiguration));
        return new Asserter(optionConfiguration.Invoke(Options));
    }
}
