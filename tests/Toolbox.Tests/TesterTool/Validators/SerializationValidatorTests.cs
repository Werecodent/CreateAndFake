using System.Runtime.Serialization;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Samples.ErrorCases;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public sealed class SerializationValidatorTests() : AutoTester(typeof(SerializationValidator))
{
    protected override TesterMod OptionConfiguration { get; } =
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
            };

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

    [Fact]
    internal static void VerifyXmlSerialization_ExceptionsWrapped()
    {
        Tools
            .Tester.Assert(x => x.VerifyXmlSerialization<InjectMockSample>())
            .Throws<TesterFailureException>();
    }
}
