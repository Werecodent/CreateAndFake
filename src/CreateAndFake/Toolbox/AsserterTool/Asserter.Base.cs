namespace CreateAndFake.Toolbox.AsserterTool;

#pragma warning disable CA1307 // Specify StringComparison for clarity: Not available for all versions.

/// <inheritdoc cref="IAsserter"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public partial class Asserter(AsserterOptions options) : IAsserter
{
    /// <inheritdoc/>
    public AsserterOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Default option configuration to use.</summary>
    protected AsserterMod Unconfigured { get; } = null!;

    /// <summary>Merges <see cref="Options"/> with <paramref name="optionConfiguration"/>.</summary>
    /// <param name="optionConfiguration">Provided modifications of <see cref="AsserterOptions"/> to merge.</param>
    /// <returns>The merged options to use.</returns>
    protected AsserterOptions ApplyConfiguration(AsserterMod optionConfiguration)
    {
        return optionConfiguration?.Invoke(Options) ?? Options;
    }

    /// <inheritdoc/>
    public virtual void Pass() { }

    /// <inheritdoc/>
    public virtual void Pass(AsserterMod optionConfiguration) { }

    /// <inheritdoc/>
    public virtual void Fail(string? details = null, string? content = null)
    {
        Fail(Unconfigured, details, content);
    }

    /// <inheritdoc/>
    public virtual void Fail(AsserterMod optionConfiguration, string? details = null, string? content = null)
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        throw new AssertException("Test failed.", details, localOptions.Gen.InitialSeed, content);
    }

    /// <inheritdoc/>
    public virtual void Fail(Exception? exception, string? details = null)
    {
        Fail(exception, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void Fail(Exception? exception, AsserterMod optionConfiguration, string? details = null)
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        throw new AssertException("Test failed.", details, localOptions.Gen.InitialSeed, exception);
    }

    /// <summary>Finds a suitable <c>Type</c> name to use for assertion messages.</summary>
    /// <param name="expected">Instance being compared to <c>actual</c>.</param>
    /// <param name="actual">Instance to run assertion checks with.</param>
    /// <returns>The <c>Type</c> name to use if found; <c>null</c> otherwise.</returns>
    private static string? GetTypeName(object? expected, object? actual)
    {
        return ExpandTypeName((expected ?? actual)?.GetType());
    }

    /// <summary>Builds <c>Type</c> name with generic argument names.</summary>
    /// <param name="type"><c>Type</c> to describe.</param>
    /// <returns>The built name.</returns>
    private static string? ExpandTypeName(Type? type)
    {
        if (type != null && type.IsGenericType)
        {
            return string.Concat(
                type.Name.Substring(0, type.Name.IndexOf('`')),
                "<",
                string.Join(",", type.GetGenericArguments().Select(ExpandTypeName)),
                ">");
        }
        else
        {
            return type?.Name;
        }
    }
}

#pragma warning restore CA1307 // Specify StringComparison for clarity
