using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.TesterTool;
using Werecodent.CreateAndFake.ValuerTool;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool;

/// <summary>Handles testing compare hints.</summary>
/// <typeparam name="T">Compare hint to test.</typeparam>
/// <param name="validTypes">Types that can be compared by the hint.</param>
/// <param name="invalidTypes">Types that can't be compared by the hint.</param>
public abstract class CompareHintTestBase<T>(
    IEnumerable<Type> validTypes = null,
    IEnumerable<Type> invalidTypes = null
)
    where T : CompareHint, new()
{
    /// <summary>Configuration to use for automatic tests.</summary>
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(InvalidCastException),
                typeof(UnsupportedException),
                typeof(ToolException),
                typeof(EngineException),
                typeof(TargetException),
                typeof(InvalidOperationException),
                typeof(TargetParameterCountException),
                typeof(ArgumentException),
                typeof(FakeCallException),
                typeof(TimeoutException),
                typeof(KeyNotFoundException),
            ],
        };

    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = new T();

    /// <summary>Types that can be compared by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? new T().SupportedTypes;

    /// <summary>Types that can't be compared by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes ?? [typeof(DataHolderSample)];

    /// <summary>If sync calls are not supported for the hint.</summary>
    protected virtual bool RequiresAsync { get; } = false;

    /// <inheritdoc cref="ITester.PreventsNullRefExceptionAsync"/>
    [Fact]
    public Task CompareHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            TestInstance,
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutationAsync"/>
    [Fact]
    public virtual Task CompareHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            TestInstance,
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public async Task TryToAsyncCompare_SupportsSameValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object data = Tools.Randomizer.Create(type);
            try
            {
                DifferenceHintAsyncResult result = TestInstance.TryToAsyncCompare(
                    data,
                    data,
                    CreateChainer(),
                    TestContext.Current.CancellationToken
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
    public virtual async Task TryToAsyncCompare_SupportsDifferentValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object one = null,
                two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                await Limiter.Hundred.StallUntilAsync(
                    $"Variant of same type: {GenericConverter.ExpandName(type)}, "
                        + $"Existing type: {GenericConverter.ExpandName(one)}.",
                    () => two = Tools.Mutator.Variant(type, one),
                    () => two.GetType() == one.GetType(),
                    TestContext.Current.CancellationToken
                );

                DifferenceHintAsyncResult result = TestInstance.TryToAsyncCompare(
                    one,
                    two,
                    CreateChainer(),
                    TestContext.Current.CancellationToken
                );

                result
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                await result
                    .Data.Assert()
                    .HasCountMoreThanAsync(
                        0,
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
    public async Task TryToAsyncCompare_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            object one = null,
                two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                two = Tools.Randomizer.Create(
                    GenericConverter.AsMatchedGeneric(type, one.GetType()) ?? type
                );

                await TestInstance
                    .TryToAsyncCompare(
                        one,
                        two,
                        CreateChainer(),
                        TestContext.Current.CancellationToken
                    )
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
    public async Task TryToCompare_SupportsSameValidTypes()
    {
        if (RequiresAsync)
        {
            return;
        }

        foreach (Type type in _validTypes)
        {
            object data = Tools.Randomizer.Create(type);
            try
            {
                DifferenceHintResult result = TestInstance.TryToCompare(
                    data,
                    data,
                    CreateChainer()
                );

                result
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                result
                    .Data.Assert()
                    .IsEmpty(
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
    public virtual async Task TryToCompare_SupportsDifferentValidTypes()
    {
        if (RequiresAsync)
        {
            return;
        }

        foreach (Type type in _validTypes)
        {
            object one = null,
                two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                await Limiter.Hundred.StallUntilAsync(
                    $"Variant of same type: {GenericConverter.ExpandName(type)}, "
                        + $"Existing type: {GenericConverter.ExpandName(one)}.",
                    () => two = Tools.Mutator.Variant(type, one),
                    () => two.GetType() == one.GetType(),
                    TestContext.Current.CancellationToken
                );

                DifferenceHintResult result = TestInstance.TryToCompare(one, two, CreateChainer());

                result
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                result
                    .Data.Assert()
                    .HasCountMoreThan(
                        0,
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
    public async Task TryToCompare_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            object one = null,
                two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                two = Tools.Randomizer.Create(
                    GenericConverter.AsMatchedGeneric(type, one.GetType()) ?? type
                );

                await TestInstance
                    .TryToCompare(one, two, CreateChainer())
                    .Assert()
                    .IsAsync(
                        DifferenceHintResult.None,
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

    /// <summary>Verifies the hint skips or throws if async is required.</summary>
    [Fact]
    public async Task TryToCompare_SyncForbiddenOrSkipped()
    {
        if (!RequiresAsync)
        {
            return;
        }

        foreach (Type type in _validTypes)
        {
            object one = null,
                two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                await Limiter.Hundred.StallUntilAsync(
                    $"Variant of same type: {GenericConverter.ExpandName(type)}, "
                        + $"Existing type: {GenericConverter.ExpandName(one)}.",
                    () => two = Tools.Mutator.Variant(type, one),
                    () => two.GetType() == one.GetType(),
                    TestContext.Current.CancellationToken
                );

                TestInstance
                    .Assert(x =>
                        x.TryToCompare(
                            one,
                            two,
                            CreateChainer(Tools.Valuer.Options with { SkipAsyncValues = false })
                        )
                    )
                    .Throws<EngineException>();

                DifferenceHintResult result = TestInstance.TryToCompare(
                    one,
                    two,
                    CreateChainer(Tools.Valuer.Options with { SkipAsyncValues = true })
                );

                result
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                result
                    .Data.Assert()
                    .IsEmpty(
                        $"Hint requiring async '{typeof(T).Name}' found differences with two random '{type.Name}'."
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
    public async Task TryToAsyncGetHashCode_SupportsSameValidTypes()
    {
        CancellationToken ct = TestContext.Current.CancellationToken;
        foreach (Type type in _validTypes)
        {
            object data = null,
                dataCopy = null;
            try
            {
                data = Tools.Randomizer.Create(type);
                dataCopy = Tools.Duplicator.Copy(data);

                HashCodeHintAsyncResult dataHash = TestInstance.TryToAsyncGetHashCode(
                    data,
                    CreateChainer(),
                    ct
                );
                dataHash
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                await TestInstance
                    .TryToAsyncGetHashCode(data, CreateChainer(), ct)
                    .Assert()
                    .IsAsync(
                        dataHash,
                        ct,
                        $"Hint '{typeof(T).Name}' generated different hash for same '{type.Name}'."
                    );
                await TestInstance
                    .TryToAsyncGetHashCode(dataCopy, CreateChainer(), ct)
                    .Assert()
                    .IsAsync(
                        dataHash,
                        ct,
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
    /// <exception cref="EngineException">When an exception is encountered.</exception>
    [Fact]
    public virtual async Task TryToAsyncGetHashCode_SupportsDifferentValidTypes()
    {
        CancellationToken ct = TestContext.Current.CancellationToken;
        foreach (Type type in _validTypes)
        {
            object data = null,
                dataDiffer = null;
            try
            {
                data = Tools.Randomizer.Create(type);
                dataDiffer = await Tools.Mutator.VariantAsync(
                    type,
                    data,
                    TestContext.Current.CancellationToken
                );

                HashCodeHintAsyncResult dataHash = TestInstance.TryToAsyncGetHashCode(
                    data,
                    CreateChainer(),
                    ct
                );
                dataHash
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");

                await Limiter.Few.AttemptAsync(
                    "Trying to generate object with different value hashes.",
                    () =>
                        TestInstance
                            .TryToAsyncGetHashCode(dataDiffer, CreateChainer(), ct)
                            .Assert()
                            .IsNotAsync(
                                dataHash,
                                ct,
                                $"Hint '{typeof(T).Name}' generated same hash for different '{type.Name}'."
                            ),
                    TestContext.Current.CancellationToken
                );
            }
            catch (Exception e)
            {
                throw new EngineException(
                    $"Error while testing type {GenericConverter.ExpandName(type)}",
                    e
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
    public async Task TryToAsyncGetHashCode_InvalidTypesFail()
    {
        CancellationToken ct = TestContext.Current.CancellationToken;
        foreach (Type type in _invalidTypes)
        {
            object data = Tools.Randomizer.Create(type);
            try
            {
                await TestInstance
                    .TryToAsyncGetHashCode(data, CreateChainer(), ct)
                    .Assert()
                    .IsAsync(
                        HashCodeHintAsyncResult.None,
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

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public async Task TryToGetHashCode_SupportsSameValidTypes()
    {
        if (RequiresAsync)
        {
            return;
        }

        CancellationToken ct = TestContext.Current.CancellationToken;
        foreach (Type type in _validTypes)
        {
            object data = null,
                dataCopy = null;
            try
            {
                data = Tools.Randomizer.Create(type);
                dataCopy = Tools.Duplicator.Copy(data);

                HashCodeHintResult dataHash = TestInstance.TryToGetHashCode(data, CreateChainer());
                dataHash
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                await TestInstance
                    .TryToGetHashCode(data, CreateChainer())
                    .Assert()
                    .IsAsync(
                        dataHash,
                        ct,
                        $"Hint '{typeof(T).Name}' generated different hash for same '{type.Name}'."
                    );
                await TestInstance
                    .TryToGetHashCode(dataCopy, CreateChainer())
                    .Assert()
                    .IsAsync(
                        dataHash,
                        ct,
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
    /// <exception cref="EngineException">When an exception is encountered.</exception>
    [Fact]
    public virtual async Task TryToGetHashCode_SupportsDifferentValidTypes()
    {
        if (RequiresAsync)
        {
            return;
        }

        CancellationToken ct = TestContext.Current.CancellationToken;
        foreach (Type type in _validTypes)
        {
            object data = null,
                dataDiffer = null;
            try
            {
                data = Tools.Randomizer.Create(type);
                dataDiffer = await Tools.Mutator.VariantAsync(
                    type,
                    data,
                    TestContext.Current.CancellationToken
                );

                HashCodeHintResult dataHash = TestInstance.TryToGetHashCode(data, CreateChainer());
                dataHash
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");

                await Limiter.Few.AttemptAsync(
                    "Trying to generate object with different value hashes.",
                    () =>
                        TestInstance
                            .TryToGetHashCode(dataDiffer, CreateChainer())
                            .Assert()
                            .IsNotAsync(
                                dataHash,
                                ct,
                                $"Hint '{typeof(T).Name}' generated same hash for different '{type.Name}'."
                            ),
                    TestContext.Current.CancellationToken
                );
            }
            catch (Exception e)
            {
                throw new EngineException(
                    $"Error while testing type {GenericConverter.ExpandName(type)}",
                    e
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
    public async Task TryToGetHashCode_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            object data = Tools.Randomizer.Create(type);
            try
            {
                await TestInstance
                    .TryToGetHashCode(data, CreateChainer())
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

    /// <summary>Verifies the hint skips or throws if async is required.</summary>
    [Fact]
    public async Task TryToGetHashCode_SyncForbiddenOrSkipped()
    {
        if (!RequiresAsync)
        {
            return;
        }

        foreach (Type type in _validTypes)
        {
            object data = null;
            try
            {
                data = Tools.Randomizer.Create(type);

                TestInstance
                    .Assert(x =>
                        x.TryToGetHashCode(
                            data,
                            CreateChainer(Tools.Valuer.Options with { SkipAsyncValues = false })
                        )
                    )
                    .Throws<EngineException>();

                HashCodeHintResult dataHash = TestInstance.TryToGetHashCode(
                    data,
                    CreateChainer(Tools.Valuer.Options with { SkipAsyncValues = true })
                );
                dataHash
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                dataHash
                    .Data.Assert()
                    .Is(0, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
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
        return new ValuerChainer(options ?? Tools.Valuer.Options, new ValuerEngine());
    }
}
