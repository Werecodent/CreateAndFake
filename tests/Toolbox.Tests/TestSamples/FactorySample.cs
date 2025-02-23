using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Tests.TestSamples;

public sealed class FactorySample
{
    [ExcludeFromCodeCoverage]
    public string Data { get; }

    private FactorySample(string data)
    {
        Data = data;
    }

    internal static FactorySample Create(int data)
    {
        return new FactorySample("Value:" + data);
    }
}
