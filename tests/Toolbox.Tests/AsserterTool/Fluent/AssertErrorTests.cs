using CreateAndFake.AsserterTool.Fluent;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public static class AssertErrorTests
{
    [Fact]
    internal static void AssertError_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AssertError>();
    }

    [Fact]
    internal static void AssertError_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AssertError>();
    }

    [Theory, RandomData]
    internal static void AssertError_CallsAndChains(Injected<AssertError> instance)
    {
        RunResults results = Tools.Runner.CallMethodsOn(instance.Dummy);
        results.RawResults
            .Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertError>)
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}
