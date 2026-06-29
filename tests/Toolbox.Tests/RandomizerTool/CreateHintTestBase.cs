using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RandomizerTool.Engine;
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
    /// <summary>Configuration to use for automatic tests.</summary>
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(InvalidOperationException)],
        };

    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = testInstance;

    /// <summary>Types that can be created by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? Type.EmptyTypes;

    /// <summary>Types that can't be created by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes ?? Type.EmptyTypes;

    /// <inheritdoc cref="ITester.PreventsNullRefExceptionAsync"/>
    [Fact]
    public Task CreateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            TestInstance,
            TestContext.Current.CancellationToken,
            config
        );
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutationAsync"/>
    [Fact]
    public Task CreateHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            TestInstance,
            TestContext.Current.CancellationToken,
            opt => config(opt) with { InjectionValues = [CreateChainer()] }
        );
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public async Task TryToCreate_SupportsValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            CreateHintResult result = TestInstance.TryToCreate(type, CreateChainer());
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
                    .IsNotNull(
                        "Hint '" + typeof(T).Name + "' did not create valid '" + type.Name + "'."
                    );

                if (result.Data is IEnumerable collection)
                {
                    collection
                        .Assert()
                        .IsNotEmpty(
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
                await Disposer.CleanupAsync(result.Data);
            }
        }
    }

    /// <summary>Verifies the hint doesn't support the wrong types.</summary>
    [Fact]
    public void TryToCreate_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            TestInstance
                .TryToCreate(type, CreateChainer())
                .Assert()
                .Is(
                    CreateHintResult.None,
                    "Hint '" + typeof(T).Name + "' should not support type '" + type.Name + "'."
                );
        }
    }

    /// <summary>Create a chainer to use for testing.</summary>
    /// <returns>Chainer to use for testing.</returns>
    /// <param name="options">Options to pass via the chainer.</param>
    protected static IRandomizerChainer CreateChainer(RandomizerOptions options = null)
    {
        return new RandomizerChainer(options ?? Tools.Randomizer.Options, new RandomizerEngine());
    }
}
