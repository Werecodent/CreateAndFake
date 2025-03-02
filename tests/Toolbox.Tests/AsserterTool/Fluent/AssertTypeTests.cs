using CreateAndFake.AsserterTool.Fluent;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public static class AssertTypeTests
{
    [Fact]
    internal static void AssertType_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AssertType>();
    }

    [Fact]
    internal static void AssertType_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AssertType>();
    }

    [Theory, RandomData]
    internal static void AssertType_CallsAndChains(Injected<AssertType> instance)
    {
        RunResults results = Tools.Runner.CallMethodsOn(instance.Dummy);
        results.RawResults
            .Where(r => r.Result != null)
            .Where(r => r.Result is not Exception)
            .Where(r => r.Result is not AssertChainer<AssertType>)
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}
