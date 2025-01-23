using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Content;

namespace CreateAndFakeTests.Toolbox.TesterTool.TestSamples;

public sealed class NullReferenceSample
{
    private readonly IValueEquatable _data;

    internal NullReferenceSample(IValueEquatable data)
    {
        _data = data;
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return _data.ToString();
    }
}
