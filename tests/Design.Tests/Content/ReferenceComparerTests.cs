using CreateAndFake.Design.Content;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Design.Tests.Content;

public static class ReferenceComparerTests
{
    [Fact]
    internal static Task ReferenceComparer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ReferenceComparer>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ReferenceComparer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ReferenceComparer>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void Equals_UsesReferenceComparison(DataSample data)
    {
        ReferenceComparer.Use.Equals(null, null).Assert().Is(true);
        ReferenceComparer.Use.Equals(data, data).Assert().Is(true);
        ReferenceComparer.Use.Equals(data, null).Assert().Is(false);
        ReferenceComparer.Use.Equals(data, data.CreateDeepClone()).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void GetHashCode_UsesReferenceValue(DataSample data)
    {
        int originalHash = ReferenceComparer.Use.GetHashCode(data);
        Tools.Mutator.Modify(data).Assert().Is(true);
        ReferenceComparer.Use.GetHashCode(data).Assert().Is(originalHash);
        ReferenceComparer.Use.GetHashCode(null).Assert().Is(0);
    }

    [Theory, RandomData]
    internal static void Compare_UsesReferenceValue(DataSample data)
    {
        ReferenceComparer.Use.Compare(data, data).Assert().Is(0);
        ReferenceComparer.Use.Compare(data, data.CreateDeepClone()).Assert().IsNot(0);
    }
}
