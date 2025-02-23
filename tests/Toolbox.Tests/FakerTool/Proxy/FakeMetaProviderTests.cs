using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.Tests.FakerTool.Proxy;

public static class FakeMetaProviderTests
{
    [Fact]
    internal static void FakeMetaProvider_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<FakeMetaProvider>();
    }

    [Fact]
    internal static void FakeMetaProvider_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<FakeMetaProvider>();
    }

    [Theory, RandomData]
    internal static void Verify_PresetOutOfRangeThrows(string name)
    {
        FakeMetaProvider provider = new()
        {
            ThrowByDefault = false
        };

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
        FakeMetaProvider provider = new()
        {
            ThrowByDefault = false
        };

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
        FakeMetaProvider provider = new()
        {
            ThrowByDefault = false
        };

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
        FakeMetaProvider provider = new();

        CallData data = new(name, Type.EmptyTypes, [], Tools.Faker.Options);
        provider.SetCallBehavior(data, Behavior.Returns(""));

        provider.Assert(p => p.CallVoid(null, name, Type.EmptyTypes, [])).Throws<InvalidOperationException>();
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
