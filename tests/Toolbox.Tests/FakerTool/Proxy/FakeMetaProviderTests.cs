using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.Tests.FakerTool.Proxy;

public static class FakeMetaProviderTests
{
    private static readonly TesterMod _Config = opt =>
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
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeMetaProvider>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task FakeMetaProvider_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeMetaProvider>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static void Verify_PresetOutOfRangeThrows(string name)
    {
        FakeMetaProvider provider = new(0, Tools.Faker.Options) { ThrowByDefault = false };

        CallData data = new(name, Type.EmptyTypes, [], Tools.Faker.Options);

        provider.SetCallBehavior(data, Behavior.None(Times.Once));
        provider.Assert(x => x.Verify()).Throws<FakeVerifyException>();

        provider.CallVoid(null, Tools.Mutator.Variant(name), Type.EmptyTypes, []);
        provider.Assert(x => x.Verify()).Throws<FakeVerifyException>();

        provider.CallVoid(null, name, Type.EmptyTypes, []);
        provider.Verify();

        provider.CallVoid(null, name, Type.EmptyTypes, []);
        provider.Assert(x => x.Verify()).Throws<FakeVerifyException>();
    }

    [Theory, RandomData]
    internal static void Verify_CustomOutOfRangeThrows(string name)
    {
        FakeMetaProvider provider = new(0, Tools.Faker.Options) { ThrowByDefault = false };

        CallData data = new(name, Type.EmptyTypes, [], Tools.Faker.Options);

        provider.Verify(0, data);
        provider.Assert(x => x.Verify(1, data)).Throws<FakeVerifyException>();

        provider.CallVoid(null, name.Tools().Variant(), Type.EmptyTypes, []);
        provider.Verify(0, data);
        provider.Assert(x => x.Verify(1, data)).Throws<FakeVerifyException>();

        provider.CallVoid(null, name, Type.EmptyTypes, []);
        provider.Assert(x => x.Verify(0, data)).Throws<FakeVerifyException>();
        provider.Verify(1, data);

        provider.CallVoid(null, name, Type.EmptyTypes, []);
        provider.Assert(x => x.Verify(1, data)).Throws<FakeVerifyException>();
        provider.Verify(2, data);
    }

    [Theory, RandomData]
    internal static void VerifyTotalCalls_OutOfRangeThrows(string name)
    {
        FakeMetaProvider provider = new(0, Tools.Faker.Options) { ThrowByDefault = false };

        provider.VerifyTotalCalls(0);
        provider.Assert(x => x.VerifyTotalCalls(1)).Throws<FakeVerifyException>();

        provider.CallVoid(null, name, Type.EmptyTypes, []);
        provider.Assert(x => x.VerifyTotalCalls(0)).Throws<FakeVerifyException>();
        provider.VerifyTotalCalls(1);

        provider.CallVoid(null, name.Tools().Variant(), Type.EmptyTypes, []);
        provider.Assert(x => x.VerifyTotalCalls(1)).Throws<FakeVerifyException>();
        provider.VerifyTotalCalls(2);
    }

    [Theory, RandomData]
    internal static void CallVoid_ReturnValueThrows(string name)
    {
        FakeMetaProvider provider = new(0, Tools.Faker.Options);

        CallData data = new(name, Type.EmptyTypes, [], Tools.Faker.Options);
        provider.SetCallBehavior(data, Behavior.Returns(""));

        provider
            .Assert(x => x.CallVoid(null, name, Type.EmptyTypes, []))
            .Throws<InvalidOperationException>();
    }

    [Theory, RandomData]
    internal static void SetLastCallBehavior_RequiresPreviousCall(Behavior behavior)
    {
        behavior
            .Assert(x =>
            {
                FakeMetaProvider.SetLastCallBehavior(x);
                FakeMetaProvider.SetLastCallBehavior(x);
            })
            .Throws<InvalidOperationException>();
    }
}
