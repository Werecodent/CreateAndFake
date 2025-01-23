namespace CreateAndFakeTests.TestSamples;

public abstract class RefSample
{
    public abstract void ReturnVoid(ref string input);

    public abstract int ReturnValue(ref int input);
}
