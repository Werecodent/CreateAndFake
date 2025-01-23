namespace CreateAndFakeTests.Toolbox.TesterTool.TestSamples;

public sealed class MethodThrowsSample
{
    public void ThrowSomething()
    {
        throw new InvalidOperationException(GetType().Name);
    }
}
