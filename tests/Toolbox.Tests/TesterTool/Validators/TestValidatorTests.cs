using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public sealed class TestValidatorTests() : AutoTester(typeof(TestValidator))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions = [typeof(AssertException)],
                DisableParameterMutationTests = true,
            };

    [Theory, RandomData]
    internal static void VerifyTestMethodNaming_MatchesTargetOptions(
        [Stub] Assembly codeAssembly,
        [Stub] Assembly testAssembly,
        AssemblyName name
    )
    {
        codeAssembly.GetTypes().SetupReturn([]);
        codeAssembly.GetName().SetupReturn(name);

        testAssembly.GetName().SetupReturn(name);
        testAssembly.GetTypes().SetupReturn([typeof(TestMethodProvider)]);

        new TestValidator(
            Tools.Tester.Options with
            {
                RequirePrefixInTestName = false,
            }
        ).VerifyTestMethodNaming([typeof(ObsoleteAttribute)], codeAssembly, testAssembly);

        new TestValidator(Tools.Tester.Options with { RequirePrefixInTestName = true })
            .Assert(x =>
                x.VerifyTestMethodNaming([typeof(ObsoleteAttribute)], codeAssembly, testAssembly)
            )
            .Throws<AssertException>();
    }

    internal static class TestMethodProvider
    {
        [Obsolete("To mark for testing.")]
        internal static void NameWithoutTarget()
        {
            // Name coverage test.
        }
    }
}
