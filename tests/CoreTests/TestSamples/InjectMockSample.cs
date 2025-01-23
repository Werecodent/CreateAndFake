namespace CreateAndFakeTests.TestSamples;

public sealed class InjectMockSample(IOnlyMockSample sample1, IOnlyMockSample sample2)
{
    private readonly IOnlyMockSample
        _sample1 = sample1 ?? throw new ArgumentNullException(nameof(sample1)),
        _sample2 = sample2 ?? throw new ArgumentNullException(nameof(sample2));

    public void TestIfMockedSeparately()
    {
        _sample1.FailIfNotMocked();
        _sample2.FailIfNotMocked();
        Tools.Asserter.IsNot(_sample1, _sample2);
    }
}
