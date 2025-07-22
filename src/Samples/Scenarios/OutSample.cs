namespace CreateAndFake.Samples.Scenarios;

public abstract class OutSample
{
    public abstract void ReturnVoid(out string input);

    public abstract int ReturnValue(out int input);
}
