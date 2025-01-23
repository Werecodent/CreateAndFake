using System.Collections;
using System.Collections.Frozen;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints;

public sealed class FrozenCollectionCreateHintTests : CreateHintTestBase<FrozenCollectionCreateHint>
{
    private static readonly FrozenCollectionCreateHint _TestInstance = new();

    private static readonly Type[] _ItemTypes =
    [
        typeof(string),
        typeof(object),
        typeof(int),
        typeof(double),
        typeof(KeyValuePair<string, int>)
    ];

    private static readonly Type[] _ValidTypes =
    [
        MakeDefined(typeof(FrozenSet<>)),
        MakeDefined(typeof(FrozenDictionary<,>))
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(object),
        typeof(IEnumerable),
        typeof(IEnumerable<>)
    ];

    public FrozenCollectionCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    private static Type MakeDefined(Type type)
    {
        if (type.IsGenericTypeDefinition)
        {
            FastRandom random = new();
            return type.MakeGenericType([.. type.GetGenericArguments().Select(t => random.NextItem(_ItemTypes))]);
        }
        else
        {
            return type;
        }
    }
}