using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Design.Tests.Types;

public sealed class ScopeCheckerTests() : AutoTester(typeof(ScopeChecker))
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(InvalidOperationException)] };

    [Fact]
    internal static void FindLoadedSpecificTypes_IncludesOnlyClasses()
    {
        ScopeChecker
            .FindLoadedSpecificTypes(typeof(DataSample).Assembly)
            .Assert()
            .Contains(typeof(DataSample))
            .And()
            .ContainsNot(typeof(IIsGoodOrBadSample));
    }

    [Theory, RandomData]
    internal static void FindLoadedTypes_IgnoresMissingAssembly(
        [Stub] Assembly assembly,
        FileNotFoundException error
    )
    {
        assembly.GetTypes().SetupReturn(Behavior<Type[]>.Throw(error, Times.Once));
        ScopeChecker.FindLoadedTypes(assembly).Assert().IsEmpty();
        assembly.Assert().Called();
    }

    [Theory, RandomData]
    internal static void FindLoadedTypes_IgnoresReflectError(
        [Stub] Assembly assembly,
        ReflectionTypeLoadException error
    )
    {
        assembly.GetTypes().SetupReturn(Behavior<Type[]>.Throw(error, Times.Once));
        ScopeChecker.FindLoadedTypes(assembly).Assert().IsEmpty();
        assembly.Assert().Called();
    }

    [Fact]
    internal static void IsVisible_TrueForPublicClasses()
    {
        ScopeChecker.IsVisible<DataSample>(typeof(string).Assembly.GetName()).Assert().Is(true);
    }

    [Fact]
    internal static void IsVisible_TrueForInternalsWithAttribute()
    {
        ScopeChecker
            .IsVisible<InternalSample>(Assembly.GetExecutingAssembly().GetName())
            .Assert()
            .Is(true);
    }

    [Fact]
    internal static void IsVisible_FalseForInternalsWithoutAttribute()
    {
        ScopeChecker
            .IsVisible<InternalSample>(typeof(string).Assembly.GetName())
            .Assert()
            .Is(false);
    }
}
