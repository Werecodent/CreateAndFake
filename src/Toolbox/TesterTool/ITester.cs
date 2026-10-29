global using TesterMod = System.Func<
    Werecodent.CreateAndFake.TesterTool.TesterOptions,
    Werecodent.CreateAndFake.TesterTool.TesterOptions
>;
using System.Reflection;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.TesterTool;

/// <summary>Tool for testing common scenarios.</summary>
/// <remarks>Useful for automating tests.</remarks>
public interface ITester : ITool<TesterOptions>
{
    /// <summary>Creates a new tool with the <paramref name="optionConfiguration"/> changes applied.</summary>
    /// <param name="optionConfiguration">Modifications of this tool's options for the new tool.</param>
    /// <returns>The created tool.</returns>
    ITester WithOptions(TesterMod optionConfiguration);

    /// <summary>Verifies <typeparamref name="T"/> can JSON serialize and deserialize.</summary>
    /// <inheritdoc cref="VerifyJsonSerialization{T}(T,TesterMod)"/>
    void VerifyJsonSerialization<T>(TesterMod? optionConfiguration = null);

    /// <summary>
    ///     Verifies the <paramref name="instance"/> can JSON serialize and deserialize.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to test.</typeparam>
    /// <param name="instance">The data to test.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    void VerifyJsonSerialization<T>(T instance, TesterMod? optionConfiguration = null);

    /// <summary>Verifies <typeparamref name="T"/> can XML serialize and deserialize.</summary>
    /// <inheritdoc cref="VerifyXmlSerialization{T}(T,TesterMod)"/>
    void VerifyXmlSerialization<T>(TesterMod? optionConfiguration = null);

    /// <summary>
    ///     Verifies the <paramref name="instance"/> can XML serialize and deserialize.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to test.</typeparam>
    /// <param name="instance">The data to test.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    void VerifyXmlSerialization<T>(T instance, TesterMod? optionConfiguration = null);

    /// <summary>Verifies <typeparamref name="T"/> has proper value equality rules for hashes.</summary>
    /// <typeparam name="T"><see cref="Type"/> to verify.</typeparam>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    void VerifyEqualsMatchesHashCodes<T>(TesterMod? optionConfiguration = null);

    /// <summary>Verifies the <paramref name="type"/> has proper value equality rules for hashes.</summary>
    /// <param name="type"><see cref="Type"/> to verify.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    void VerifyEqualsMatchesHashCodes(Type type, TesterMod? optionConfiguration = null);

    /// <summary>Verifies <typeparamref name="T"/> uses all values for value equality and equality works.</summary>
    /// <typeparam name="T"><see cref="Type"/> to verify.</typeparam>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    void VerifyValueEquality<T>(TesterMod? optionConfiguration = null);

    /// <summary>Verifies the <paramref name="type"/> uses all values for value equality and equality works.</summary>
    /// <param name="type"><see cref="Type"/> to verify.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    void VerifyValueEquality(Type type, TesterMod? optionConfiguration = null);

