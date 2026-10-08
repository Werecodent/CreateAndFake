using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class DoubleEnumerableSampleTests
{
    [Fact]
    public static Task DoubleEnumerableSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DoubleEnumerableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task DoubleEnumerableSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DoubleEnumerableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    public static void GetEnumerator_ImplicitIntWorks(
        IEnumerable<int> ints,
        IEnumerable<string> strings
    )
    {
        ((IEnumerable<int>)new DoubleEnumerableSample(ints, strings))
            .GetEnumerator()
            .Assert()
            .IsNotNull();

        ((IEnumerable<int>)new DoubleEnumerableSample(null, strings))
            .GetEnumerator()
            .Assert()
            .IsNotNull();
    }
}
