namespace CreateAndFake.NUnit.Tests;

[TestFixture]
public static class StubAttributeTests
{
    [Test]
    public static Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<StubAttribute>();
    }

    [Test]
    public static Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<StubAttribute>();
    }
}
