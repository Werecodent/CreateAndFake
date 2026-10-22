using Werecodent.CreateAndFake.RandomizerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class SpanCreateHintTests : CreateHintTestBase<SpanCreateHint>
{
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

    public SpanCreateHintTests()
        : base(_ValidTypes) { }

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
