using System.Diagnostics.CodeAnalysis;

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
}
