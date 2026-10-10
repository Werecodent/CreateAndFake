using System.Reflection;
using System.Runtime.Serialization;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool;

/// <summary>Handles testing copy hints.</summary>
/// <typeparam name="T">Copy hint to test.</typeparam>
/// <param name="validTypes">Types that can be copied by the hint.</param>
/// <param name="invalidTypes">Types that can't be copied by the hint.</param>
public abstract class CopyHintTestBase<T>(
    IEnumerable<Type> validTypes = null,
    IEnumerable<Type> invalidTypes = null
)
    where T : CopyHint, new()
{
    /// <summary>Configuration to use for automatic tests.</summary>
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(ToolException),
                typeof(ArgumentException),
                typeof(UnsupportedException),
                typeof(NotSupportedException),
                typeof(ArgumentNullException),
                typeof(SerializationException),
                typeof(PlatformNotSupportedException),
                typeof(TargetParameterCountException),
            ],
        };

    /// <summary>Instance to test with.</summary>
    protected T TestInstance { get; } = new T();

    /// <summary>Types that can be copied by the hint.</summary>
    private readonly IEnumerable<Type> _validTypes = validTypes ?? new T().SupportedTypes;

    /// <summary>Types that can't be copied by the hint.</summary>
    private readonly IEnumerable<Type> _invalidTypes = invalidTypes ?? [typeof(DataHolderSample)];

    /// <inheritdoc cref="ITester.PreventsNullRefExceptionAsync"/>
    [Fact]
    public Task CopyHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<T>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutationAsync"/>
    [Fact]
    public virtual Task CopyHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<T>(
            TestContext.Current.CancellationToken,
            _Config
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
                data = type.Tools().CreateRandomInstance();
                result = TestInstance.TryCopy(data, CreateChainer());

                await result
                    .HasData.Assert()
                    .Is(
                        true,
                        $"Hint '{GenericConverter.ExpandName<T>()}' failed to support "
                            + $"value of type '{GenericConverter.ExpandName(type)}'. "
                            + $"Actual type '{GenericConverter.ExpandName(data)}'."
                    )
                    .Also(result.Data)
                    .IsAsync(
                        data,
                        TestContext.Current.CancellationToken,
                        $"Hint '{GenericConverter.ExpandName<T>()}' failed to create clone that's "
                            + $"equal by value for type '{GenericConverter.ExpandName(type)}'. "
                            + $"Actual type '{GenericConverter.ExpandName(data)}'."
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
            object data = type.Tools().CreateRandomInstance();
            try
            {
                await TestInstance
                    .TryCopy(data, CreateChainer())
                    .Assert()
                    .IsAsync(
                        CopyHintResult.None,
                        TestContext.Current.CancellationToken,
                        $"Hint '{GenericConverter.ExpandName<T>()}' should not support "
                            + $"value of type '{GenericConverter.ExpandName(type)}'. "
                            + $"Actual type '{GenericConverter.ExpandName(data)}'."
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
