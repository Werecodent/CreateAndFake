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
    /// <param name="optionConfiguration">Modifications of Options for the new tool.</param>
    /// <returns>The created tool.</returns>
    ITester WithOptions(TesterMod optionConfiguration);

    /// <summary>Verifies <typeparamref name="T"/> can JSON serialize and deserialize.</summary>
    /// <inheritdoc cref="VerifyJsonSerialization{T}(T,TesterMod)"/>
    void VerifyJsonSerialization<T>(TesterMod? optionConfiguration = null);

    /// <typeparam name="T">The <see cref="Type"/> to test.</typeparam>
    /// <inheritdoc cref="VerifyJsonSerialization"/>
    void VerifyJsonSerialization<T>(T instance, TesterMod? optionConfiguration = null);

    /// <summary>
    ///     Verifies the <paramref name="instance"/> can JSON serialize and deserialize.
    /// </summary>
    /// <param name="instance">The data to test.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    void VerifyJsonSerialization(object instance, TesterMod? optionConfiguration = null);

    /// <summary>Verifies <typeparamref name="T"/> can XML serialize and deserialize.</summary>
    /// <inheritdoc cref="VerifyXmlSerialization{T}(T,TesterMod)"/>
    void VerifyXmlSerialization<T>(TesterMod? optionConfiguration = null);

    /// <typeparam name="T">The <see cref="Type"/> to test.</typeparam>
    /// <inheritdoc cref="VerifyXmlSerialization"/>
    void VerifyXmlSerialization<T>(T instance, TesterMod? optionConfiguration = null);

    /// <summary>
    ///     Verifies the <paramref name="instance"/> can XML serialize and deserialize.
    /// </summary>
    /// <param name="instance">The data to test.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    void VerifyXmlSerialization(object instance, TesterMod? optionConfiguration = null);

    /// <summary>Verifies <typeparamref name="T"/> has proper value equality rules for hashes.</summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    void VerifyEqualsMatchesHashCodes<T>(TesterMod? optionConfiguration = null);

    /// <summary>Verifies the <paramref name="type"/> has proper value equality rules for hashes.</summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    void VerifyEqualsMatchesHashCodes(Type type, TesterMod? optionConfiguration = null);

    /// <summary>Verifies <typeparamref name="T"/> uses all values for value equality and equality works.</summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    void VerifyValueEquality<T>(TesterMod? optionConfiguration = null);

    /// <summary>Verifies the <paramref name="type"/> uses all values for value equality and equality works.</summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    void VerifyValueEquality(Type type, TesterMod? optionConfiguration = null);

    /// <inheritdoc cref="PreventsNullRefExceptionAsync(Type,CancellationToken,TesterMod)"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    Task PreventsNullRefExceptionAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    ///  <summary>
    ///     Verifies nulls are guarded on the type.
    ///     Tests each parameter possible with null.
    ///     Constructor and factory parameters are tested by running all methods.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <inheritdoc cref="PreventsNullRefExceptionAsync{T}(T,CancellationToken,TesterMod)"/>
    /// <param name="type">Type to verify.</param>
    Task PreventsNullRefExceptionAsync(
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
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    Task PreventsNullRefExceptionAsync<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <inheritdoc cref="PreventsParameterMutationAsync{T}(T,CancellationToken,TesterMod)"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    Task PreventsParameterMutationAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <inheritdoc cref="PreventsParameterMutationAsync{T}(T,CancellationToken,TesterMod)"/>
    /// <param name="type">Type to verify.</param>
    Task PreventsParameterMutationAsync(
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
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    Task PreventsParameterMutationAsync<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <inheritdoc cref="PassthroughWithNoExceptionsAsync"/>
    /// <typeparam name="T">Type to verify.</typeparam>
    Task PassthroughWithNoExceptionsAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Verifies no exceptions are thrown on any method when using injection and random data.</summary>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    Task PassthroughWithNoExceptionsAsync(
        object instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>
    ///     Verifies <paramref name="testAssembly"/> has a test class for all classes in <paramref name="codeAssembly"/>.
    /// </summary>
    /// <param name="codeAssembly">Assembly being tested.</param>
    /// <param name="testAssembly">Assembly with the tests.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    void ProvidesTestClassCoverage(
        Assembly codeAssembly,
        Assembly testAssembly,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Verifies tests in <paramref name="testAssembly"/> have appropriate names.</summary>
    /// <param name="testMarkers">All test framework <see cref="Attribute"/>s that marks methods as tests.</param>
    /// <inheritdoc cref="ProvidesTestClassCoverage"/>
    void VerifyTestMethodNaming(
        IEnumerable<Type> testMarkers,
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
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    Task ValidateRandomDataParametersAsync(
        Assembly testAssembly,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Validates the state of the <c>CreateAndFake</c> framework as configured.</summary>
    /// <inheritdoc cref="VerifyToolSetSupportAsync(IEnumerable{Type},CancellationToken,TesterMod)"/>
    Task VerifyToolSetIntegrityAsync(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>
    ///     Validates <typeparamref name="T"/> are fully
    ///     compatible with the framework as configured.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> to verify support for.</typeparam>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task VerifyToolSetSupportAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>
    ///     Validates the <paramref name="type"/> are
    ///     fully compatible with the framework as configured.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to verify support for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task VerifyToolSetSupportAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>
    ///     Validates the <paramref name="types"/> are
    ///     fully compatible with the framework as configured.
    /// </summary>
    /// <param name="types"><see cref="Type"/>s to verify support for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task VerifyToolSetSupportAsync(
        IEnumerable<Type> types,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Triggers a debug assertion of all classes in the <paramref name="codeAssembly"/>.</summary>
    /// <param name="codeAssembly">Assembly being tested.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    void VerifyAllToStrings(Assembly codeAssembly, TesterMod? optionConfiguration = null);
}
