using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Proxy;

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
    internal static void Verify_PresetOutOfRangeThrows(
        MethodInfo method,
        [Unique] MethodInfo otherMethod
    )
    {
        FakeMetaProvider provider = new(0, Tools.Faker.Options) { ThrowByDefault = false };

        CallData data = new(method.Name, Type.EmptyTypes, [], Tools.Faker.Options);

        provider.SetCallBehavior(data, Behavior.None(Times.Once));
        provider.Assert(x => x.Verify()).Throws<FakeVerifyException>();

        provider.CallVoid(null, otherMethod, Type.EmptyTypes, []);
        provider.Assert(x => x.Verify()).Throws<FakeVerifyException>();

        provider.CallVoid(null, method, Type.EmptyTypes, []);
        provider.Verify();

        provider.CallVoid(null, method, Type.EmptyTypes, []);
        provider.Assert(x => x.Verify()).Throws<FakeVerifyException>();
    }

    [Theory, RandomData]
    internal static void Verify_CustomOutOfRangeThrows(
        MethodInfo method,
        [Unique] MethodInfo otherMethod
    )
    {
        FakeMetaProvider provider = new(0, Tools.Faker.Options) { ThrowByDefault = false };

        CallData data = new(method.Name, Type.EmptyTypes, [], Tools.Faker.Options);

        provider.Verify(0, data);
        provider.Assert(x => x.Verify(1, data)).Throws<FakeVerifyException>();

        provider.CallVoid(null, otherMethod, Type.EmptyTypes, []);
        provider.Verify(0, data);
        provider.Assert(x => x.Verify(1, data)).Throws<FakeVerifyException>();

        provider.CallVoid(null, method, Type.EmptyTypes, []);
        provider.Assert(x => x.Verify(0, data)).Throws<FakeVerifyException>();
        provider.Verify(1, data);

        provider.CallVoid(null, method, Type.EmptyTypes, []);
        provider.Assert(x => x.Verify(1, data)).Throws<FakeVerifyException>();
        provider.Verify(2, data);
    }

    [Theory, RandomData]
    internal static void VerifyTotalCalls_OutOfRangeThrows(
        MethodInfo method,
        [Unique] MethodInfo otherMethod
    )
    {
        FakeMetaProvider provider = new(0, Tools.Faker.Options) { ThrowByDefault = false };

        provider.VerifyTotalCalls(0);
        provider.Assert(x => x.VerifyTotalCalls(1)).Throws<FakeVerifyException>();

        provider.CallVoid(null, method, Type.EmptyTypes, []);
        provider.Assert(x => x.VerifyTotalCalls(0)).Throws<FakeVerifyException>();
        provider.VerifyTotalCalls(1);

        provider.CallVoid(null, otherMethod, Type.EmptyTypes, []);
        provider.Assert(x => x.VerifyTotalCalls(1)).Throws<FakeVerifyException>();
        provider.VerifyTotalCalls(2);
    }

    [Theory, RandomData]
    internal static void CallVoid_ReturnValueThrows(MethodInfo method)
    {
        FakeMetaProvider provider = new(0, Tools.Faker.Options);

        CallData data = new(method.Name, Type.EmptyTypes, [], Tools.Faker.Options);
        provider.SetCallBehavior(data, Behavior.Returns(""));

        provider
            .Assert(x => x.CallVoid(null, method, Type.EmptyTypes, []))
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
