using CreateAndFake.Fluent.Tooling;

namespace CreateAndFake.Tests.Fluent.Tooling;

public static class TypeToolsTests
{
    [Fact]
    internal static Task TypeTools_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TypeTools),
            TestContext.Current.CancellationToken
        );
    }
}
