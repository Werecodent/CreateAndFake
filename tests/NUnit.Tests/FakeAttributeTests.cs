namespace CreateAndFake.NUnit.Tests;

[TestFixture]
public static class FakeAttributeTests
{
    [Test]
    public static Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>();
    }

    [Test]
    public static Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>();
    }
}
