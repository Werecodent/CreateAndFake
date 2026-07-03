using CreateAndFake.AsserterTool;
using CreateAndFake.Design;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue015Tests
{
    internal static class Sample
    {
        public static void Bad(int[] value)
        {
            ArgumentGuard.ThrowIfNull(value, nameof(value));

            value[0] = value[0].Tools().Variant();
        }
    }

    [Fact]
    internal static Task Issue015_GuardsParameterMutation()
    {
        return Tools
            .Tester.PreventsParameterMutationAsync(
                typeof(Sample),
                TestContext.Current.CancellationToken,
                opt => opt with { DisableParameterMutationTests = false }
            )
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }
}
