using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class DoubleEnumerableSample(IEnumerable<int> intData, IEnumerable<string> stringData)
    : IEnumerable<int>,
        IEnumerable<string>
{
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<string> GetEnumerator()
    {
        return (stringData ?? []).GetEnumerator();
    }

    IEnumerator<int> IEnumerable<int>.GetEnumerator()
    {
        return (intData ?? []).GetEnumerator();
    }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
