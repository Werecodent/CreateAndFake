using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Extensions;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool.Handlers;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Handlers;
using CreateAndFake.RunnerTool;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.Engine;
using Xunit.Internal;

namespace CreateAndFake.Tests;

public static class ToolsTests
{
    [Fact]
    internal static void CreateAndFake_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
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
    internal static Task Tools_AllSupportedTypesValid()
    {
        return Tools.Tester.VerifyToolSetIntegrityAsync(
            ToolSet.DefaultSet,
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void Tools_IntegrationWorks(
        DataHolderSample original,
        [Fake] DataHolderSample faked
    )
    {
        DataHolderSample dupe = original.CreateDeepClone();

        original.Assert().Is(dupe).And.IsNot(original.CreateVariant());

        faked.HasNested(dupe).SetupReturn(true, Times.Once);
        faked.HasNested(original).Assert().Is(true, "Value equality did not work for args.");
        faked.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Tools_HandlesInfinites(InfiniteSample sample)
    {
        Tools.Mutator.Assert(m => m.Variant(sample)).Throws<ToolException>();

        InfiniteSample dupe = Tools.Duplicator.Copy(sample);

        dupe.Assert().Is(sample);
        Tools.Valuer.GetHashCode(dupe).Assert().Is(Tools.Valuer.GetHashCode(sample));
    }

    /* [Fact, ExcludeFromCodeCoverage] */
    internal static async Task Tools_AllCreateAndFakeTypesWork()
    {
        Type[] ignore =
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
        ];

        Dictionary<Type, Exception> failures = [];

        foreach (
            Type type in typeof(Tools)
                .Assembly.GetTypes()
                .Where(t => !(t.IsAbstract && t.IsSealed))
                .Where(t => !t.Inherits<Attribute>())
                .Where(t => !ignore.Contains(t))
                .Where(t => !t.IsNestedPrivate)
                .Where(t => !Attribute.IsDefined(t, typeof(CompilerGeneratedAttribute)))
        )
        {
            try
            {
                await TestTrip(type);
            }
            catch (Exception e)
            {
                failures.Add(type, e.Unwrap());
            }
        }
        failures.Assert().IsEmpty();
    }

    /*[Fact]
    internal static async Task Tools_ValidSamplesWork()
    {
        Dictionary<Type, Exception> failures = [];

        foreach (Type type in SampleGenerator.AllValidDataSamples)
        {
            try
            {
                await TestTrip(type);
            }
            catch (Exception e)
            {
                failures.Add(type, e.Unwrap());
            }
        }
        failures
            .Select(f => (f.Key.Name, f.Value.GetType().Name, f.Value.Message))
            .Assert()
            .IsEmpty();
    }

    [Fact]
    internal static Task Tools_TestIndividual()
    {
        return TestTrip(typeof(BaseHolder<>));
    }*/

    [Fact]
    internal static async Task Tools_ExceptionTypesWork()
    {
        Type type = typeof(Exception);

        for (int i = 0; i < 100; i++)
        {
            await TestTrip(type);
        }
    }

    /// <summary>Verifies the type works with the tools.</summary>
    /// <param name="type">Type to test.</param>
    private static async Task TestTrip(Type type)
    {
        CancellationToken ct = TestContext.Current.CancellationToken;
        string failMessage = "Behavior did not work for type '" + type.FullName + "'.";
        object original = null,
            variant = null,
            dupe = null;
        try
        {
            original = Tools.Randomizer.Create(type);
            dupe = Tools.Duplicator.Copy(original);

            await Tools.Asserter.ValuesEqualAsync(original, dupe, ct, failMessage);
            await Tools.Asserter.ValuesEqualAsync(
                await Tools.Valuer.GetHashCodeAsync(original, ct),
                await Tools.Valuer.GetHashCodeAsync(dupe, ct),
                ct,
                $"HashCode {failMessage}"
            );

            if (
                TypeDescriber.GetAllProperties(type).Any() || TypeDescriber.GetAllFields(type).Any()
            )
            {
                variant = Tools.Mutator.Variant(type, original);

                await Tools.Asserter.ValuesNotEqualAsync(original, variant, ct, failMessage);
                await Tools.Asserter.ValuesNotEqualAsync(
                    await Tools.Valuer.GetHashCodeAsync(original, ct),
                    await Tools.Valuer.GetHashCodeAsync(variant, ct),
                    ct,
                    failMessage
                );

                if (Tools.Mutator.Modify(original))
                {
                    await Tools.Asserter.ValuesNotEqualAsync(dupe, original, ct);
                }
            }

            if (
                Tools.Faker.Supports(type)
                && !type.Inherits<IDisposable>()
                && !type.Inherits<IToolOptions>()
            )
            {
                Tools.Faker.Mock(type);
            }
        }
        finally
        {
            await Disposer.CleanupAsync(original, variant, dupe);
        }
    }
}
