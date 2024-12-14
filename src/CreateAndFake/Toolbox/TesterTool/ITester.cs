global using TesterMod = System.Func<
    CreateAndFake.Toolbox.TesterTool.TesterOptions,
    CreateAndFake.Toolbox.TesterTool.TesterOptions>;
namespace CreateAndFake.Toolbox.TesterTool;

/// <summary>Automates common tests.</summary>
public interface ITester
{
    /// <summary>Configured options for <c>this</c>.</summary>
    TesterOptions Options { get; }

    /// <inheritdoc cref="PreventsNullRefException{T}(T,TesterMod)"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    void PreventsNullRefException<T>(TesterMod? optionConfiguration = null);

    /// <inheritdoc cref="PreventsNullRefException{T}(T,TesterMod)"/>
    /// <param name="type">Type to verify.</param>
    void PreventsNullRefException(Type type, TesterMod? optionConfiguration = null);

    /// <summary>
    ///     Verifies nulls are guarded on the type.
    ///     Tests each parameter possible with null.
    ///     Constructor and factory parameters are not tested by running all methods.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    void PreventsNullRefException<T>(T instance, TesterMod? optionConfiguration = null);

    /// <inheritdoc cref="PreventsParameterMutation{T}(T,TesterMod)"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    void PreventsParameterMutation<T>(TesterMod? optionConfiguration = null);

    /// <inheritdoc cref="PreventsParameterMutation{T}(T,TesterMod)"/>
    /// <param name="type">Type to verify.</param>
    void PreventsParameterMutation(Type type, TesterMod? optionConfiguration = null);

    /// <summary>
    ///     Verifies mutations are prevented on the type.
    ///     Constructor and factory parameters are not tested by running all methods.
    ///     Ignores any exception and moves on.
    /// </summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    void PreventsParameterMutation<T>(T instance, TesterMod? optionConfiguration = null);

    /// <inheritdoc cref="PassthroughWithNoExceptions"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    void PassthroughWithNoExceptions<T>(TesterMod? optionConfiguration = null);

    /// <summary>Verifies no exceptions are thrown on any method when using injection and random data.</summary>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    void PassthroughWithNoExceptions(object instance, TesterMod? optionConfiguration = null);
}
