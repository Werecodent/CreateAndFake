using System.Runtime.Serialization;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

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

    [Theory, RandomData]
    internal static void VerifyJsonSerialization_Functional(string sample)
    {
        Tools.Tester.VerifyJsonSerialization<string>();
        Tools.Tester.VerifyJsonSerialization(sample);
    }

    [Theory, RandomData]
    internal static void VerifyXmlSerialization_Functional(string sample)
    {
        Tools.Tester.VerifyXmlSerialization<string>();
        Tools.Tester.VerifyXmlSerialization(sample);
    }
}
