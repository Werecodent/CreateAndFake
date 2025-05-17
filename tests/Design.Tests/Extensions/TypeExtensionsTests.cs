using System.Reflection;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Design.Tests.Extensions;

public static class TypeExtensionsTests
{
    [Fact]
    internal static Task TypeExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(typeof(TypeExtensions));
    }

    [Fact]
    internal static void Inherits_RaceConditionPrevented()
    {
        Type testType = Tools
            .Faker.Stub<object>(
                [
                    .. Assembly
                        .GetExecutingAssembly()
                        .GetTypes()
                        .Where(t => t.IsInterface)
                        .Where(t => t.IsVisible),
                ]
            )
            .GetType();

        Parallel.For(0, 10, i => testType.Inherits<object>()).IsCompleted.Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void FindLoadedTypes_IgnoresMissingAssembly(
        Fake<Assembly> assembly,
        FileNotFoundException error
    )
    {
        assembly.Setup(d => d.GetTypes(), Behavior.Throw(error));

        TypeExtensions.FindLoadedTypes(assembly.Dummy).Assert().IsEmpty();
    }

    [Theory, RandomData]
    internal static void FindLoadedTypes_IgnoresReflectError(
        Fake<Assembly> assembly,
        ReflectionTypeLoadException error
    )
    {
        assembly.Setup(d => d.GetTypes(), Behavior.Throw(error));

        TypeExtensions.FindLoadedTypes(assembly.Dummy).Assert().IsEmpty();
    }
}
