using CreateAndFake.MutatorTool;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.TesterTool;

namespace CreateAndFake.Tests.MutatorTool;

/// <summary>Handles testing <see cref="IMutateHint"/>s.</summary>
/// <typeparam name="T">Hint <see cref="Type"/> to test.</typeparam>
/// <param name="testInstance">Instance to test with.</param>
/// <param name="validTypes"><inheritdoc cref="_validTypes" path="/summary"/></param>
/// <param name="invalidTypes"><inheritdoc cref="_invalidTypes" path="/summary"/></param>
public abstract class CreateHintTestBase<T>(
    T testInstance,
    IEnumerable<Type> validTypes,
    IEnumerable<Type> invalidTypes
)
    where T : IMutateHint
{
    /// <summary>Configuration to use for automatic tests.</summary>
    private static readonly TesterMod config = opt => opt with { IgnorableExceptions = [] };

    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = testInstance;

    /// <summary>Types that are supported by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? Type.EmptyTypes;

    /// <summary>Types that aren't supported by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes ?? Type.EmptyTypes;

    /// <inheritdoc cref="ITester.PreventsNullRefException"/>
    [Fact]
    public Task MutateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            TestInstance,
            TestContext.Current.CancellationToken,
            config
        );
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutation"/>
    [Fact]
    public Task MutateHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            TestInstance,
            TestContext.Current.CancellationToken,
            opt => config(opt) with { InjectionValues = [CreateChainer()] }
        );
    }

    /// <summary>Create a chainer to use for testing.</summary>
    /// <returns>The created chainer.</returns>
    /// <param name="options">Options to pass via the chainer.</param>
    protected static IMutatorChainer CreateChainer(MutatorOptions options = null)
    {
        return new MutatorChainer(options ?? Tools.Mutator.Options, new MutatorEngine());
    }
}
