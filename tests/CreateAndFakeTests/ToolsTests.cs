using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.TesterTool;
using CreateAndFakeTests.TestSamples;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace CreateAndFakeTests;

public static class ToolsTests
{
    private const BindingFlags _Mutable = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

    [Theory, RandomData]
    internal static void Tools_IntegrationWorks(DataHolderSample original, [Fake] DataHolderSample faked)
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
        Tools.Mutator.Assert(m => m.Variant(sample)).Throws<TimeoutException>();

        InfiniteSample dupe = Tools.Duplicator.Copy(sample);

        dupe.Assert().Is(sample);
        Tools.Valuer.GetHashCode(dupe).Assert().Is(Tools.Valuer.GetHashCode(sample));
    }

    [Fact, ExcludeFromCodeCoverage]
    internal static void Tools_AllCreateAndFakeTypesWork()
    {
        Type[] ignore = [
            typeof(Fake<>),
            typeof(VoidType),
            typeof(AnyGeneric),
            typeof(Injected<>),
            typeof(Behavior<>),
            typeof(ToolSet),
            typeof(Tools),
            typeof(BaseGuarder),
        ];

        foreach (Type type in typeof(Tools).Assembly.GetTypes()
            .Where(t => !(t.IsAbstract && t.IsSealed))
            .Where(t => !t.Inherits<Attribute>())
            .Where(t => !ignore.Contains(t))
            .Where(t => !t.IsNestedPrivate)
            .Where(t => t.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
            .Where(t => !t.FullName.Contains("<PrivateImplementationDetails>"))
            .Where(t => !t.FullName.Contains("<>z__ReadOnly")))
        {
            try
            {
                TestTrip(type);
            }
            catch (Exception e)
            {
                e.Assert().Fail($"Failed testing type '{type}'.");
                throw;
            }
        }
    }

    [Fact]
    internal static void Tools_ExceptionTypesWork()
    {
        Type type = typeof(Exception);

        for (int i = 0; i < 100; i++)
        {
            TestTrip(type);
        }
    }

    /// <summary>Verifies the type works with the tools.</summary>
    /// <param name="type">Type to test.</param>
    private static void TestTrip(Type type)
    {
        string failMessage = "Behavior did not work for type '" + type.FullName + "'.";
        object
            original = null,
            variant = null,
            dupe = null;
        try
        {
            original = Tools.Randomizer.Create(type);
            dupe = Tools.Duplicator.Copy(original);

            Tools.Asserter.ValuesEqual(original, dupe, failMessage);
            Tools.Asserter.ValuesEqual(
                Tools.Valuer.GetHashCode(original),
                Tools.Valuer.GetHashCode(dupe), $"HashCode {failMessage}");

            if (type.GetProperties(_Mutable).Length != 0 || type.GetFields(_Mutable).Length != 0)
            {
                variant = Tools.Mutator.Variant(type, original);

                Tools.Asserter.ValuesNotEqual(original, variant, failMessage);
                Tools.Asserter.ValuesNotEqual(
                    Tools.Valuer.GetHashCode(original),
                    Tools.Valuer.GetHashCode(variant), failMessage);

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
            Disposer.Cleanup(original, variant, dupe);
        }
    }
}
