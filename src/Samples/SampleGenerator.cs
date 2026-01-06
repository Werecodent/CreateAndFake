using System.Collections.Immutable;
using System.Dynamic;
using System.Reflection;
using CreateAndFake.Samples.DoubleValue;
using CreateAndFake.Samples.SingleValue;

namespace CreateAndFake.Samples;

public static class SampleGenerator
{
    private static readonly Type[] _BaseTypes =
    [
        typeof(int),
        typeof(double),
        typeof(string),
        typeof(object),
        typeof(DateTime),
    ];

    private static readonly Type[] _SingleCollections =
    [
        typeof(IList<>),
        typeof(List<>),
        typeof(ISet<>),
        typeof(HashSet<>),
        typeof(ICollection<>),
        typeof(IEnumerable<>),
    ];

    private static readonly Type[] _DoubleCollections =
    [
        typeof(IDictionary<,>),
        typeof(Dictionary<,>),
        typeof(KeyValuePair<,>),
    ];

    private static readonly Type[] _SingleHolders =
    [
        typeof(BaseHolder<>),
        typeof(BaseReadableHolder<>),
        typeof(BaseWriteableHolder<>),
        typeof(Holder<>),
        typeof(IHolder<>),
        typeof(IReadableHolder<>),
        typeof(IWriteableHolder<>),
        typeof(ReadableHolder<>),
        typeof(WriteableHolder<>),
    ];

    private static ImmutableArray<Type> MakeMasterList()
    {
        return
        [
            .. Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => Attribute.IsDefined(t, typeof(ValidSampleAttribute))),
            .. _BaseTypes,
            .. InsertIntoSingleGenerics(_BaseTypes, _SingleCollections),
            .. InsertIntoSingleGenerics(_BaseTypes, _SingleHolders),
            .. InsertIntoDoubleGenerics(_BaseTypes, _DoubleCollections),
        ];
    }

    private static IEnumerable<Type> InsertIntoSingleGenerics(
        ICollection<Type> types,
        IEnumerable<Type> generics
    )
    {
        return generics.SelectMany(generic => types.Select(type => generic.MakeGenericType(type)));
    }

    private static IEnumerable<Type> InsertIntoDoubleGenerics(
        ICollection<Type> types,
        IEnumerable<Type> generics
    )
    {
        return generics.SelectMany(generic =>
            types.SelectMany(typeA => types.Select(typeB => generic.MakeGenericType(typeA, typeB)))
        );
    }

    public static ImmutableArray<Type> AllValidDataSamples { get; } = MakeMasterList();
}
