using System.Reflection;

namespace CreateAndFake.Design.Tests.Extensions;

public static class TypeExtensionsTests
{
    [Fact]
    internal static Task TypeExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(TypeExtensions),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void Inherits_RaceConditionPrevented()
    {
        Type testType = Tools
            .Faker.Stub<object>([
                .. Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsInterface)
                    .Where(t => t.IsVisible),
            ])
            .GetType();

        Parallel.For(0, 10, _ => testType.Inherits<object>()).IsCompleted.Assert().Is(true);
    }

    [Fact]
    internal static void Inherits_SelfIncluded()
    {
        typeof(string).Inherits<string>();
    }
}
