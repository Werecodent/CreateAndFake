using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.TesterTool.Guarders;
using CreateAndFake.TesterTool.Validators;

namespace CreateAndFake.TesterTool;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public class Tester(TesterOptions options) : ITester
{
    /// <inheritdoc/>
    public TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Configures options to use for a test.</summary>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply.</param>
    /// <returns>The options to use.</returns>
    private TesterOptions Configure(TesterMod? optionConfiguration = null)
    {
        return optionConfiguration?.Invoke(Options) ?? Options;
    }

    /// <inheritdoc/>
    public virtual void VerifyJsonSerialization<T>(TesterMod? optionConfiguration = null)
    {
        new SerializationValidator(Configure(optionConfiguration)).VerifyJsonSerialization<T>();
    }

    /// <inheritdoc/>
    public virtual void VerifyJsonSerialization<T>(
        T instance,
        TesterMod? optionConfiguration = null
    )
    {
        new SerializationValidator(Configure(optionConfiguration)).VerifyJsonSerialization(
            instance
        );
    }

    /// <inheritdoc/>
    public virtual void VerifyJsonSerialization(
        object instance,
        TesterMod? optionConfiguration = null
    )
    {
        new SerializationValidator(Configure(optionConfiguration)).VerifyJsonSerialization(
            instance
        );
    }

    /// <inheritdoc/>
    public virtual void VerifyXmlSerialization<T>(TesterMod? optionConfiguration = null)
    {
        new SerializationValidator(Configure(optionConfiguration)).VerifyXmlSerialization<T>();
    }

    /// <inheritdoc/>
    public virtual void VerifyXmlSerialization<T>(T instance, TesterMod? optionConfiguration = null)
    {
        new SerializationValidator(Configure(optionConfiguration)).VerifyXmlSerialization(instance);
    }

    /// <inheritdoc/>
    public virtual void VerifyXmlSerialization(
        object instance,
        TesterMod? optionConfiguration = null
    )
    {
        new SerializationValidator(Configure(optionConfiguration)).VerifyXmlSerialization(instance);
    }

    /// <inheritdoc/>
    public virtual Task PreventsNullRefExceptionAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new NullGuarder(Configure(optionConfiguration)).PreventsNullRefExceptionAsync<T>(
            canceler
        );
    }

    /// <inheritdoc/>
    public virtual Task PreventsNullRefExceptionAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new NullGuarder(Configure(optionConfiguration)).PreventsNullRefExceptionAsync(
            type,
            canceler
        );
    }

    /// <inheritdoc/>
    public virtual Task PreventsNullRefExceptionAsync<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new NullGuarder(Configure(optionConfiguration)).PreventsNullRefExceptionAsync(
            instance,
            canceler
        );
    }

    /// <inheritdoc/>
    public virtual Task PreventsParameterMutationAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new MutationGuarder(
            Configure(optionConfiguration)
        ).PreventsParameterMutationAsync<T>(canceler);
    }

    /// <inheritdoc/>
    public virtual Task PreventsParameterMutationAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new MutationGuarder(Configure(optionConfiguration)).PreventsParameterMutationAsync(
            type,
            canceler
        );
    }

    /// <inheritdoc/>
    public virtual Task PreventsParameterMutationAsync<T>(
        T instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new MutationGuarder(Configure(optionConfiguration)).PreventsParameterMutationAsync(
            instance,
            canceler
        );
    }

    /// <inheritdoc/>
    public virtual Task PassthroughWithNoExceptionsAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new ExceptionGuarder(
            Configure(optionConfiguration)
        ).PassthroughWithNoExceptionsAsync<T>(canceler);
    }

    /// <inheritdoc/>
    public virtual Task PassthroughWithNoExceptionsAsync(
        object instance,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new ExceptionGuarder(
            Configure(optionConfiguration)
        ).PassthroughWithNoExceptionsAsync(instance, canceler);
    }

    /// <inheritdoc/>
    public virtual void ProvidesTestClassCoverage(
        Assembly codeAssembly,
        Assembly testAssembly,
        TesterMod? optionConfiguration = null
    )
    {
        new TestValidator(Configure(optionConfiguration)).ProvidesTestClassCoverage(
            codeAssembly,
            testAssembly
        );
    }

    /// <inheritdoc/>
    public virtual void VerifyTestMethodNaming(
        IEnumerable<Type> testMarkers,
        Assembly codeAssembly,
        Assembly testAssembly,
        TesterMod? optionConfiguration = null
    )
    {
        new TestValidator(Configure(optionConfiguration)).VerifyTestMethodNaming(
            testMarkers,
            codeAssembly,
            testAssembly
        );
    }

    /// <inheritdoc/>
    public virtual Task ValidateRandomDataParametersAsync(
        Assembly testAssembly,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new SupportValidator(
            Configure(optionConfiguration)
        ).ValidateRandomDataParametersAsync(testAssembly, canceler);
    }

    /// <inheritdoc/>
    public virtual Task VerifyToolSetIntegrityAsync(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new SupportValidator(Configure(optionConfiguration)).VerifyToolSetIntegrityAsync(
            canceler
        );
    }

    /// <inheritdoc/>
    public virtual Task VerifyToolSetSupportAsync(
        IEnumerable<Type> types,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return new SupportValidator(Configure(optionConfiguration)).VerifyToolSetSupportAsync(
            types,
            canceler
        );
    }

    /// <inheritdoc/>
    public ITester WithOptions(TesterMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Tester(optionConfiguration.Invoke(Options));
    }
}
