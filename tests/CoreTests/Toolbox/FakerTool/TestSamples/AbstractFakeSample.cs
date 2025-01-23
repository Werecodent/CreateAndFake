namespace CreateAndFakeTests.Toolbox.FakerTool.TestSamples;

public abstract class AbstractFakeSample : IFakeSample
{
    public abstract int Num { get; }

    public abstract string Hint { get; }

    public abstract int Count { get; set; }

    public abstract string Text { get; set; }

    public abstract int Calc();

    public abstract int Calc(int data);

    public abstract void Combo(int num, string text);

    public abstract string Read();

    public abstract string Read(string data);
}
