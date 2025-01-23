namespace CreateAndFakeTests.TestSamples;

public abstract class OutSample
{
    public abstract void ReturnVoid(out string input);

    public abstract int ReturnValue(out int input);
}
