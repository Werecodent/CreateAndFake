using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool;

/// <summary>Handles testing compare hints.</summary>
/// <typeparam name="T">Compare hint to test.</typeparam>
/// <param name="testInstance">Instance to test with.</param>
/// <param name="validTypes">Types that can be compared by the hint.</param>
/// <param name="invalidTypes">Types that can't be compared by the hint.</param>
public abstract class CompareHintTestBase<T>(
    T testInstance,
    IEnumerable<Type> validTypes,
    IEnumerable<Type> invalidTypes
)
    where T : CompareHint
{
    /// <summary>Configuration to use for automatic tests.</summary>
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(InvalidCastException),
                typeof(NotSupportedException),
                typeof(ToolException),
                typeof(TargetException),
                typeof(InvalidOperationException),
                typeof(TargetParameterCountException),
                typeof(InsufficientExecutionStackException),
                typeof(ArgumentException),
                typeof(FakeCallException),
                typeof(TimeoutException),
            ],
        };

    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = testInstance;

    /// <summary>Types that can be compared by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes;

    /// <summary>Types that can't be compared by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes;

    /// <inheritdoc cref="ITester.PreventsNullRefException"/>
    [Fact]
    public Task CompareHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            TestInstance,
            TestContext.Current.CancellationToken,
            config
        );
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutation"/>
    [Fact]
    public Task CompareHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            TestInstance,
            TestContext.Current.CancellationToken,
            config
        );
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public async Task TryCompare_SupportsSameValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object data = Tools.Randomizer.Create(type);
            try
            {
                DifferenceHintAsyncResult result = TestInstance.TryAsyncCompare(
                    data,
                    data,
                    CreateChainer()
                );

                result
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                await result
                    .Data.Assert()
                    .IsEmptyAsync(
                        TestContext.Current.CancellationToken,
                        $"Hint '{typeof(T).Name}' found differences with same '{type.Name}' of '{data.GetType()}'."
                    );
            }
            finally
            {
                await Disposer.CleanupAsync(data);
            }
        }
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public virtual async Task TryCompare_SupportsDifferentValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object one = null,
                two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                two = Tools.Mutator.Variant(one.GetType(), one);

                DifferenceHintAsyncResult result = TestInstance.TryAsyncCompare(
                    one,
                    two,
                    CreateChainer()
                );

                result
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                await result
                    .Data.Assert()
                    .IsNotEmptyAsync(
                        TestContext.Current.CancellationToken,
                        $"Hint '{typeof(T).Name}' didn't find differences with two random '{type.Name}'."
                    );
            }
            finally
            {
                await Disposer.CleanupAsync(one, two);
            }
        }
    }

    /// <summary>Verifies the hint doesn't support the wrong types.</summary>
    [Fact]
    public async Task TryCompare_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            object one = null,
                two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                two = Tools.Randomizer.Create(one.GetType());

                await TestInstance
                    .TryAsyncCompare(one, two, CreateChainer())
                    .Assert()
                    .IsAsync(
                        DifferenceHintAsyncResult.None,
                        TestContext.Current.CancellationToken,
                        $"Hint '{typeof(T).Name}' should not support type '{type.Name}'."
                    );
            }
            finally
            {
                await Disposer.CleanupAsync(one, two);
            }
        }
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public async Task TryGetHashCode_SupportsSameValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object data = null,
                dataCopy = null;
            try
            {
                data = Tools.Randomizer.Create(type);
                dataCopy = Tools.Duplicator.Copy(data);

                HashCodeHintResult dataHash = TestInstance.TryGetHashCode(data, CreateChainer());
                dataHash
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                await TestInstance
                    .TryGetHashCode(data, CreateChainer())
                    .Assert()
                    .IsAsync(
                        dataHash,
                        TestContext.Current.CancellationToken,
                        $"Hint '{typeof(T).Name}' generated different hash for same '{type.Name}'."
                    );
                await TestInstance
                    .TryGetHashCode(dataCopy, CreateChainer())
                    .Assert()
                    .IsAsync(
                        dataHash,
                        TestContext.Current.CancellationToken,
                        $"Hint '{typeof(T).Name}' generated different hash for dupe '{type.Name}'."
                    );
            }
            finally
            {
                await Disposer.CleanupAsync(data, dataCopy);
            }
        }
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public async Task TryGetHashCode_SupportsDifferentValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object data = null,
                dataDiffer = null;
            try
            {
                data = Tools.Randomizer.Create(type);
                dataDiffer = Tools.Mutator.Variant(data);

                HashCodeHintAsyncResult dataHash = TestInstance.TryAsyncGetHashCode(
                    data,
                    CreateChainer(),
                    TestContext.Current.CancellationToken
                );
                dataHash
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                await TestInstance
                    .TryAsyncGetHashCode(
                        dataDiffer,
                        CreateChainer(),
                        TestContext.Current.CancellationToken
                    )
                    .Assert()
                    .IsNotAsync(
                        dataHash,
                        TestContext.Current.CancellationToken,
                        $"Hint '{typeof(T).Name}' generated same hash for different '{type.Name}'."
                    );
            }
            finally
            {
                await Disposer.CleanupAsync(data, dataDiffer);
            }
        }
    }

    /// <summary>Verifies the hint doesn't support the wrong types.</summary>
    [Fact]
    public async Task TryGetHashCode_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            object data = Tools.Randomizer.Create(type);
            try
            {
                await TestInstance
                    .TryGetHashCode(data, CreateChainer())
                    .Assert()
                    .IsAsync(
                        HashCodeHintResult.None,
                        TestContext.Current.CancellationToken,
                        $"Hint '{typeof(T).Name}' should not support type '{type.Name}'."
                    );
            }
            finally
            {
                await Disposer.CleanupAsync(data);
            }
        }
    }

    /// <summary>Create a chainer to use for testing.</summary>
    /// <returns>Chainer to use for testing.</returns>
    /// <param name="options">Options to pass via the chainer.</param>
    protected static IValuerChainer CreateChainer(ValuerOptions options = null)
    {
        return new ValuerChainer(
            options ?? Tools.Valuer.Options,
            new ValuerEngine(Valuer.DefaultHints)
        );
    }
}
