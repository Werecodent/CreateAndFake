global using TesterMod = System.Func<
    CreateAndFake.TesterTool.TesterOptions,
    CreateAndFake.TesterTool.TesterOptions
>;
using System.Reflection;
using CreateAndFake.Design.Tooling;
using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.TesterTool;

/// <summary>Automates common tests.</summary>
public interface ITester : ITool<TesterOptions>
{
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> for the new tool.</param>
    /// <returns>The created tool.</returns>
    ITester WithOptions(TesterMod optionConfiguration);

    /// <inheritdoc cref="PreventsNullRefException(Type,CancellationToken,TesterMod)"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    Task PreventsNullRefException<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    ///  <summary>
    ///     Verifies nulls are guarded on the type.
    ///     Tests each parameter possible with null.
    ///     Constructor and factory parameters are tested by running all methods.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <inheritdoc cref="PreventsNullRefException{T}(T,CancellationToken,TesterMod)"/>
    /// <param name="type">Type to verify.</param>
    Task PreventsNullRefException(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>
    ///     Verifies nulls are guarded on the type.
    ///     Tests each parameter possible with null.
    ///     Constructor and factory parameters are not tested by running all methods.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    Task PreventsNullRefException<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <inheritdoc cref="PreventsParameterMutation{T}(T,CancellationToken,TesterMod)"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    Task PreventsParameterMutation<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <inheritdoc cref="PreventsParameterMutation{T}(T,CancellationToken,TesterMod)"/>
    /// <param name="type">Type to verify.</param>
    Task PreventsParameterMutation(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>
    ///     Verifies mutations are prevented on the type.
    ///     Constructor and factory parameters are not tested by running all methods.
    ///     Ignores any exception and moves on.
    /// </summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    Task PreventsParameterMutation<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <inheritdoc cref="PassthroughWithNoExceptions"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    Task PassthroughWithNoExceptions<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Verifies no exceptions are thrown on any method when using injection and random data.</summary>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    Task PassthroughWithNoExceptions(
        object instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

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

    /// <summary>
    ///     Validates <paramref name="testAssembly"/> methods marked by
    ///     <see cref="IRandomDataMarker"/> can be populated with random data.
    /// </summary>
    /// <param name="testAssembly">Assembly with the tests.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    Task ValidateRandomDataParameters(
        Assembly testAssembly,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Validates the state of the <c>CreateAndFake</c> framework as configured.</summary>
    /// <param name="tools">Tools being used. Likely <see cref="ToolSet.DefaultSet"/>.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">
    ///     Modifications of <see cref="ITool{T}.Options"/> to apply for this call.
    /// </param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task VerifyToolSetIntegrity(
        ToolSet tools,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );
}
