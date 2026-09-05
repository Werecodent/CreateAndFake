using System.Diagnostics.CodeAnalysis;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public class DataHolderSample : DataSample
{
    public DataSample? NestedValue { get; set; }

    [ExcludeFromCodeCoverage]
    public virtual bool HasNested(DataSample value)
    {
        return false;
    }
}