    /// <summary>Verifies cancellation behavior.</summary>
    /// <typeparam name="T"><see cref="Type"/> to verify.</typeparam>
    /// <param name="factory">Behavior to test.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="TesterOptions.ThrowsUponCancelWithEmptySeries"/></item>
    ///     </list>
    /// </remarks>
    Task VerifySupportsCancellationAsync<T>(
        Func<int, IAsyncEnumerable<T>> factory,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <inheritdoc cref="VerifySupportsCancellationAsync{T}(Func{int,IAsyncEnumerable{T}},CancellationToken,TesterMod?)"/>
    Task VerifySupportsCancellationAsync<T>(
        Func<int, CancellationToken, IAsyncEnumerable<T>> factory,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <typeparam name="T"><see cref="Type"/> to verify.</typeparam>
    /// <inheritdoc cref="PreventsNullRefExceptionAsync(Type,CancellationToken,TesterMod)"/>
    Task PreventsNullRefExceptionAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    ///  <summary>
    ///     Verifies nulls are guarded on the type.
    ///     Tests each parameter possible with null.
    ///     Constructor and factory parameters are tested by running all methods.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to verify.</param>
    /// <inheritdoc cref="PreventsNullRefExceptionAsync{T}(T,CancellationToken,TesterMod)"/>
    Task PreventsNullRefExceptionAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>
    ///     Verifies nulls are guarded on the type.
    ///     Tests each parameter possible with null.
    ///     Constructor and factory parameters are not tested by running all methods.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> to verify.</typeparam>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="TesterOptions.InjectionValues"/></item>
    ///         <item><see cref="TesterOptions.MethodsToIgnore"/></item>
    ///         <item><see cref="TesterOptions.IncludeInternals"/></item>
    ///         <item><see cref="TesterOptions.IgnorableExceptions"/></item>
    ///         <item><see cref="TesterOptions.IgnoreAllExceptions"/></item>
    ///         <item><see cref="TesterOptions.OnlyDeclaredMethods"/></item>
    ///         <item><see cref="TesterOptions.IncludeConstructors"/></item>
    ///         <item><see cref="TesterOptions.IncludeStaticMethods"/></item>
    ///         <item><see cref="TesterOptions.IgnoreObsoleteMethods"/></item>
    ///         <item><see cref="TesterOptions.IncludeInstanceMethods"/></item>
    ///         <item><see cref="TesterOptions.DisableNullRefExceptionTests"/></item>
    ///     </list>
    /// </remarks>
    Task PreventsNullRefExceptionAsync<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <typeparam name="T"><see cref="Type"/> to verify.</typeparam>
    /// <inheritdoc cref="PreventsParameterMutationAsync{T}(T,CancellationToken,TesterMod)"/>
    Task PreventsParameterMutationAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>
    ///     Verifies mutations are prevented on the type.
    ///     Constructor and factory parameters are tested by running all methods.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to verify.</param>
    /// <inheritdoc cref="PreventsParameterMutationAsync{T}(T,CancellationToken,TesterMod)"/>
    Task PreventsParameterMutationAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>
    ///     Verifies mutations are prevented on the type.
    ///     Constructor and factory parameters are not tested by running all methods.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> to verify.</typeparam>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="TesterOptions.InjectionValues"/></item>
    ///         <item><see cref="TesterOptions.MethodsToIgnore"/></item>
    ///         <item><see cref="TesterOptions.IncludeInternals"/></item>
    ///         <item><see cref="TesterOptions.IgnorableExceptions"/></item>
    ///         <item><see cref="TesterOptions.IgnoreAllExceptions"/></item>
    ///         <item><see cref="TesterOptions.OnlyDeclaredMethods"/></item>
    ///         <item><see cref="TesterOptions.IncludeConstructors"/></item>
    ///         <item><see cref="TesterOptions.IncludeStaticMethods"/></item>
    ///         <item><see cref="TesterOptions.IgnoreObsoleteMethods"/></item>
    ///         <item><see cref="TesterOptions.IncludeInstanceMethods"/></item>
    ///         <item><see cref="TesterOptions.DisableParameterMutationTests"/></item>
    ///     </list>
    /// </remarks>
    Task PreventsParameterMutationAsync<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <typeparam name="T"><see cref="Type"/> to verify.</typeparam>
    /// <inheritdoc cref="PassthroughWithNoExceptionsAsync"/>
    Task PassthroughWithNoExceptionsAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Verifies no exceptions are thrown on any method when using injection and random data.</summary>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="TesterOptions.MethodsToIgnore"/></item>
    ///         <item><see cref="TesterOptions.IncludeInternals"/></item>
    ///         <item><see cref="TesterOptions.IgnorableExceptions"/></item>
    ///         <item><see cref="TesterOptions.IgnoreAllExceptions"/></item>
    ///         <item><see cref="TesterOptions.OnlyDeclaredMethods"/></item>
    ///         <item><see cref="TesterOptions.IgnoreObsoleteMethods"/></item>
    ///         <item><see cref="TesterOptions.DisablePassthroughTests"/></item>
    ///     </list>
    /// </remarks>
    Task PassthroughWithNoExceptionsAsync(
        object instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>
    ///     Verifies <paramref name="testAssembly"/> has a test
    ///     class for all classes in <paramref name="codeAssembly"/>.
    /// </summary>
    /// <param name="codeAssembly">Assembly being tested.</param>
    /// <param name="testAssembly">Assembly with the tests.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="TesterOptions.TestClassNameSuffixes"/></item>
    ///         <item><see cref="TesterOptions.ClassCoverageExclusions"/></item>
    ///         <item><see cref="TesterOptions.AllowedTestNamePrefixes"/></item>
    ///         <item><see cref="TesterOptions.TestClassNameGenericSubstitutes"/></item>
    ///     </list>
    /// </remarks>
    void ProvidesTestClassCoverage(
        Assembly codeAssembly,
        Assembly testAssembly,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Verifies tests in <paramref name="testAssembly"/> have appropriate names.</summary>
    /// <param name="testMarkers">All test framework <see cref="Attribute"/>s that marks methods as tests.</param>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="TesterOptions.TestClassNameSuffixes"/></item>
    ///         <item><see cref="TesterOptions.AllowedTestNamePrefixes"/></item>
    ///         <item><see cref="TesterOptions.TestClassNameGenericSubstitutes"/></item>
    ///     </list>
    /// </remarks>
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
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    Task ValidateRandomDataParametersAsync(
        Assembly testAssembly,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Validates a configuration.</summary>
    /// <param name="environmentName">Name of the environment to test.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task ValidateTestSettingsConfigAsync(
        string? environmentName,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Validates the state of the <c>CreateAndFake</c> framework as configured.</summary>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="TesterOptions.IntegrityIgnorableTypes"/></item>
    ///     </list>
    /// </remarks>
    /// <inheritdoc cref="VerifyToolSetSupportAsync(IEnumerable{Type},CancellationToken,TesterMod)"/>
    Task VerifyToolSetIntegrityAsync(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Validates <typeparamref name="T"/> are fully compatible with the framework as configured.</summary>
    /// <typeparam name="T"><see cref="Type"/> to verify support for.</typeparam>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task VerifyToolSetSupportAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Validates the <paramref name="type"/> are fully compatible with the framework as configured.</summary>
    /// <param name="type"><see cref="Type"/> to verify support for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task VerifyToolSetSupportAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Validates the <paramref name="types"/> are fully compatible with the framework as configured.</summary>
    /// <param name="types"><see cref="Type"/>s to verify support for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task VerifyToolSetSupportAsync(
        IEnumerable<Type> types,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    );

    /// <summary>Triggers a debug assertion of all classes in the <paramref name="codeAssembly"/>.</summary>
    /// <param name="codeAssembly">Assembly being tested.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    void VerifyAllToStrings(Assembly codeAssembly, TesterMod? optionConfiguration = null);
}
