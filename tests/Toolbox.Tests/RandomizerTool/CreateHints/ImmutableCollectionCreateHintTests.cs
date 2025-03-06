using System.Collections;
using System.Collections.Immutable;
using CreateAndFake.Design.Randomization;
using CreateAndFake.RandomizerTool.CreateHints;

namespace CreateAndFake.Tests.RandomizerTool.CreateHints;

public sealed class ImmutableCollectionCreateHintTests
    : CreateHintTestBase<ImmutableCollectionCreateHint>
{
    private static readonly ImmutableCollectionCreateHint _TestInstance = new();

    private static readonly Type[] _ItemTypes =
    [
        typeof(string),
        typeof(object),
        typeof(int),
        typeof(double),
        typeof(KeyValuePair<string, int>),
    ];

    private static readonly Type[] _ValidTypes =
    [
        .. ImmutableCollectionCreateHint
            .PotentialCollections.Concat(
                [
                    typeof(IImmutableList<>),
                    typeof(IImmutableQueue<>),
                    typeof(IImmutableStack<>),
                    typeof(IImmutableDictionary<,>),
                ]
            )
            .Select(MakeDefined),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(object),
        typeof(IEnumerable),
        typeof(IEnumerable<>),
    ];

    public ImmutableCollectionCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    private static Type MakeDefined(Type type)
    {
        if (type.IsGenericTypeDefinition)
        {
            FastRandom random = new();
            return type.MakeGenericType(
                [.. type.GetGenericArguments().Select(t => random.NextItem(_ItemTypes))]
            );
        }
        else
        {
            return type;
        }
    }
}
