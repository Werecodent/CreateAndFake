using System.Runtime.Serialization;
using CreateAndFake.AsserterTool;
using CreateAndFake.TesterTool.Validators;

namespace CreateAndFake.Tests.TesterTool.Validators;

public static class SerializationValidatorTests
{
    [Fact]
    internal static Task SerializationValidator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<SerializationValidator>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task SerializationValidator_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<SerializationValidator>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(AssertException),
                        typeof(SerializationException),
                        typeof(ArgumentOutOfRangeException),
                        typeof(InvalidCastException),
                    ],
                }
        );
    }
}
