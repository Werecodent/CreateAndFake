using CreateAndFake.Design.Types;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue065Tests
{
    internal sealed class InfiniteA
    {
        public int Id { get; set; }

        public ICollection<InfiniteB> Start { get; set; }

        public override string ToString()
        {
            return TypeHelper.ExpandedName(GetType());
        }
    }

    internal sealed class InfiniteB
    {
        public InfiniteC Nested { get; set; }

        public override string ToString()
        {
            return TypeHelper.ExpandedName(GetType());
        }
    }

    internal sealed class InfiniteC
    {
        public InfiniteA Back { get; set; }

        public override string ToString()
        {
            return TypeHelper.ExpandedName(GetType());
        }
    }

    [Fact]
    internal static Task Issue065_HandlesInfiniteChains()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            [typeof(InfiniteA), typeof(InfiniteB), typeof(InfiniteC)],
            TestContext.Current.CancellationToken
        );
    }
}
