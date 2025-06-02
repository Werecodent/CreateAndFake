using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Tests.FakerTool.TestSamples;

#pragma warning disable CA1065, MA0025 // For testing.

[ExcludeFromCodeCoverage]
public class VirtualFakeSample : AbstractFakeSample
{
    public override int Num => throw new NotImplementedException();

    public override string Hint => throw new NotImplementedException();

    public override int Count
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public override string Text
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public override int Calc()
    {
        throw new NotImplementedException();
    }

    public override int Calc(int data)
    {
        throw new NotImplementedException();
    }

    public override void Combo(int num, string text)
    {
        throw new NotImplementedException();
    }

    public override string Read()
    {
        throw new NotImplementedException();
    }

    public override string Read(string data)
    {
        throw new NotImplementedException();
    }
}

#pragma warning restore CA1065, MA0025
