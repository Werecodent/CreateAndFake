using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public sealed class DoubleEnumerableSampleTests : AutoTester<DoubleEnumerableSample>
{
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
