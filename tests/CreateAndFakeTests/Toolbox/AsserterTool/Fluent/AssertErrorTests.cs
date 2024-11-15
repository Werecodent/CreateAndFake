using CreateAndFake.Toolbox.AsserterTool;

namespace CreateAndFakeTests.Toolbox.AsserterTool.Fluent;

public static class AssertErrorTests
{
    [Fact]
    internal static void AssertError_GuardsNulls()
    {
        // Fix me: Tools.Tester.PreventsNullRefException<AssertError>();
    }

    [Fact]
    internal static void AssertError_NoParameterMutation()
    {
        // Fix me: Tools.Tester.PreventsParameterMutation<AssertError>();
    }

    [Theory, RandomData]
    internal static void Fail_Throws(Exception error)
    {
        error.Assert(d => d.Assert().Fail()).Throws<AssertException>();
    }
}
