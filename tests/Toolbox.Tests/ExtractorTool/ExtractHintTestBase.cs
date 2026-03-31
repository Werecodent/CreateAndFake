using CreateAndFake.Design.Content;
using CreateAndFake.ExtractorTool;
using CreateAndFake.ExtractorTool.Engine;
using CreateAndFake.TesterTool;

namespace CreateAndFake.Tests.ExtractorTool;

/// <summary>Handles testing extract hints.</summary>
/// <typeparam name="T">Extract hint to test.</typeparam>
/// <param name="testInstance">Instance to test with.</param>
/// <param name="validTypes">Types that can be created by the hint.</param>
/// <param name="invalidTypes">Types that can't be created by the hint.</param>
public abstract class ExtractHintTestBase<T>(
    T testInstance,
    IEnumerable<Type> validTypes,
    IEnumerable<Type> invalidTypes
)
    where T : IExtractHint
{
    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = testInstance;

    /// <summary>Types that can be created by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? Type.EmptyTypes;

    /// <summary>Types that can't be created by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes ?? Type.EmptyTypes;

    /// <inheritdoc cref="ITester.PreventsNullRefExceptionAsync"/>
    [Fact]
    public Task ExtractHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            TestInstance,
            TestContext.Current.CancellationToken
        );
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public async Task TryExtract_SupportsValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object value = Tools.Randomizer.Create(type);
            ExtractHintResult result = TestInstance.TryExtract(value, CreateChainer());
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
            }
            finally
            {
                await Disposer.CleanupAsync(value, result.Data);
            }
        }
    }

    /// <summary>Verifies the hint doesn't support the wrong types.</summary>
    [Fact]
    public async Task TryExtract_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            object value = Tools.Randomizer.Create(type);
            try
            {
                await TestInstance
                    .TryExtract(value, CreateChainer())
                    .Assert()
                    .IsAsync(
                        ExtractHintResult.None,
                        TestContext.Current.CancellationToken,
                        "Hint '" + typeof(T).Name + "' should not support type '" + type.Name + "'."
                    );
            }
            finally
            {
                await Disposer.CleanupAsync(value);
            }
        }
    }

    /// <summary>Create a chainer to use for testing.</summary>
    /// <returns>Chainer to use for testing.</returns>
    /// <param name="options">Options to pass via the chainer.</param>
    protected static IExtractorChainer CreateChainer(ExtractorOptions options = null)
    {
        return new ExtractorChainer(options ?? Tools.Extractor.Options, new ExtractorEngine());
    }
}
