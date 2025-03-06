using CreateAndFake.Design.Content;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.TesterTool;

namespace CreateAndFake.Tests.DuplicatorTool;

/// <summary>Handles testing copy hints.</summary>
/// <typeparam name="T">Copy hint to test.</typeparam>
/// <param name="validTypes">Types that can be copied by the hint.</param>
/// <param name="invalidTypes">Types that can't be copied by the hint.</param>
/// <param name="copiesByRef">If the hint copies by reference instead for value types.</param>
public abstract class CopyHintTestBase<T>(
    IEnumerable<Type> validTypes,
    IEnumerable<Type> invalidTypes,
    bool copiesByRef = false
)
    where T : CopyHint, new()
{
    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = new T();

    /// <summary>Types that can be copied by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? Type.EmptyTypes;

    /// <summary>Types that can't be copied by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes ?? Type.EmptyTypes;

    /// <summary>If the hint copies by reference instead for value types.</summary>
    private readonly bool _copiesByRef = copiesByRef;

    /// <inheritdoc cref="ITester.PreventsNullRefException"/>
    [Fact]
    public void CopyHint_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(TestInstance);
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutation"/>
    [Fact]
    public void CopyHint_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(TestInstance);
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public void TryCopy_SupportsValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object data = null;
            CopyHintResult result = CopyHintResult.None;
            try
            {
                data = Tools.Randomizer.Create(type);
                result = TestInstance.TryCopy(data, CreateChainer());

                result
                    .Assert()
                    .Is(
                        new CopyHintResult(data),
                        "Hint '" + typeof(T).Name + "' failed to clone type '" + type.Name + "'."
                    );

                if (_copiesByRef || data is string)
                {
                    result
                        .Data.Assert()
                        .ReferenceEqual(
                            data,
                            "Hint '"
                                + typeof(T).Name
                                + "' expected to copy value types by ref of type '"
                                + type.Name
                                + "'."
                        );
                }
                else
                {
                    result
                        .Data.Assert()
                        .ReferenceNotEqual(
                            data,
                            "Hint '"
                                + typeof(T).Name
                                + "' copied by ref instead of a deep clone of type '"
                                + type.Name
                                + "'."
                        );
                }
            }
            finally
            {
                Disposer.Cleanup(data, result.Data);
            }
        }
    }

    /// <summary>Verifies the hint doesn't support the wrong types.</summary>
    [Fact]
    public void TryCopy_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            object data = Tools.Randomizer.Create(type);
            try
            {
                TestInstance
                    .TryCopy(data, CreateChainer())
                    .Assert()
                    .Is(
                        CopyHintResult.None,
                        "Hint '" + typeof(T).Name + "' should not support type '" + type.Name + "'."
                    );
            }
            finally
            {
                Disposer.Cleanup(data);
            }
        }
    }

    /// <param name="optionConfiguration">Modifications of options to apply for this call.</param>
    /// <returns>Chainer to use for testing.</returns>
    protected static DuplicatorChainer CreateChainer(DuplicatorMod optionConfiguration = null)
    {
        return new DuplicatorChainer(
            optionConfiguration?.Invoke(Tools.Duplicator.Options) ?? Tools.Duplicator.Options,
            Tools.Duplicator,
            (o, c) => Tools.Duplicator.Copy(o)
        );
    }
}
