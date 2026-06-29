using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Engine;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Tests.FakerTool.TestSamples;

namespace CreateAndFake.Tests.FakerTool.Engine;

public static class FakerChainerTests
{
    [Fact]
    internal static Task FakerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakerChainer>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(InvalidOperationException),
                    ],
                }
        );
    }

    [Fact]
    internal static Task FakerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakerChainer>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(InvalidOperationException),
                    ],
                }
        );
    }

    [Fact]
    internal static void Inject_HandlesValues()
    {
        Injected<FakeHolderSample> sample = Tools.Faker.InjectMocks<FakeHolderSample>();
        sample.Dummy.Value1.Assert().Is(0);
        sample.Dummy.Value2.Assert().Is(null);
    }

    [Fact]
    internal static void Inject_ConstructorRequired()
    {
        Tools
            .Faker.Assert(f => f.InjectStubs<IOnlyMockSample>())
            .Throws<InvalidOperationException>();
    }

    [Theory, RandomData]
    internal static void Inject_UsesValues(int num, string text)
    {
        Injected<FakeHolderSample> sample = Tools.Faker.InjectMocks<FakeHolderSample>([
            null,
            Tools.Faker.Stub<AbstractFakeSample>(),
            num,
            text,
        ]);

        sample.Dummy.Sample1.Text.Assert().Is(null);
        sample.Dummy.Sample2.Assert(s => s.Calc()).Throws<FakeCallException>();
        sample.Dummy.Value1.Assert().Is(num);
        sample.Dummy.Value2.Assert().Is(text);
    }
}
