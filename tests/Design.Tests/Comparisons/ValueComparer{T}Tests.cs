using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Design.Tests.Comparisons;

public sealed class ValueComparer_T_Tests() : AutoTester(typeof(ValueComparer<>))
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(IterationLimitException)] };

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
