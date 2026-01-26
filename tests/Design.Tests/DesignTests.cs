using System.Collections.Frozen;
using System.Reflection;

namespace CreateAndFake.Design.Tests;

public static class DesignTests
{
    [Fact]
    internal static void Design_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(ArgumentGuard)),
            Assembly.GetExecutingAssembly(),
            opt =>
                opt with
                {
                    TestClassCoverageExceptions = FrozenSet.ToFrozenSet([
                        "CallerArgumentExpressionAttribute",
                        "CompilerFeatureRequiredAttribute",
                        "IsExternalInit",
                        "RequiredMemberAttribute",
                        "DoesNotReturnAttribute",
                        "MaybeNullAttribute",
                        "NotNullAttribute",
                        "NotNullIfNotNullAttribute",
                        "NotNullWhenAttribute",
                        "SetsRequiredMembersAttribute",
                        "ValueHandler`1",
                    ]),
                }
        );
    }

    [Fact]
    internal static Task Design_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParameters(
            Assembly.GetExecutingAssembly(),
            TestContext.Current.CancellationToken
        );
    }
}
