using System.Collections;
using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertEnumerable : AssertEnumerableBase<AssertEnumerable>
{
    /// <inheritdoc/>
    internal AssertEnumerable(IAsserter asserter, IEnumerable? collection)
        : base(asserter, collection) { }
}
