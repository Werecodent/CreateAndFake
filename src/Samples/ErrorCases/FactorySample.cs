using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public sealed class FactorySample
{
    [ExcludeFromCodeCoverage]
    public string Data { get; }

    private FactorySample(string data)
    {
        Data = data;
    }

    public static FactorySample Create(int data)
    {
        return new FactorySample("Value:" + data);
    }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
