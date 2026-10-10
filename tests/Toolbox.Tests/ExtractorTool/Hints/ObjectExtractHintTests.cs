using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.ExtractorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class ObjectExtractHintTests : ExtractHintTestBase<ObjectExtractHint>
{
    private sealed class HiddenData
    {
        private readonly string _data = "A";

        private string Data { get; } = "B";

        public override string ToString()
        {
            return Data + _data;
        }
    }

    private static readonly Type[] _ValidTypes = [typeof(DataHolderSample)];

    public ObjectExtractHintTests()
        : base(_ValidTypes, []) { }

    [Fact]
    internal void Extract_UtilizesExtractPrivateMembersFlag()
    {
        HiddenData sample = new();
        object[] publics = [sample];
        object[] privates = [sample, "B", "A"];

        Tools.Extractor.Extract(sample).Assert().Is(publics);

        Tools
            .Extractor.Extract(sample, opt => opt with { ExtractPrivateMembers = true })
            .Assert()
            .Is(privates);
    }

    [Fact]
    internal async Task ExtractAsync_UtilizesExtractPrivateMembersFlag()
    {
        HiddenData sample = new();
        object[] publics = [sample];
        object[] privates = [sample, "B", "A"];

        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );
        await map.Assert().IsAsync(publics, TestContext.Current.CancellationToken);

        IAsyncContentMap map2 = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken,
            opt => opt with { ExtractPrivateMembers = true }
        );
        await map2.Assert().IsAsync(privates, TestContext.Current.CancellationToken);
    }
}
