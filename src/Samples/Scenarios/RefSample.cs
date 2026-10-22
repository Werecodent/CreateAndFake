namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public abstract class RefSample
{
    public abstract void ReturnVoid(ref string input);

    public abstract int ReturnValue(ref int input);
}
