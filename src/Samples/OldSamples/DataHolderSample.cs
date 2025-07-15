using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Samples.OldSamples;

public class DataHolderSample : DataSample
{
    public DataSample? NestedValue { get; set; }

    [ExcludeFromCodeCoverage]
    public virtual bool HasNested(DataSample value)
    {
        return false;
    }
}
