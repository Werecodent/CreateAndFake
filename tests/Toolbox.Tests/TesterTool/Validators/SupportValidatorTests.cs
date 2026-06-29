using System.Runtime.Serialization;
using CreateAndFake.AsserterTool;
using CreateAndFake.TesterTool.Validators;

namespace CreateAndFake.Tests.TesterTool.Validators;

public static class SupportValidatorTests
{
    [Fact]
    internal static Task SupportValidator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<SupportValidator>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task SupportValidator_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<SupportValidator>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(AssertException),
                        typeof(SerializationException),
                        typeof(PlatformNotSupportedException),
                    ],
                }
        );
    }
}
