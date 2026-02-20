using System.Collections;
using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertEnumerable : AssertEnumerableBase<AssertEnumerable>
{
    /// <inheritdoc/>
    internal AssertEnumerable(IAsserter asserter, IEnumerable? collection)
        : base(asserter, collection) { }
}
