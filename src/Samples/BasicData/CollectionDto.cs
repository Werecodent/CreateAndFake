using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.BasicData;

/// <summary>Contains many concrete collections to test.</summary>
[ValidSample]
public class CollectionDto
{
    public CollectionDto<int>? IntCollectionValue { get; set; }

    public CollectionDto<double>? DoubleCollectionValue { get; set; }

    public CollectionDto<string>? StringCollectionValue { get; set; }

    public CollectionDto<object>? ObjectCollectionValue { get; set; }

    public CollectionDto<DateTime>? DateTimeCollectionValue { get; set; }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
