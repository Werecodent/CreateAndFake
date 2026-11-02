using System.Collections;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.RandomizerTool;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool;

/// <summary>Handles testing create hints.</summary>
/// <typeparam name="T">Create hint to test.</typeparam>
/// <param name="validTypes">Types that can be created by the hint.</param>
/// <param name="invalidTypes">Types that can't be created by the hint.</param>
public abstract class CreateHintTestBase<T>(
    IEnumerable<Type> validTypes = null,
    IEnumerable<Type> invalidTypes = null
) : AutoTester<T>
    where T : CreateHint, new()
{
    /// <inheritdoc/>
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                InjectionValues = [CreateChainer()],
                IgnorableExceptions = [typeof(InvalidOperationException)],
            };

    /// <summary>Types that can be created by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? new T().SupportedTypes;

    /// <summary>Types that can't be created by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes =
        invalidTypes ?? [typeof(DataHolderSample), typeof(IEnumerable), typeof(IEnumerable<>)];

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
