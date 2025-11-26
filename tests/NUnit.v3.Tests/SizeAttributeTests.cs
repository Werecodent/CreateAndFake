namespace CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class SizeAttributeTests
{
    [Test]
    public static Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>();
    }

    [Test]
    public static Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>();
    }
}
