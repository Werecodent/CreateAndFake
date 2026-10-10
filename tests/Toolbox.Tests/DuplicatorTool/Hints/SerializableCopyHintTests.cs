using System.Runtime.Serialization;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class SerializableCopyHintTests : CopyHintTestBase<SerializableCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Exception),
        typeof(AggregateException),
        typeof(IOException),
        typeof(AssertException),
    ];

    public SerializableCopyHintTests()
        : base(_ValidTypes) { }

    [Theory, RandomData]
    internal void TryCopy_InvalidDataContractExceptionRethrown([Stub] ISerializable data)
    {
        TestInstance.Assert(x => x.TryCopy(data, CreateChainer())).Throws<SerializationException>();
    }
}
