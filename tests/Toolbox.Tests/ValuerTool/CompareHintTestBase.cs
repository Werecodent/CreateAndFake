using CreateAndFake.Design.Content;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;

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
    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = testInstance;

    /// <summary>Types that can be compared by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes;

    /// <summary>Types that can't be compared by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes;

    /// <inheritdoc cref="ITester.PreventsNullRefException"/>
    [Fact]
    public void CompareHint_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(TestInstance);
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutation"/>
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
                DifferenceHintResult result = TestInstance.TryCompare(data, data, CreateChainer());

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
            object one = null,
                two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                two = Tools.Mutator.Variant(one.GetType(), one);

                DifferenceHintResult result = TestInstance.TryCompare(one, two, CreateChainer());

                result
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                result
                    .Data.ToArray()
                    .Assert()
                    .IsNotEmpty(
                        $"Hint '{typeof(T).Name}' didn't find differences with two random '{type.Name}'."
                    );
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
            object one = null,
                two = null;
            try
            {
                one = Tools.Randomizer.Create(type);
                two = Tools.Randomizer.Create(one.GetType());

                TestInstance
                    .TryCompare(one, two, CreateChainer())
                    .Assert()
                    .Is(
                        DifferenceHintResult.None,
                        $"Hint '{typeof(T).Name}' should not support type '{type.Name}'."
                    );
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
                TestInstance
                    .TryGetHashCode(data, CreateChainer())
                    .Assert()
                    .Is(
                        dataHash,
                        $"Hint '{typeof(T).Name}' generated different hash for same '{type.Name}'."
                    );
                TestInstance
                    .TryGetHashCode(dataCopy, CreateChainer())
                    .Assert()
                    .Is(
                        dataHash,
                        $"Hint '{typeof(T).Name}' generated different hash for dupe '{type.Name}'."
                    );
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
            object data = null,
                dataDiffer = null;
            try
            {
                data = Tools.Randomizer.Create(type);
                dataDiffer = Tools.Mutator.Variant(data);

                HashCodeHintResult dataHash = TestInstance.TryGetHashCode(data, CreateChainer());
                dataHash
                    .HasData.Assert()
                    .Is(true, $"Hint '{typeof(T).Name}' failed to support '{type.Name}'.");
                TestInstance
                    .TryGetHashCode(dataDiffer, CreateChainer())
                    .Assert()
                    .IsNot(
                        dataHash,
                        $"Hint '{typeof(T).Name}' generated same hash for different '{type.Name}'."
                    );
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
                TestInstance
                    .TryGetHashCode(data, CreateChainer())
                    .Assert()
                    .Is(
                        HashCodeHintResult.None,
                        $"Hint '{typeof(T).Name}' should not support type '{type.Name}'."
                    );
            }
            finally
            {
                Disposer.Cleanup(data);
            }
        }
    }

    /// <returns>Chainer to use for testing.</returns>
    /// <param name="options">Options to pass via the chainer.</param>
    protected static ValuerChainer CreateChainer(ValuerOptions options = null)
    {
        return new ValuerChainer(
            options ?? Tools.Valuer.Options,
            Tools.Valuer,
            (o, c) => Tools.Valuer.GetHashCode(o),
            (e, a, c) => Tools.Valuer.Compare(e, a)
        );
    }
}
