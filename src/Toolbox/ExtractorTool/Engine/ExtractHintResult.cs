using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <inheritdoc/>
public sealed class ExtractHintResult : HintResult<bool>
{
    /// <summary>For when a hint doesn't support a type or fails to extract from it.</summary>
    public static ExtractHintResult None { get; } = new(false, false);

    /// <inheritdoc/>
    private ExtractHintResult(bool hasData, bool data)
        : base(hasData, data) { }

    /// <inheritdoc cref="ExtractHintResult(bool,bool)"/>
    public ExtractHintResult(bool data)
        : this(true, data) { }
}
