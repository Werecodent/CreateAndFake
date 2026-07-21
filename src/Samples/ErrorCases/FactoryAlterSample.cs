using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public sealed class FactoryAlterSample
{
    public string Data { get; }

    private FactoryAlterSample(string data)
    {
        Data = data;
    }

    public static FactoryAlterSample Create(int data)
    {
        return new FactoryAlterSample("Value:" + data);
    }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
