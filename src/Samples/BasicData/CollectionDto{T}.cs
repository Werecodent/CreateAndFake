using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.BasicData;

/// <summary>Assorted collection types to test.</summary>
/// <typeparam name="T">Item type for the collections.</typeparam>
[ValidSample]
public class CollectionDto<T>
{
    public T[]? ArrayValues { get; set; }

    public IEnumerable<T>? EnumerableValues { get; set; }

    public ICollection<T>? CollectionValues { get; set; }

    public IList<T>? ListValues { get; set; }

    public ISet<T>? SetValues { get; set; }

    public IDictionary<int, T>? DictValues { get; set; }

    public IDictionary<T, int>? IntDictValues { get; set; }

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
