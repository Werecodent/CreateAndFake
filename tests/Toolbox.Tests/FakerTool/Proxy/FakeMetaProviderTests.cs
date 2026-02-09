using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.Tests.FakerTool.Proxy;

public static class FakeMetaProviderTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(FakeCallException),
                typeof(ToolException),
                typeof(FakeVerifyException),
                typeof(InvalidOperationException),
            ],
        };

    [Fact]
    internal static Task FakeMetaProvider_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeMetaProvider>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task FakeMetaProvider_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeMetaProvider>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Theory, RandomData]
    internal static void Verify_PresetOutOfRangeThrows(string name)
    {
        FakeMetaProvider provider = new(0) { ThrowByDefault = false };

        CallData data = new(name, Type.EmptyTypes, [], Tools.Faker.Options);

        provider.SetCallBehavior(data, Behavior.None(Times.Once));
        provider.Assert(p => p.Verify()).Throws<FakeVerifyException>();

        provider.CallVoid(null, Tools.Mutator.Variant(name), Type.EmptyTypes, []);
        provider.Assert(p => p.Verify()).Throws<FakeVerifyException>();

        provider.CallVoid(null, name, Type.EmptyTypes, []);
        provider.Verify();

        provider.CallVoid(null, name, Type.EmptyTypes, []);
        provider.Assert(p => p.Verify()).Throws<FakeVerifyException>();
    }

    [Theory, RandomData]
    internal static void Verify_CustomOutOfRangeThrows(string name)
    {
        FakeMetaProvider provider = new(0) { ThrowByDefault = false };

        CallData data = new(name, Type.EmptyTypes, [], Tools.Faker.Options);

        provider.Verify(0, data);
        provider.Assert(p => p.Verify(1, data)).Throws<FakeVerifyException>();

        provider.CallVoid(null, name.CreateVariant(), Type.EmptyTypes, []);
        provider.Verify(0, data);
        provider.Assert(p => p.Verify(1, data)).Throws<FakeVerifyException>();

        provider.CallVoid(null, name, Type.EmptyTypes, []);
        provider.Assert(p => p.Verify(0, data)).Throws<FakeVerifyException>();
        provider.Verify(1, data);

        provider.CallVoid(null, name, Type.EmptyTypes, []);
        provider.Assert(p => p.Verify(1, data)).Throws<FakeVerifyException>();
        provider.Verify(2, data);
    }

    [Theory, RandomData]
    internal static void VerifyTotalCalls_OutOfRangeThrows(string name)
    {
        FakeMetaProvider provider = new(0) { ThrowByDefault = false };

        provider.VerifyTotalCalls(0);
        provider.Assert(p => p.VerifyTotalCalls(1)).Throws<FakeVerifyException>();

        provider.CallVoid(null, name, Type.EmptyTypes, []);
        provider.Assert(p => p.VerifyTotalCalls(0)).Throws<FakeVerifyException>();
        provider.VerifyTotalCalls(1);

        provider.CallVoid(null, name.CreateVariant(), Type.EmptyTypes, []);
        provider.Assert(p => p.VerifyTotalCalls(1)).Throws<FakeVerifyException>();
        provider.VerifyTotalCalls(2);
    }

    [Theory, RandomData]
    internal static void CallVoid_ReturnValueThrows(string name)
    {
        FakeMetaProvider provider = new(0);

        CallData data = new(name, Type.EmptyTypes, [], Tools.Faker.Options);
        provider.SetCallBehavior(data, Behavior.Returns(""));

        provider
            .Assert(p => p.CallVoid(null, name, Type.EmptyTypes, []))
            .Throws<InvalidOperationException>();
    }

    [Theory, RandomData]
    internal static void SetLastCallBehavior_RequiresPreviousCall(Behavior behavior)
    {
        behavior
            .Assert(b =>
            {
                FakeMetaProvider.SetLastCallBehavior(b);
                FakeMetaProvider.SetLastCallBehavior(b);
            })
            .Throws<InvalidOperationException>();
    }
}
