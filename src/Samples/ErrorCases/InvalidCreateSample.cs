using System.Diagnostics.CodeAnalysis;

namespace Werecodent.CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public sealed class InvalidCreateSample : IOnlyMockSample
{
    public InvalidCreateSample()
    {
        throw new InvalidOperationException("Tried to create invalid sample.");
    }

    [ExcludeFromCodeCoverage] // Designed to never be reached.
    public bool FailIfNotMocked()
    {
        throw new InvalidOperationException("Mock was not created.");
    }
}
