namespace CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public interface IOnlyMockSample
{
    bool FailIfNotMocked();
}
