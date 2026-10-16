using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RunnerTool.Attributes;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RunnerTool.Attributes;

public static class BaseCapAttributeTests
{
    [Fact]
    internal static Task BaseCapAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<BaseCapAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task BaseCapAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<BaseCapAttribute>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(InvalidCastException),
                        typeof(ToolException),
                    ],
                }
        );
    }

    [Fact]
    internal static Task CreateParameterValue_PreventsMismatchedMinValue()
    {
        return Tools
            .Runner.CreateForAsync(
                typeof(BaseCapAttributeTests).GetMethod(
                    nameof(BadMinValueMatch),
                    BindingFlags.Static | BindingFlags.NonPublic
                ),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<ToolException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task CreateParameterValue_PreventsMismatchedMaxValue()
    {
        return Tools
            .Runner.CreateForAsync(
                typeof(BaseCapAttributeTests).GetMethod(
                    nameof(BadMaxValueMatch),
                    BindingFlags.Static | BindingFlags.NonPublic
                ),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<ToolException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task CreateParameterValue_PreventsNonValue()
    {
        return Tools
            .Runner.CreateForAsync(
                typeof(BaseCapAttributeTests).GetMethod(
                    nameof(BadTypeValueMatch),
                    BindingFlags.Static | BindingFlags.NonPublic
                ),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<ToolException>(TestContext.Current.CancellationToken);
    }

    private static int BadMinValueMatch([Cap(2.3, 5)] int size)
    {
        return size;
    }

    private static int BadMaxValueMatch([Cap(3, 4u)] int size)
    {
        return size;
    }

    private static DataSample BadTypeValueMatch([Cap(3, 4)] DataSample sample)
    {
        return sample;
    }
}
