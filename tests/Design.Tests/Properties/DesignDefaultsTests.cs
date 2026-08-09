using Werecodent.CreateAndFake.Design.Properties;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Tests.Properties;

public static class DesignDefaultsTests
{
    [Fact]
    internal static Task DesignDefaults_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(DesignDefaults),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task DesignDefaults_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(DesignDefaults),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void IterationLimit_ObjectSubclassMinimum()
    {
        TypeDescriber
            .For<object>()
            .FindLoadedSubclasses()
            .Count()
            .Assert()
            .LessThan(DesignDefaults.IterationLimit);
    }
}
