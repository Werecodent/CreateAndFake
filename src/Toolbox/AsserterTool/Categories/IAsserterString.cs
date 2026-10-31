namespace Werecodent.CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common string test scenarios.</summary>
public interface IAsserterString
{
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void Contains(string content, string? text, string? details = null);

    /// <summary>Verifies the <c>text</c> contains the given <paramref name="content"/>.</summary>
    /// <param name="content">Expected text to be present.</param>
    /// <param name="text">Instance to test.</param>
    /// <remarks>Important relevant options: <list type="bullet">
    ///     <item><see cref="AsserterOptions.ComparisonModeForStringAsserts"/></item>
    /// </list></remarks>
    /// <inheritdoc cref="IAsserterObject.Is(object,object,AsserterMod,string)"/>
    void Contains(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ContainsNot(string,string,AsserterMod,string)"/>
    void ContainsNot(string content, string? text, string? details = null);

    /// <summary>Verifies the <c>text</c> does not contain the given <paramref name="content"/>.</summary>
    /// <param name="content">Expected text to be missing.</param>
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void ContainsNot(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="StartsWith(string,string,AsserterMod,string)"/>
    void StartsWith(char content, string? text, string? details = null);

    /// <inheritdoc cref="StartsWith(string,string,AsserterMod,string)"/>
    void StartsWith(
        char content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="StartsWith(string,string,AsserterMod,string)"/>
    void StartsWith(string content, string? text, string? details = null);

    /// <summary>Verifies the <c>text</c> starts with the given <paramref name="content"/>.</summary>
    /// <param name="content">Expected text to be at the start.</param>
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void StartsWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="StartsNotWith(string,string,AsserterMod,string)"/>
    void StartsNotWith(char content, string? text, string? details = null);

    /// <inheritdoc cref="StartsNotWith(string,string,AsserterMod,string)"/>
    void StartsNotWith(
        char content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="StartsNotWith(string,string,AsserterMod,string)"/>
    void StartsNotWith(string content, string? text, string? details = null);

    /// <summary>Verifies the <c>text</c> does not start with the given <paramref name="content"/>.</summary>
    /// <param name="content">Expected text to not be at the start.</param>
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void StartsNotWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="EndsWith(string,string,AsserterMod,string)"/>
    void EndsWith(char content, string? text, string? details = null);

    /// <inheritdoc cref="EndsWith(string,string,AsserterMod,string)"/>
    void EndsWith(
        char content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="EndsWith(string,string,AsserterMod,string)"/>
    void EndsWith(string content, string? text, string? details = null);

    /// <summary>Verifies the <c>text</c> ends with the given <paramref name="content"/>.</summary>
    /// <param name="content">Expected text to be at the end.</param>
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void EndsWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="EndsNotWith(string,string,AsserterMod,string)"/>
    void EndsNotWith(char content, string? text, string? details = null);

    /// <inheritdoc cref="EndsNotWith(string,string,AsserterMod,string)"/>
    void EndsNotWith(
        char content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="EndsNotWith(string,string,AsserterMod,string)"/>
    void EndsNotWith(string content, string? text, string? details = null);

    /// <summary>Verifies the <c>text</c> does not end with <paramref name="content"/>.</summary>
    /// <param name="content">Expected text to not be at the end.</param>
    /// <inheritdoc cref="Contains(string,string,AsserterMod,string)"/>
    void EndsNotWith(
        string content,
        string? text,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
