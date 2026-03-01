using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Comparisons;

namespace CreateAndFake.Tests.TesterTool.TestSamples;

public sealed class NullReferenceSample(IValueEquatable data)
{
    private readonly IValueEquatable _data = data;

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return _data.ToString();
    }
}
