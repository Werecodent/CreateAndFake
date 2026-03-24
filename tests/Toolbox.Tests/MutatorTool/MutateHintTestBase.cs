using CreateAndFake.Design.Types;
using CreateAndFake.MutatorTool;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.TesterTool;

namespace CreateAndFake.Tests.MutatorTool;

/// <summary>Handles testing <see cref="IMutateHint"/>s.</summary>
/// <typeparam name="T">Hint <see cref="Type"/> to test.</typeparam>
/// <param name="validTypes"><inheritdoc cref="_validTypes" path="/summary"/></param>
/// <param name="invalidTypes"><inheritdoc cref="_invalidTypes" path="/summary"/></param>
public abstract class MutateHintTestBase<T>(
    IEnumerable<Type> validTypes = null,
    IEnumerable<Type> invalidTypes = null
)
    where T : IMutateHint, new()
{
    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = new T();

    /// <summary>Types that are supported by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? new T().SupportedTypes;

    /// <summary>Types that aren't supported by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes ?? [typeof(DataHolderSample)];

    /// <inheritdoc cref="ITester.PreventsNullRefExceptionAsync"/>
    [Fact]
    public Task MutateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<T>(TestContext.Current.CancellationToken);
    }

    /// <summary>Ensure the hint supports all provided valid types.</summary>
    [Fact]
    public void TryModifying_ValidTypesSupported()
    {
        foreach (Type type in _validTypes)
        {
            object data = type.CreateRandomInstance();
            TestInstance
                .TryModifying(data, CreateChainer())
                .HasData.Assert()
                .Is(
                    true,
                    $"Hint '{GenericTypeConverter.ExpandedName<T>()}' failed to modify type "
                        + $"'{GenericTypeConverter.ExpandedName(type)}'. "
                        + $"Actual type: '{GenericTypeConverter.ExpandedName(data)}'."
                );
        }
    }

    /// <summary>Ensure the hint does not support any invalid types.</summary>
    [Fact]
    public void TryModifying_InvalidTypesNotSupported()
    {
        foreach (Type type in _invalidTypes)
        {
            object data = type.CreateRandomInstance();
            TestInstance
                .TryModifying(data, CreateChainer())
                .HasData.Assert()
                .Is(
                    false,
                    $"Hint '{GenericTypeConverter.ExpandedName<T>()}' modified type "
                        + $"'{GenericTypeConverter.ExpandedName(type)}'. "
                        + $"Actual type: '{GenericTypeConverter.ExpandedName(data)}'."
                );
        }
    }

    /// <summary>Ensure priority is within range.</summary>
    [Fact]
    public void EnginePriority_Constrained()
    {
        TestInstance
            .EnginePriority.Assert()
            .GreaterThan((int)MutatePriority.None)
            .And.LessThan((int)MutatePriority.Highest);
    }

    /// <summary>Ensure expanded type name is used.</summary>
    [Fact]
    public void ToString_Overridden()
    {
        TestInstance.ToString().Assert().Is(GenericTypeConverter.ExpandedName(TestInstance));
    }

    /// <typeparam name="TData">The <see cref="Type"/> to test.</typeparam>
    /// <inheritdoc cref="RunModifyTest"/>
    protected void RunModifyTest<TData>(bool shouldMutate, int? collectionSize = null)
    {
        RunModifyTest(typeof(TData), shouldMutate, collectionSize);
    }

    /// <summary>Tests that attempted mutation works as intended.</summary>
    /// <param name="type">The <see cref="Type"/> to test.</param>
    /// <param name="shouldMutate">If the data should be mutated.</param>
    /// <param name="collectionSize">Exact collection size to use if set.</param>
    protected void RunModifyTest(Type type, bool shouldMutate, int? collectionSize = null)
    {
        object data =
            (collectionSize == null)
                ? Tools.Randomizer.Create(type)
                : Tools.Randomizer.Create(
                    type,
                    opt =>
                        opt with
                        {
                            CollectionMinSize = collectionSize.Value,
                            CollectionMaxSize = collectionSize.Value,
                        }
                );
        object original = data.CreateDeepClone();

        TestInstance
            .TryModifying(data, CreateChainer())
            .Assert()
            .Is(new MutateHintResult(shouldMutate));

        if (shouldMutate)
        {
            data.Assert().IsNot(original);
        }
        else
        {
            data.Assert().Is(original);
        }
    }

    /// <summary>Create a chainer to use for testing.</summary>
    /// <returns>The created chainer.</returns>
    /// <param name="options">Options to pass via the chainer.</param>
    protected static IMutatorChainer CreateChainer(MutatorOptions options = null)
    {
        return new MutatorChainer(options ?? Tools.Mutator.Options, new MutatorEngine());
    }
}
