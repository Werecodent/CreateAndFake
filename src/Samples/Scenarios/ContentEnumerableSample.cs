using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class ContentEnumerableSample(string? data) : IEnumerable<int>
{
    public string Data { get; } = data ?? "";

    public ContentEnumerableSample(ContentEnumerableSample original)
        : this(original?.Data) { }

    public IEnumerator<int> GetEnumerator()
    {
        return Data.Select(x => (int)x).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
