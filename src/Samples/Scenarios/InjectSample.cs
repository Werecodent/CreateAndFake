using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public class InjectSample(DataSample data, DataSample data2)
{
    public DataSample Data { get; } = data;

    public DataSample Data2 { get; } = data2;

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
