using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IStringAsserter
{
    /// <inheritdoc/>
    public virtual void Contains(string content, string? text, string? details = null)
    {
        Contains(content, text, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void Contains(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (text == null || text.IndexOf(content, localOptions.StringCompareOption) == -1)
        {
            throw new AssertException(
                $"Text was missing '{content}'.",
                details,
                localOptions.Gen.InitialSeed,
                text
            );
        }
    }

    /// <inheritdoc/>
    public virtual void ContainsNot(string content, string? text, string? details = null)
    {
        ContainsNot(content, text, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ContainsNot(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (text != null && text.IndexOf(content, localOptions.StringCompareOption) >= 0)
        {
            throw new AssertException(
                $"Text contained '{content}'.",
                details,
                localOptions.Gen.InitialSeed,
                text
            );
        }
    }

    /// <inheritdoc/>
    public virtual void StartsWith(string content, string? text, string? details = null)
    {
        StartsWith(content, text, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void StartsWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (text == null || !text.StartsWith(content, localOptions.StringCompareOption))
        {
            throw new AssertException(
                $"Text did not start with '{content}'.",
                details,
                localOptions.Gen.InitialSeed,
                text
            );
        }
    }

    /// <inheritdoc/>
    public virtual void StartsNotWith(string content, string? text, string? details = null)
    {
        StartsNotWith(content, text, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void StartsNotWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (text != null && text.StartsWith(content, localOptions.StringCompareOption))
        {
            throw new AssertException(
                $"Text started with '{content}'.",
                details,
                localOptions.Gen.InitialSeed,
                text
            );
        }
    }

    /// <inheritdoc/>
    public virtual void EndsWith(string content, string? text, string? details = null)
    {
        EndsWith(content, text, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void EndsWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (text == null || !text.EndsWith(content, localOptions.StringCompareOption))
        {
            throw new AssertException(
                $"Text did not end with '{content}'.",
                details,
                localOptions.Gen.InitialSeed,
                text
            );
        }
    }

    /// <inheritdoc/>
    public virtual void EndsNotWith(string content, string? text, string? details = null)
    {
        EndsNotWith(content, text, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void EndsNotWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (text != null && text.EndsWith(content, localOptions.StringCompareOption))
        {
            throw new AssertException(
                $"Text ended with '{content}'.",
                details,
                localOptions.Gen.InitialSeed,
                text
            );
        }
    }
}
