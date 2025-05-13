global using TesterMod = System.Func<
    CreateAndFake.TesterTool.TesterOptions,
    CreateAndFake.TesterTool.TesterOptions
>;
using System.Reflection;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.TesterTool;

/// <summary>Automates common tests.</summary>
public interface ITester : ITool<TesterOptions>
{
    /// <inheritdoc cref="PreventsNullRefException{T}(T,TesterMod)"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    Task PreventsNullRefException<T>(TesterMod? optionConfiguration = null);

    /// <inheritdoc cref="PreventsNullRefException{T}(T,TesterMod)"/>
    /// <param name="type">Type to verify.</param>
    Task PreventsNullRefException(Type type, TesterMod? optionConfiguration = null);

    /// <summary>
    ///     Verifies nulls are guarded on the type.
    ///     Tests each parameter possible with null.
    ///     Constructor and factory parameters are not tested by running all methods.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    Task PreventsNullRefException<T>(T instance, TesterMod? optionConfiguration = null);

    /// <inheritdoc cref="PreventsParameterMutation{T}(T,TesterMod)"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    Task PreventsParameterMutation<T>(TesterMod? optionConfiguration = null);

    /// <inheritdoc cref="PreventsParameterMutation{T}(T,TesterMod)"/>
    /// <param name="type">Type to verify.</param>
    Task PreventsParameterMutation(Type type, TesterMod? optionConfiguration = null);

    /// <summary>
    ///     Verifies mutations are prevented on the type.
    ///     Constructor and factory parameters are not tested by running all methods.
    ///     Ignores any exception and moves on.
    /// </summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    Task PreventsParameterMutation<T>(T instance, TesterMod? optionConfiguration = null);

    /// <inheritdoc cref="PassthroughWithNoExceptions"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    Task PassthroughWithNoExceptions<T>(TesterMod? optionConfiguration = null);

    /// <summary>Verifies no exceptions are thrown on any method when using injection and random data.</summary>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    Task PassthroughWithNoExceptions(object instance, TesterMod? optionConfiguration = null);

    /// <summary>
    ///     Verifies <paramref name="testAssembly"/> has a test class for all classes in <paramref name="codeAssembly"/>.
    /// </summary>
    /// <param name="codeAssembly">Assembly being tested.</param>
    /// <param name="testAssembly">Assembly with the tests.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    void ProvidesTestClassCoverage(
        Assembly codeAssembly,
        Assembly testAssembly,
        TesterMod? optionConfiguration = null
    );
}
