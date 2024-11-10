using System.Collections;

namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertGroup : AssertGroupBase<AssertGroup>
{
    /// <inheritdoc/>
    internal AssertGroup(AsserterOptions options, IEnumerable? collection) : base(options, collection) { }
}
