using CreateAndFake.AsserterTool.Fluent;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public static class AssertStringTests
{
    [Fact]
    internal static void AssertString_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AssertString>();
    }

    [Fact]
    internal static void AssertString_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AssertString>();
    }

    [Theory, RandomData]
    internal static void AssertString_CallsAndChains(Injected<AssertString> instance)
    {
        RunResults results = Tools.Runner.CallMethodsOn(instance.Dummy);
        results.RawResults
            .Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertString>)
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}
