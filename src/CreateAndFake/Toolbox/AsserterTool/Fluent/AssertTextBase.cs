#pragma warning disable CA1307 // Specify StringComparison for clarity: Not available for all versions.
#pragma warning disable CA1310 // Specify StringComparison for correctness: Not available for all versions.

namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <summary>Handles common <see cref="string"/> assertion calls.</summary>
/// <param name="text"><inheritdoc cref="Text" path="/summary"/></param>
/// <inheritdoc cref="AssertGroupBase{T}"/>
public abstract class AssertTextBase<T>(AsserterOptions options, string? text)
    : AssertGroupBase<T>(options, text) where T : AssertTextBase<T>
{
    /// <summary>Text to run assertion checks with.</summary>
    protected string? Text { get; } = text;

    /// <summary>Verifies <c>text</c> contains <paramref name="content"/>.</summary>
    /// <param name="content">Potential internal text to check for.</param>
    /// <param name="details">Optional failure details to include.</param>
    /// <returns>Chainer to make additional assertions with.</returns>
    /// <exception cref="AssertException">If <c>text</c> fails to match the expected behavior.</exception>
    public virtual AssertChainer<T> Contains(string content, string? details = null)
    {
        if (Text == null || !Text.Contains(content))
        {
            throw new AssertException($"Text was missing '{content}'.", details, Options.Gen.InitialSeed, Text);
        }
        return ToChainer();
    }

    /// <summary>Verifies <c>text</c> does not contain <paramref name="content"/>.</summary>
    /// <inheritdoc cref="Contains"/>
    public virtual AssertChainer<T> ContainsNot(string content, string? details = null)
    {
        if (Text != null && Text.Contains(content))
        {
            throw new AssertException($"Text contained '{content}'.", details, Options.Gen.InitialSeed, Text);
        }
        return ToChainer();
    }

    /// <summary>Verifies <c>text</c> starts with <paramref name="content"/>.</summary>
    /// <inheritdoc cref="Contains"/>
    public virtual AssertChainer<T> StartsWith(string content, string? details = null)
    {
        if (Text == null || !Text.StartsWith(content))
        {
            throw new AssertException($"Text did not start with '{content}'.", details, Options.Gen.InitialSeed, Text);
        }
        return ToChainer();
    }

    /// <summary>Verifies <c>text</c> does not start with <paramref name="content"/>.</summary>
    /// <inheritdoc cref="Contains"/>
    public virtual AssertChainer<T> StartsNotWith(string content, string? details = null)
    {
        if (Text != null && Text.StartsWith(content))
        {
            throw new AssertException($"Text started with '{content}'.", details, Options.Gen.InitialSeed, Text);
        }
        return ToChainer();
    }

    /// <summary>Verifies <c>text</c> ends with <paramref name="content"/>.</summary>
    /// <inheritdoc cref="Contains"/>
    public virtual AssertChainer<T> EndsWith(string content, string? details = null)
    {
        if (Text == null || !Text.EndsWith(content))
        {
            throw new AssertException($"Text did not end with '{content}'.", details, Options.Gen.InitialSeed, Text);
        }
        return ToChainer();
    }

    /// <summary>Verifies <c>text</c> does not end with <paramref name="content"/>.</summary>
    /// <inheritdoc cref="Contains"/>
    public virtual AssertChainer<T> EndsNotWith(string content, string? details = null)
    {
        if (Text != null && Text.EndsWith(content))
        {
            throw new AssertException($"Text ended with '{content}'.", details, Options.Gen.InitialSeed, Text);
        }
        return ToChainer();
    }
}

#pragma warning restore CA1307 // Specify StringComparison for clarity
#pragma warning restore CA1310 // Specify StringComparison for correctness
