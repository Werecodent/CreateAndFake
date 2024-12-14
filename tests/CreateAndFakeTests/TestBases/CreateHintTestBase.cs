using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.RandomizerTool;

namespace CreateAndFakeTests.TestBases;

/// <summary>Handles testing create hints.</summary>
/// <typeparam name="T">Create hint to test.</typeparam>
/// <param name="testInstance">Instance to test with.</param>
/// <param name="validTypes">Types that can be created by the hint.</param>
/// <param name="invalidTypes">Types that can't be created by the hint.</param>
public abstract class CreateHintTestBase<T>(
    T testInstance,
    IEnumerable<Type> validTypes,
    IEnumerable<Type> invalidTypes) where T : CreateHint
{
    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = testInstance;

    /// <summary>Types that can be created by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? Type.EmptyTypes;

    /// <summary>Types that can't be created by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes ?? Type.EmptyTypes;

    /// <summary>Verifies null reference exceptions are prevented.</summary>
    [Fact]
    public void CreateHint_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(
            TestInstance, opt => opt with { InjectionValues = [Tools.Randomizer.Options] });
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public void TryCreate_SupportsValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            (bool, object) result = TestInstance.TryCreate(type, CreateChainer());
            try
            {
                result.Item1.Assert().Is(true,
                    "Hint '" + typeof(T).Name + "' did not support type '" + type.Name + "'.");
                result.Item2.Assert().IsNot(null,
                    "Hint '" + typeof(T).Name + "' did not create valid '" + type.Name + "'.");

                if (result.Item2 is IEnumerable collection)
                {
                    collection.GetEnumerator().MoveNext().Assert().Is(true,
                        "Hint '" + typeof(T).Name + "' failed to create populated '" + type + "'.");
                }
            }
            finally
            {
                Disposer.Cleanup(result.Item2);
            }
        }
    }

    /// <summary>Verifies the hint doesn't support the wrong types.</summary>
    [Fact]
    public void TryCreate_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            TestInstance.TryCreate(type, CreateChainer()).Assert().Is((false, (object)null),
                "Hint '" + typeof(T).Name + "' should not support type '" + type.Name + "'.");
        }
    }

    /// <returns>Chainer to use for testing.</returns>
    protected static RandomizerChainer CreateChainer()
    {
        return CreateChainer(Tools.Randomizer.Options);
    }

    /// <returns>Chainer to use for testing.</returns>
    /// <param name="options">Options to pass via the chainer.</param>
    protected static RandomizerChainer CreateChainer(RandomizerOptions options)
    {
        return new RandomizerChainer(options, (t, c) => Tools.Randomizer.Create(t));
    }
}