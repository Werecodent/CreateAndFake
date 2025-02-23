using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Fluent;

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
    internal static void Fail_Throws(Exception error)
    {
        error.Assert(d => d.Assert().Fail()).Throws<AssertException>();
    }
}
