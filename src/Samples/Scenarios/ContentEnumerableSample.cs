using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class ContentEnumerableSample(string? data) : IEnumerable<char>
{
    public string Data { get; } = data ?? "";

    public ContentEnumerableSample(ContentEnumerableSample original)
        : this(original?.Data) { }

    public IEnumerator<char> GetEnumerator()
    {
        return Data.GetEnumerator();
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
