using CreateAndFake.Design.Exceptions;
using CreateAndFake.MutatorTool;

namespace CreateAndFake.Tests.MutatorTool;

public static class MutatorTests
{
    [Fact]
    internal static Task Mutator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<Mutator>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ToolException)] }
        );
    }

    [Fact]
    internal static Task Mutator_PassthroughWithNoExceptions()
    {
        return Tools.Tester.PassthroughWithNoExceptionsAsync(
            Tools.Mutator,
            TestContext.Current.CancellationToken
        );
    }
}
