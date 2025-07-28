using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;
using Xunit.Internal;

namespace CreateAndFake.Tests;

public static class ToolsTests
{
    [Fact]
    internal static void CreateAndFake_Tests_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(ToolSet)),
            Assembly.GetExecutingAssembly()
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

    [Fact, ExcludeFromCodeCoverage]
    internal static async Task Tools_AllCreateAndFakeTypesWork()
    {
        Type[] ignore =
        [
            typeof(Arg),
            typeof(Fake<>),
            typeof(VoidType),
            typeof(AnyGeneric),
            typeof(Injected<>),
            typeof(Behavior<>),
            typeof(ToolSet),
            typeof(Tools),
            typeof(BaseGuarder),
            typeof(IValuerAsyncComparable),
        ];

        Dictionary<Type, Exception> failures = [];

        foreach (
            Type type in typeof(Tools)
                .Assembly.GetTypes()
                .Where(t => !(t.IsAbstract && t.IsSealed))
                .Where(t => !t.Inherits<Attribute>())
                .Where(t => !ignore.Contains(t))
                .Where(t => !t.IsNestedPrivate)
                .Where(t => t.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
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
        string failMessage = "Behavior did not work for type '" + type.FullName + "'.";
        object original = null,
            variant = null,
            dupe = null;
        try
        {
            original = Tools.Randomizer.Create(type);
            dupe = Tools.Duplicator.Copy(original);

            Tools.Asserter.ValuesEqual(original, dupe, failMessage);
            Tools.Asserter.ValuesEqual(
                await Tools.Valuer.GetHashCodeAsync(original),
                await Tools.Valuer.GetHashCodeAsync(dupe),
                $"HashCode {failMessage}"
            );

            if (
                TypeDescriber.GetAllProperties(type).Any() || TypeDescriber.GetAllFields(type).Any()
            )
            {
                variant = Tools.Mutator.Variant(type, original);

                Tools.Asserter.ValuesNotEqual(original, variant, failMessage);
                Tools.Asserter.ValuesNotEqual(
                    await Tools.Valuer.GetHashCodeAsync(original),
                    await Tools.Valuer.GetHashCodeAsync(variant),
                    failMessage
                );

                if (Tools.Mutator.Modify(original))
                {
                    Tools.Asserter.ValuesNotEqual(dupe, original);
                }
            }

            if (Tools.Faker.Supports(type) && !type.Inherits<IDisposable>())
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
