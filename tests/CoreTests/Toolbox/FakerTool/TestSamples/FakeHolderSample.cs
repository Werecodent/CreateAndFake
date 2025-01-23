namespace CreateAndFakeTests.Toolbox.FakerTool.TestSamples;

public sealed class FakeHolderSample(IFakeSample sample1, AbstractFakeSample sample2, int value1, string value2)
{
    public IFakeSample Sample1 { get; } = sample1;

    public AbstractFakeSample Sample2 { get; } = sample2;

    public int Value1 { get; } = value1;

    public string Value2 { get; } = value2;
}
