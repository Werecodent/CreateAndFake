using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Tests.TestSamples;

public class DataHolderSample : DataSample
{
    public DataSample NestedValue { get; set; }

    [ExcludeFromCodeCoverage]
    public virtual bool HasNested(DataSample value)
    {
        return false;
    }
}
