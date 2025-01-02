using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.AsserterTool.Fluent;

namespace CreateAndFakeTests.Toolbox.AsserterTool.Fluent;

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
