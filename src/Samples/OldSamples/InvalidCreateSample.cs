using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Samples.OldSamples;

[ExcludeFromCodeCoverage]
public sealed class InvalidCreateSample : IOnlyMockSample
{
    public InvalidCreateSample()
    {
        throw new InvalidOperationException("Tried to create invalid sample.");
    }

    public bool FailIfNotMocked()
    {
        throw new InvalidOperationException("Mock was not created.");
    }
}
