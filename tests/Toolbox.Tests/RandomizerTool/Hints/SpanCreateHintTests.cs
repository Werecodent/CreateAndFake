using System.Collections;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class SpanCreateHintTests : CreateHintTestBase<SpanCreateHint>
{
    private static readonly SpanCreateHint _TestInstance = new();

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
        MakeDefined(typeof(Span<>)),
        MakeDefined(typeof(ReadOnlySpan<>)),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(DataHolderSample),
        typeof(IEnumerable),
        typeof(IEnumerable<>),
    ];

    public SpanCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    private static Type MakeDefined(Type type)
    {
        if (type.IsGenericTypeDefinition)
        {
            return type.MakeGenericType([
                .. type.GetGenericArguments().Select(_ => Tools.Gen.NextItem(_ItemTypes)),
            ]);
        }
        else
        {
            return type;
        }
    }
}
