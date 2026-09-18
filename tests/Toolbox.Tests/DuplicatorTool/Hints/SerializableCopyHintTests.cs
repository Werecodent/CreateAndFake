using System.Runtime.Serialization;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class SerializableCopyHintTests : CopyHintTestBase<SerializableCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Exception),
        typeof(AggregateException),
        typeof(IOException),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public SerializableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryCopy_InvalidDataContractExceptionRethrown([Stub] ISerializable data)
    {
        TestInstance.Assert(x => x.TryCopy(data, CreateChainer())).Throws<SerializationException>();
    }
}
