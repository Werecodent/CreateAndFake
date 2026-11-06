using Werecodent.CreateAndFake.Design.Exceptions;

namespace Werecodent.CreateAndFake.TesterTool;

/// <typeparam name="T"><see cref="Type"/> under test.</typeparam>
/// <inheritdoc/>
public abstract class BaseAutoTester<T> : BaseAutoTester
{
    /// <summary>Backing value for <see cref="TestInstance"/>.</summary>
    /// <remarks>Used to track if <see cref="TestInstance"/> has been overridden.</remarks>
    private readonly Lazy<T> _defaultTestInstance;

    /// <summary>Specific instance used for automated tests.</summary>
    /// <value><c>AutoTools.Randomizer.Create&lt;T&gt;()</c></value>
    protected virtual T TestInstance => _defaultTestInstance.Value;

    /// <inheritdoc cref="BaseAutoTester{T}"/>
    protected BaseAutoTester()
        : base(typeof(T))
    {
        _defaultTestInstance = new(() => AutoTools.Randomizer.Create<T>());
    }

    /// <summary>Identifies if <see cref="TestInstance"/> has been set by a subclass.</summary>
    /// <returns><see langword="true"/> if overridden, <see langword="false"/> otherwise.</returns>
    private bool IsTestInstanceOverridden()
    {
        return TestInstance is not null && !_defaultTestInstance.IsValueCreated;
    }

    /// <inheritdoc/>
    protected override async Task GuardsNullsImplementation(CancellationToken canceler)
    {
        try
        {
            if (IsTestInstanceOverridden())
            {
                await AutoTools
                    .Tester.PreventsNullRefExceptionAsync(
                        TestInstance,
                        canceler,
                        OptionConfiguration
                    )
                    .ConfigureAwait(false);
            }
            else
            {
                await AutoTools
                    .Tester.PreventsNullRefExceptionAsync<T>(canceler, OptionConfiguration)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            throw new AutoTesterFailureException(GetType(), e);
        }
    }

    /// <inheritdoc/>
    protected override async Task NoParameterMutationImplementation(CancellationToken canceler)
    {
        try
        {
            if (IsTestInstanceOverridden())
            {
                await AutoTools
                    .Tester.PreventsParameterMutationAsync(
                        TestInstance,
                        canceler,
                        OptionConfiguration
                    )
                    .ConfigureAwait(false);
            }
            else
            {
                await AutoTools
                    .Tester.PreventsParameterMutationAsync<T>(canceler, OptionConfiguration)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            throw new AutoTesterFailureException(GetType(), e);
        }
    }

    /// <inheritdoc/>
    protected override void DebugToStringImplementation()
    {
        try
        {
            AutoTools.Asserter.Debug(TestInstance);
        }
        catch (Exception e)
        {
            throw new AutoTesterFailureException(GetType(), e);
        }
    }
}
