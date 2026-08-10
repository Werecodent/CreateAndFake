using Werecodent.CreateAndFake.Design.Comparisons;

namespace Werecodent.CreateAndFake.Tests.TesterTool.TestSamples;

public sealed class NullReferenceSample(IValueEquatable data)
{
    private readonly IValueEquatable _data = data;

    public override string ToString()
    {
        return _data.ToString();
    }
}
