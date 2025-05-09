using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.RandomizerTool;
using CreateAndFake.TesterTool;

namespace CreateAndFake.Tests.RandomizerTool;

/// <summary>Handles testing create hints.</summary>
/// <typeparam name="T">Create hint to test.</typeparam>
/// <param name="testInstance">Instance to test with.</param>
/// <param name="validTypes">Types that can be created by the hint.</param>
/// <param name="invalidTypes">Types that can't be created by the hint.</param>
public abstract class CreateHintTestBase<T>(
    T testInstance,
    IEnumerable<Type> validTypes,
    IEnumerable<Type> invalidTypes
)
    where T : CreateHint
{
    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = testInstance;

    /// <summary>Types that can be created by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? Type.EmptyTypes;

    /// <summary>Types that can't be created by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes ?? Type.EmptyTypes;

    /// <inheritdoc cref="ITester.PreventsNullRefException"/>
    [Fact]
    public Task CreateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(TestInstance);
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutation"/>
    [Fact]
    public Task CreateHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            TestInstance,
            opt =>
                opt with
                {
                    InjectionValues =
                    [
                        CreateChainer(
                            Tools.Randomizer.Options with
                            {
                                Gen = Tools.Randomizer.Create<FastRandom>(),
                            }
                        ),
                    ],
                }
        );
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public void TryCreate_SupportsValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            CreateHintResult result = TestInstance.TryCreate(type, CreateChainer());
            try
            {
                result
                    .HasData.Assert()
                    .Is(
                        true,
                        "Hint '" + typeof(T).Name + "' did not support type '" + type.Name + "'."
                    );
                result
                    .Data.Assert()
                    .IsNot(
                        null,
                        "Hint '" + typeof(T).Name + "' did not create valid '" + type.Name + "'."
                    );

                if (result.Data is IEnumerable collection)
                {
                    collection
                        .GetEnumerator()
                        .MoveNext()
                        .Assert()
                        .Is(
                            true,
                            "Hint '"
                                + typeof(T).Name
                                + "' failed to create populated '"
                                + type
                                + "'."
                        );
                }
            }
            finally
            {
                Disposer.Cleanup(result.Data);
            }
        }
    }

    /// <summary>Verifies the hint doesn't support the wrong types.</summary>
    [Fact]
    public void TryCreate_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            TestInstance
                .TryCreate(type, CreateChainer())
                .Assert()
                .Is(
                    CreateHintResult.None,
                    "Hint '" + typeof(T).Name + "' should not support type '" + type.Name + "'."
                );
        }
    }

    /// <returns>Chainer to use for testing.</returns>
    /// <param name="options">Options to pass via the chainer.</param>
    protected static RandomizerChainer CreateChainer(RandomizerOptions options = null)
    {
        return new RandomizerChainer(
            options ?? Tools.Randomizer.Options,
            (t, c) => Tools.Randomizer.Create(t)
        );
    }
}
