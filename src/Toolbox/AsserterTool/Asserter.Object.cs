using System.Text;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IObjectAsserter
{
    /// <inheritdoc/>
    public void Is(object? expected, object? actual, string? details = null)
    {
        Is(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public void Is(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ValuesEqual(expected, actual, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public void IsNot(object? expected, object? actual, string? details = null)
    {
        IsNot(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public void IsNot(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ValuesNotEqual(expected, actual, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual void ReferenceEqual(object? expected, object? actual, string? details = null)
    {
        ReferenceEqual(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ReferenceEqual(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (!ReferenceEquals(expected, actual))
        {
            throw new AssertException(
                "References failed to equal.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
    }

    /// <inheritdoc/>
    public virtual void ReferenceNotEqual(object? expected, object? actual, string? details = null)
    {
        ReferenceNotEqual(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ReferenceNotEqual(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (ReferenceEquals(expected, actual))
        {
            throw new AssertException(
                "References failed to not equal.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
    }

    /// <inheritdoc/>
    public virtual void ValuesEqual(object? expected, object? actual, string? details = null)
    {
        ValuesEqual(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ValuesEqual(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        Difference[] differences = [.. localOptions.Valuer.Compare(expected, actual)];
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
    public virtual void ValuesNotEqual(object? expected, object? actual, string? details = null)
    {
        ValuesNotEqual(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ValuesNotEqual(
        object? expected,
        object? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (localOptions.Valuer.Equals(expected, actual))
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
    public virtual void AreUnique(object? expected, object? actual, string? details = null)
    {
        AreUnique(expected, actual, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void AreUnique(
        object? expected,
        object? actual,
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
    }

    /// <inheritdoc/>
    public virtual void Called(object? fake, Times? total = null)
    {
        Called(fake, Unconfigured, total);
    }

    /// <inheritdoc/>
    public virtual void Called(object? fake, AsserterMod? optionConfiguration, Times? total = null)
    {
        IsNot(null, fake);
        new Fake((IFaked)fake!).VerifyAll(total);
    }
}
