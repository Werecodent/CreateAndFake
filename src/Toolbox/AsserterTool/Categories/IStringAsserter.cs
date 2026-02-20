namespace CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common string test scenarios.</summary>
public interface IStringAsserter
{
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void Contains(string content, string? text, string? details = null);

    /// <summary>Verifies <c>text</c> contains <paramref name="content"/>.</summary>
    /// <param name="content">Expected internal text.</param>
    /// <param name="text">Text to run assertion checks with.</param>
    /// <inheritdoc cref="IObjectAsserter.Is(object,object,AsserterMod,string)"/>
    void Contains(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ContainsNot(string,string,AsserterMod,string)"/>
    void ContainsNot(string content, string? text, string? details = null);

    /// <summary>Verifies <c>text</c> does not contain <paramref name="content"/>.</summary>
    /// <param name="content">Expected missing internal text.</param>
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void ContainsNot(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="StartsWith(string,string,AsserterMod,string)"/>
    void StartsWith(string content, string? text, string? details = null);

    /// <summary>Verifies <c>text</c> starts with <paramref name="content"/>.</summary>
    /// <param name="content">Expected beginning text.</param>
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void StartsWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="StartsNotWith(string,string,AsserterMod,string)"/>
    void StartsNotWith(string content, string? text, string? details = null);

    /// <summary>Verifies <c>text</c> does not start with <paramref name="content"/>.</summary>
    /// <param name="content">Expected missing beginning text.</param>
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void StartsNotWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="EndsWith(string,string,AsserterMod,string)"/>
    void EndsWith(string content, string? text, string? details = null);

    /// <summary>Verifies <c>text</c> ends with <paramref name="content"/>.</summary>
    /// <param name="content">Expected ending text.</param>
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void EndsWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="EndsNotWith(string,string,AsserterMod,string)"/>
    void EndsNotWith(string content, string? text, string? details = null);

    /// <summary>Verifies <c>text</c> does not end with <paramref name="content"/>.</summary>
    /// <param name="content">Expected missing ending text.</param>
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void EndsNotWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
