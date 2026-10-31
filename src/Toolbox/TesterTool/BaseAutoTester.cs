using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.TesterTool;

#pragma warning disable CA1707, VSTHRD200 // Match test format.

/// <summary>Automatically configures and runs common tests.</summary>
/// <param name="testType"><see cref="Type"/> under test.</param>
public abstract class BaseAutoTester(Type testType)
{
    /// <summary>Modifications of the <see cref="ITester"/>'s options to apply for the tests.</summary>
    protected virtual TesterMod? OptionConfiguration { get; } = null;

    /// <summary>Determines the behavior for <see cref="Tester_ValueEqualityOverrideCheck"/>.</summary>
    /// <remarks><list type="bullet">
    ///     <item><see langword="true"/> = <see cref="ITester.VerifyValueEquality"/></item>
    ///     <item><see langword="false"/> = <see cref="ITester.VerifyEqualsMatchesHashCodes"/></item>
    /// </list></remarks>
    /// <value><see langword="true"/></value>
    protected virtual bool EqualsOverrideUsesFullValueEquality { get; } = true;

    /// <summary>Reflection tools used to run the tests.</summary>
    /// <value><c>Tools.Source</c></value>
    protected virtual ToolSet AutoTools => Tools.Source;

    /// <inheritdoc cref="TypeDescriber"/>
    protected TypeDescriber TestTypeDescriber { get; } = TypeDescriber.For(testType);

    /// <summary>Verifies nulls are guarded on the type.</summary>
    /// <remarks><see cref="ITester.PreventsNullRefExceptionAsync"/></remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public abstract Task Tester_GuardsNulls();

    /// <inheritdoc cref="Tester_GuardsNulls"/>
    protected virtual async Task GuardsNullsImplementation(CancellationToken canceler)
    {
        try
        {
            await AutoTools
                .Tester.PreventsNullRefExceptionAsync(testType, canceler, OptionConfiguration)
                .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new AutoTesterFailureException(GetType(), e);
        }
    }

    /// <summary>Verifies mutations are prevented on the type.</summary>
    /// <remarks><see cref="ITester.PreventsNullRefExceptionAsync"/></remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public abstract Task Tester_NoParameterMutation();

    /// <inheritdoc cref="Tester_NoParameterMutation"/>
    protected virtual async Task NoParameterMutationImplementation(CancellationToken canceler)
    {
        try
        {
            await AutoTools
                .Tester.PreventsParameterMutationAsync(testType, canceler, OptionConfiguration)
                .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new AutoTesterFailureException(GetType(), e);
        }
    }

    /// <summary>Verifies proper value equality rules.</summary>
    /// <remarks>Control via: <see cref="EqualsOverrideUsesFullValueEquality"/></remarks>
    public abstract void Tester_ValueEqualityOverrideCheck();

    /// <inheritdoc cref="Tester_ValueEqualityOverrideCheck"/>
    protected virtual void ValueEqualityOverrideCheckImplementation()
    {
        if (
            testType.GetMethod(nameof(Equals), [typeof(object)])!.DeclaringType != typeof(object)
            || TestTypeDescriber.Inherits<IValuerEquatable>()
            || TestTypeDescriber.Inherits<IValueEquatable>()
        )
        {
            try
            {
                if (EqualsOverrideUsesFullValueEquality)
                {
                    AutoTools.Tester.VerifyValueEquality(testType);
                }
                else
                {
                    AutoTools.Tester.VerifyEqualsMatchesHashCodes(testType);
                }
            }
            catch (Exception e)
            {
                throw new AutoTesterFailureException(GetType(), e);
            }
        }
    }

    /// <summary>
    ///     Allows manual inspection of <see cref="object.ToString"/> when
    ///     <see cref="AsserterOptions.TriggerFailureForDebugAsserts"/> is set.
    /// </summary>
    /// <remarks><see cref="IAsserter.Debug(string?)"/></remarks>
    public abstract void Tester_Debug_ToString();

    /// <inheritdoc cref="Tester_Debug_ToString"/>
    protected virtual void DebugToStringImplementation()
    {
        if (!TestTypeDescriber.IsStatic())
        {
            try
            {
                AutoTools.Asserter.Debug(AutoTools.Randomizer.Create(testType).ToString());
            }
            catch (Exception e)
            {
                throw new AutoTesterFailureException(GetType(), e);
            }
        }
    }
}

#pragma warning restore
