using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.Tests.FakerTool.TestSamples;

namespace CreateAndFake.Tests.FakerTool;

public static class FakerTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(ArgumentException), typeof(InvalidOperationException)],
        };

    [Fact]
    internal static Task Faker_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<Faker>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static void New_NullValuerValid()
    {
        new Faker(Tools.Faker.Options with { Valuer = null }).Assert().Pass();
    }

    [Fact]
    internal static void Mock_SampleWorks()
    {
        Fake<DataHolderSample> sample = Tools.Faker.Mock<DataHolderSample>();
        sample.ThrowByDefault.Assert().Is(true);
        sample.Dummy.Assert(d => d.HasNested(sample.Dummy.NestedValue)).Throws<FakeCallException>();
    }

    [Fact]
    internal static void Stub_SampleWorks()
    {
        Fake<DataHolderSample> sample = Tools.Faker.Stub<DataHolderSample>();
        sample.ThrowByDefault.Assert().Is(false);
        sample.Dummy.HasNested(null).Assert().Is(false);
    }

    [Fact]
    internal static void Supports_InvalidTypesFalse()
    {
        Tools.Faker.Supports<int>().Assert().Is(false);
        Tools.Faker.Supports<Array>().Assert().Is(false);
        Tools.Faker.Supports<InternalSample>().Assert().Is(false);
        Tools.Faker.Supports(typeof(void*)).Assert().Is(false);
        Tools.Faker.Supports(typeof(ConstraintSample<,>)).Assert().Is(false);
    }

    [Fact]
    internal static void InjectMocks_AreMocks()
    {
        Injected<FakeHolderSample> sample = Tools.Faker.InjectMocks<FakeHolderSample>();

        sample.Dummy.Sample1.Assert(s => s.Calc()).Throws<FakeCallException>();
        sample.Dummy.Sample2.Assert(s => s.Text).Throws<FakeCallException>();
    }

    [Fact]
    internal static void InjectStubs_AreStubs()
    {
        Injected<FakeHolderSample> sample = Tools.Faker.InjectStubs<FakeHolderSample>();

        sample.Dummy.Sample1.Calc().Assert().Is(0);
        sample.Dummy.Sample2.Text.Assert().IsNull();
    }
}
