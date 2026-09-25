using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Exceptions;

namespace Werecodent.CreateAndFake.Design.Tests.Comparisons;

public static class ValueComparer_T_Tests
{
    [Fact]
    internal static void Debug_ValueComparer_ToString()
    {
        typeof(ValueComparer<>).Tools().CreateRandomInstance().ToString().Assert().Debug();
    }

    [Fact]
    internal static Task ValueComparer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ValueComparer<>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(IterationLimitException)] }
        );
    }

    [Fact]
    internal static Task ValueComparer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ValueComparer<>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(IterationLimitException)] }
        );
    }

    [Fact]
    internal static Task ValueComparer_VerifyToolSupport()
    {
        return Tools.Tester.VerifyToolSetSupportAsync<ValueComparer<IValueEquatable>>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ValueComparer_PassthroughWithNoExceptions()
    {
        return Tools.Tester.PassthroughWithNoExceptionsAsync(
            ValueComparer<IValueEquatable>.Use,
            TestContext.Current.CancellationToken
        );
    }
}
