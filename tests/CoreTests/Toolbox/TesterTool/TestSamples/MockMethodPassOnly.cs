using System.Diagnostics.CodeAnalysis;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.TesterTool.TestSamples;

public sealed class MockMethodPassOnly
{
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "For testing.")]
    public bool MockPassOnly(IOnlyMockSample sample)
    {
        sample?.FailIfNotMocked();
        return true;
    }
}
