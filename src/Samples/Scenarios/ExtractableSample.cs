using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public class ExtractableSample : IExtractable
{
    public int NumberValue { get; set; }

    public IEnumerable<object?> ExtractContents()
    {
        yield return $"{NumberValue}";
    }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
