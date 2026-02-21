using System.Reflection;
using System.Runtime.Serialization;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.TesterTool;

namespace CreateAndFake.Tests.DuplicatorTool;

/// <summary>Handles testing copy hints.</summary>
/// <typeparam name="T">Copy hint to test.</typeparam>
/// <param name="validTypes">Types that can be copied by the hint.</param>
/// <param name="invalidTypes">Types that can't be copied by the hint.</param>
public abstract class CopyHintTestBase<T>(
    IEnumerable<Type> validTypes,
    IEnumerable<Type> invalidTypes
)
    where T : CopyHint, new()
{
    /// <summary>Configuration to use for automatic tests.</summary>
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(NotSupportedException),
                typeof(TargetParameterCountException),
                typeof(ArgumentException),
                typeof(ToolException),
                typeof(SerializationException),
                typeof(PlatformNotSupportedException),
            ],
        };

    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = new T();

    /// <summary>Types that can be copied by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? Type.EmptyTypes;

    /// <summary>Types that can't be copied by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes ?? Type.EmptyTypes;

    /// <inheritdoc cref="ITester.PreventsNullRefExceptionAsync"/>
    [Fact]
    public Task CopyHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            TestInstance,
            TestContext.Current.CancellationToken,
            config
        );
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutationAsync"/>
    [Fact]
    public Task CopyHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            TestInstance,
            TestContext.Current.CancellationToken,
            config
        );
    }

    /// <summary>Verifies the hint supports the correct types.</summary>
    [Fact]
    public async Task TryCopy_SupportsValidTypes()
    {
        foreach (Type type in _validTypes)
        {
            object data = null;
            CopyHintResult result = CopyHintResult.None;
            try
            {
                data = Tools.Randomizer.Create(type);
                result = TestInstance.TryCopy(data, CreateChainer());

                await Tools.Asserter.IsAsync(
                    new CopyHintResult(data),
                    result,
                    TestContext.Current.CancellationToken,
                    $"Hint '{typeof(T).Name}' failed to clone type '{type.Name}'."
                        + $"Actual type: '{data?.GetType()}'."
                );

                await result
                    .Data.Assert()
                    .IsAsync(
                        data,
                        TestContext.Current.CancellationToken,
                        $"Hint '{typeof(T).Name}' failed to create clone that's equal by "
                            + $"value for type '{type.Name}'. Actual type '{data?.GetType()}'."
                    );
            }
            finally
            {
                await Disposer.CleanupAsync(data, result.Data);
            }
        }
    }

    /// <summary>Verifies the hint doesn't support the wrong types.</summary>
    [Fact]
    public async Task TryCopy_InvalidTypesFail()
    {
        foreach (Type type in _invalidTypes)
        {
            object data = Tools.Randomizer.Create(type);
            try
            {
                await TestInstance
                    .TryCopy(data, CreateChainer())
                    .Assert()
                    .IsAsync(
                        CopyHintResult.None,
                        TestContext.Current.CancellationToken,
                        "Hint '" + typeof(T).Name + "' should not support type '" + type.Name + "'."
                    );
            }
            finally
            {
                await Disposer.CleanupAsync(data);
            }
        }
    }

    /// <summary>Create a chainer to use for testing.</summary>
    /// <param name="optionConfiguration">Modifications of options to apply for this call.</param>
    /// <returns>Chainer to use for testing.</returns>
    protected static IDuplicatorChainer CreateChainer(DuplicatorMod optionConfiguration = null)
    {
        return new DuplicatorChainer(
            optionConfiguration?.Invoke(Tools.Duplicator.Options) ?? Tools.Duplicator.Options,
            new DuplicatorEngine()
        );
    }
}
