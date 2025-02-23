using System.Collections;

namespace CreateAndFake.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertEnumerable : AssertEnumerableBase<AssertEnumerable>
{
    /// <inheritdoc/>
    internal AssertEnumerable(IAsserter asserter, IEnumerable? collection) : base(asserter, collection) { }
}
