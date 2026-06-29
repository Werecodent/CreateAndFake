using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class RandomizerChainerTests
{
    /*
    System.ArgumentOutOfRangeException : length ('-1803443842') must be a non-negative value. (Parameter 'length')
    Actual value was -1803443842.
    at System.ArgumentOutOfRangeException.ThrowNegative[T](T value, String paramName)
    at System.String.Ctor(Char[] value, Int32 startIndex, Int32 length)
    at System.RuntimeMethodHandle.InvokeMethod(Object target, Void** arguments, Signature sig, Boolean isConstructor)
    at System.Reflection.MethodBaseInvoker.InvokeDirectByRefWithFewArgs(Object obj, Span`1 copyOfArgs, BindingFlags invokeAttr)

    [Fact]
    internal static Task RandomizerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<RandomizerChainer>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ArgumentOutOfRangeException)] }
        );
    }

    [Fact]
    internal static Task RandomizerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<RandomizerChainer>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentOutOfRangeException),
                        typeof(ArgumentException),
                    ],
                }
        );
    }*/

    [Fact]
    internal static Task RandomizerChainer_PassthroughWithNoExceptions()
    {
        return Tools.Tester.PassthroughWithNoExceptionsAsync<RandomizerChainer>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidCastException)] }
        );
    }

    [Theory, RandomData]
    internal static void CreateSpecific_ThrowsWhenRecreatingSameType(
        [Stub] IRandomizerEngine engine,
        Type specificType,
        Type parentType
    )
    {
        engine
            .Create(specificType, Arg.Any<IRandomizerChainer>())
            .SetupCall(
                Behavior.Set<Type, IRandomizerChainer, object>(
                    (_, chainer) => chainer.CreateSpecific(specificType, parentType)
                )
            );

        new RandomizerChainer(Tools.Randomizer.Options, engine)
            .Assert(c => c.CreateSpecific(specificType, parentType))
            .Throws<EngineException>();
    }

    [Theory, RandomData]
    internal static void CreateInternal_ThrowsWhenSameTypeRecreated(
        [Stub] IRandomizerEngine engine,
        object data
    )
    {
        Type type = Tools.Mutator.Variant(data.GetType());

        engine
            .Create(type, Arg.Any<IRandomizerChainer>())
            .SetupCall(
                Behavior.Set<Type, IRandomizerChainer, object>(
                    (_, chainer) => chainer.CreateInternal(type, data)
                )
            );

        new RandomizerChainer(Tools.Randomizer.Options, engine)
            .Assert(c => c.CreateInternal(type, data))
            .Throws<EngineException>();
    }

    [Theory, RandomData]
    internal static void CreateInternal_CreatesParentLoop(
        [Stub] IRandomizerEngine engine,
        object parentData,
        object childData
    )
    {
        Type type = Tools.Mutator.VariantOf([parentData.GetType(), childData.GetType()]);

        engine
            .Create(type, Arg.Any<IRandomizerChainer>())
            .SetupCall(
                Behavior.Set<Type, IRandomizerChainer, object>(
                    (_, chainer) => chainer.CreateInternal(parentData.GetType(), childData)
                )
            );

        new RandomizerChainer(Tools.Randomizer.Options, engine)
            .CreateInternal(type, parentData)
            .Assert()
            .Is(parentData);
    }

    [Theory, RandomData]
    internal static void CreateInternal_CreatesSelfLoop(
        [Stub] IRandomizerEngine engine,
        object data
    )
    {
        new RandomizerChainer(Tools.Randomizer.Options, engine)
            .CreateInternal(data.GetType(), data)
            .Assert()
            .Is(data);
    }
}
