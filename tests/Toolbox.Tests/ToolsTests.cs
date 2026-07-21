using System.Reflection;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests;

public static class ToolsTests
{
    [Fact]
    internal static void CreateAndFake_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(ToolSet)),
            Assembly.GetExecutingAssembly(),
            opt => opt with { TestClassCoverageExceptions = [nameof(Emitter)] }
        );
    }

    [Fact]
    internal static void CreateAndFake_ValidateTestMethodNaming()
    {
        Tools.Tester.VerifyTestMethodNaming(
            [typeof(FactAttribute), typeof(TheoryAttribute)],
            Assembly.GetAssembly(typeof(ToolSet)),
            Assembly.GetExecutingAssembly()
        );
    }

    [Fact]
    internal static Task CreateAndFake_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            Assembly.GetExecutingAssembly(),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void CreateAndFake_VerifyAllToStrings()
    {
        Tools.Tester.VerifyAllToStrings(Assembly.GetAssembly(typeof(ToolSet)));
    }

    [Fact]
    internal static Task Tools_VerifyIntegrity()
    {
        return Tools.Tester.VerifyToolSetIntegrityAsync(TestContext.Current.CancellationToken);
    }

    /* [Fact]
    internal static Task Tools_SupportsAll()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            TypeDescriber.For<object>().FindLoadedSubclasses(),
            TestContext.Current.CancellationToken
        );
    }*/

    [Theory, RandomData]
    internal static void Tools_IntegrationWorks(
        DataHolderSample original,
        [Fake] DataHolderSample faked
    )
    {
        DataHolderSample dupe = original.Tools().Copy();

        original.Assert().Is(dupe).And.IsNot(original.Tools().Variant());

        faked.HasNested(dupe).SetupReturn(true, Times.Once);
        faked.HasNested(original).Assert().Is(true, "Value equality did not work for args.");
        faked.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Tools_HandlesInfinites(InfiniteSample sample)
    {
        Tools.Mutator.Assert(x => x.Variant(sample)).Throws<ToolException>();

        InfiniteSample dupe = Tools.Duplicator.Copy(sample);

        dupe.Assert().Is(sample);
        Tools.Valuer.GetHashCode(dupe).Assert().Is(Tools.Valuer.GetHashCode(sample));
    }

    /* [Fact]
    internal static Task Tools_AllCreateAndFakeTypesWork()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            typeof(Tools)
                .Assembly.GetTypes()
                .Where(t => !(t.IsAbstract && t.IsSealed))
                .Where(t => !TypeDescriber.For(t).Inherits<Attribute>())
                .Where(t => !t.IsNestedPrivate)
                .Where(t => !Attribute.IsDefined(t, typeof(CompilerGeneratedAttribute))),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IntegrityIgnorableTypes =
                    [
                        typeof(Arg),
                        typeof(Fake<>),
                        typeof(VoidType),
                        typeof(VoidReturn),
                        typeof(AnyGeneric),
                        typeof(Injected<>),
                        typeof(Behavior<>),
                        typeof(FactoryCreateHandler<>),
                        typeof(FactoryCopyHandler<>),
                        typeof(ToolSet),
                        typeof(Tools),
                        typeof(BaseGuarder),
                        typeof(IValuerAsyncComparable),
                        typeof(DifferenceHintAsyncResult),
                    ],
                }
        );
    }*/

    [Fact]
    internal static Task Tools_TestIndividual()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            [typeof(ISet<int>)],
            TestContext.Current.CancellationToken
        );
    }
}
