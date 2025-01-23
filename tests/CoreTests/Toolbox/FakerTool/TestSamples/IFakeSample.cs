namespace CreateAndFakeTests.Toolbox.FakerTool.TestSamples;

public interface IFakeSample
{
    int Num { get; }

    string Hint { get; }

    int Count { get; set; }

    string Text { get; set; }

    int Calc();

    int Calc(int data);

    string Read();

    string Read(string data);

    void Combo(int num, string text);
}
