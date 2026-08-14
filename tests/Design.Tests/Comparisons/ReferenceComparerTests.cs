using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Design.Tests.Comparisons;

public static class ReferenceComparerTests
{
    [Fact]
    internal static Task ReferenceComparer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ReferenceComparer>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ReferenceComparer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ReferenceComparer>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void Equals_UsesReferenceComparison(DataSample data, [Copy] DataSample dupe)
    {
        ReferenceComparer.Use.Equals(null, null).Assert().Is(true);
        ReferenceComparer.Use.Equals(data, data).Assert().Is(true);
        ReferenceComparer.Use.Equals(data, null).Assert().Is(false);
        ReferenceComparer.Use.Equals(data, dupe).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void GetHashCode_UsesReferenceValue(DataSample data)
    {
        int originalHash = ReferenceComparer.Use.GetHashCode(data);

        Tools.Mutator.Modify(data).Assert().Is(true);
        ReferenceComparer.Use.GetHashCode(data).Assert().Is(originalHash);

        Limiter.Few.Retry(
            "Occasional hash collisions acceptable.",
            () =>
                ReferenceComparer.Use.GetHashCode(data.Tools().Copy()).Assert().IsNot(originalHash),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void GetHashCode_NullIsValid()
    {
        ReferenceComparer.Use.Assert(x => x.GetHashCode(null)).ThrowsNo<Exception>();
    }

    [Theory, RandomData]
    internal static void Compare_UsesReferenceValue(DataSample data, [Copy] DataSample dupe)
    {
        ReferenceComparer.Use.Compare(data, data).Assert().Is(0);
        ReferenceComparer.Use.Compare(data, dupe).Assert().IsNot(0);
    }
}
