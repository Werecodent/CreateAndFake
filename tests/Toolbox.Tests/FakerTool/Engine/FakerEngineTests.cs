using System.Collections.Frozen;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Engine;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Samples.ErrorCases;
using Werecodent.CreateAndFake.Tests.FakerTool.TestSamples;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Engine;

public static class FakerEngineTests
{
    [Fact]
    internal static Task FakerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            new FakerEngine(),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    MethodsToIgnore = FrozenSet.ToFrozenSet([
                        "SelectHints",
                        "Inject",
                        "InjectMocks",
                        "InjectStubs",
                    ]),
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(InvalidOperationException),
                    ],
                }
        );
    }

    [Fact]
    internal static Task FakerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new FakerEngine(),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    MethodsToIgnore = FrozenSet.ToFrozenSet([
                        "SelectHints",
                        "Inject",
                        "InjectMocks",
                        "InjectStubs",
                    ]),
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
        sample.Dummy.Value2.Assert().IsNull();
    }

    [Fact]
    internal static void Inject_ConstructorRequired()
    {
        Tools
            .Faker.Assert(x => x.InjectStubs<IOnlyMockSample>())
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

        sample.Dummy.Sample1.Text.Assert().IsNull();
        sample.Dummy.Sample2.Assert(x => x.Calc()).Throws<FakeCallException>();
        sample.Dummy.Value1.Assert().Is(num);
        sample.Dummy.Value2.Assert().Is(text);
    }
}
