using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.TesterTool;
using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFakeTests.TestBases;

/// <summary>Handles testing compare hints.</summary>
/// <typeparam name="T">Compare hint to test.</typeparam>
/// <param name="testInstance">Instance to test with.</param>
/// <param name="validTypes">Types that can be compared by the hint.</param>
/// <param name="invalidTypes">Types that can't be compared by the hint.</param>
public abstract class CompareHintTestBase<T>(
    T testInstance,
    IEnumerable<Type> validTypes,
    IEnumerable<Type> invalidTypes) where T : CompareHint
{
    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = testInstance;

    /// <summary>Types that can be compared by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes;

    /// <summary>Types that can't be compared by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes;

    /// <inheritdoc cref="ITester.PreventsNullRefException{T}(Func{TesterOptions,TesterOptions})"/>
    [Fact]
    public void CompareHint_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(TestInstance);
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutation{T}(Func{TesterOptions,TesterOptions})"/>
    [Fact]
    public void CompareHint_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(TestInstance);
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public void TryCompare_SupportsSameValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object data = Tools.Randomizer.Create(type);
            try
            {
                (bool, IEnumerable<Difference>) result = TestInstance.TryCompare(data, data, CreateChainer());

                result.Item1.Assert().Is(true,
                    $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                result.Item2.Assert().IsEmpty(
                    $"Hint '{typeof(T).Name}' found differences with same '{type.Name}' of '{data.GetType()}'.");
            }
            finally
            {
                Disposer.Cleanup(data);
            }
        }
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public virtual void TryCompare_SupportsDifferentValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object one = null, two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                two = Tools.Mutator.Variant(one.GetType(), one);

                (bool, IEnumerable<Difference>) result = TestInstance.TryCompare(one, two, CreateChainer());

                result.Item1.Assert().Is(true,
                    $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                result.Item2.ToArray().Assert().IsNotEmpty(
                    $"Hint '{typeof(T).Name}' didn't find differences with two random '{type.Name}'.");
            }
            finally
            {
                Disposer.Cleanup(one, two);
            }
        }
    }

    /// <summary>Verifies the hint doesn't support the wrong types.</summary>
    [Fact]
    public void TryCompare_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            object one = null, two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                two = Tools.Randomizer.Create(one.GetType());

                TestInstance.TryCompare(one, two, CreateChainer()).Assert().Is((false, (IEnumerable<Difference>)null),
                    $"Hint '{typeof(T).Name}' should not support type '{type.Name}'.");
            }
            finally
            {
                Disposer.Cleanup(one, two);
            }
        }
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public void TryGetHashCode_SupportsSameValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object data = null, dataCopy = null;
            try
            {
                data = Tools.Randomizer.Create(type);
                dataCopy = Tools.Duplicator.Copy(data);

                (bool, int) dataHash = TestInstance.TryGetHashCode(data, CreateChainer());
                dataHash.Item1.Assert().Is(true,
                    $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                TestInstance.TryGetHashCode(data, CreateChainer()).Assert().Is(dataHash,
                    $"Hint '{typeof(T).Name}' generated different hash for same '{type.Name}'.");
                TestInstance.TryGetHashCode(dataCopy, CreateChainer()).Assert().Is(dataHash,
                    $"Hint '{typeof(T).Name}' generated different hash for dupe '{type.Name}'.");
            }
            finally
            {
                Disposer.Cleanup(data, dataCopy);
            }
        }
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public void TryGetHashCode_SupportsDifferentValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object data = null, dataDiffer = null;
            try
            {
                data = Tools.Randomizer.Create(type);
                dataDiffer = Tools.Mutator.Variant(data);

                (bool, int) dataHash = TestInstance.TryGetHashCode(data, CreateChainer());
                dataHash.Item1.Assert().Is(true,
                    $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                TestInstance.TryGetHashCode(dataDiffer, CreateChainer()).Assert().IsNot(dataHash,
                    $"Hint '{typeof(T).Name}' generated same hash for different '{type.Name}'.");
            }
            finally
            {
                Disposer.Cleanup(data, dataDiffer);
            }
        }
    }

    /// <summary>Verifies the hint doesn't support the wrong types.</summary>
    [Fact]
    public void TryGetHashCode_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            object data = Tools.Randomizer.Create(type);
            try
            {
                TestInstance.TryGetHashCode(data, CreateChainer()).Assert().Is((false, default(int)),
                    $"Hint '{typeof(T).Name}' should not support type '{type.Name}'.");
            }
            finally
            {
                Disposer.Cleanup(data);
            }
        }
    }

    /// <returns>Chainer to use for testing.</returns>
    protected static ValuerChainer CreateChainer()
    {
        return new ValuerChainer(
            Tools.Valuer.Options,
            Tools.Valuer,
            (o, c) => Tools.Valuer.GetHashCode(o),
            (e, a, c) => Tools.Valuer.Compare(e, a));
    }
}
