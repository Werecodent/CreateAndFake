using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.TesterTool.Guarders;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.TesterTool;

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
    public virtual void VerifyEqualsMatchesHashCodes<T>(TesterMod? optionConfiguration = null)
    {
        VerifyEqualsMatchesHashCodes(typeof(T), optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual void VerifyEqualsMatchesHashCodes(
        Type type,
        TesterMod? optionConfiguration = null
    )
    {
        new EqualityValidator(Configure(optionConfiguration)).VerifyEqualsMatchesHashCodes(type);
    }

    /// <inheritdoc/>
    public virtual void VerifyValueEquality<T>(TesterMod? optionConfiguration = null)
    {
        VerifyValueEquality(typeof(T), optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual void VerifyValueEquality(Type type, TesterMod? optionConfiguration = null)
    {
        new EqualityValidator(Configure(optionConfiguration)).VerifyValueEquality(type);
    }

    /// <inheritdoc/>
    public virtual Task PreventsNullRefExceptionAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return PreventsNullRefExceptionAsync(typeof(T), canceler, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual Task PreventsNullRefExceptionAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        TesterOptions localOptions = Configure(optionConfiguration);

        return new NullGuarder(localOptions).PreventsNullRefExceptionAsync(
            GenericFixer.FixType(type, localOptions),
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
        return PreventsParameterMutationAsync(typeof(T), canceler, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual Task PreventsParameterMutationAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        TesterOptions localOptions = Configure(optionConfiguration);

        return new MutationGuarder(localOptions).PreventsParameterMutationAsync(
            GenericFixer.FixType(type, localOptions),
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
    public void VerifyAllToStrings(Assembly codeAssembly, TesterMod? optionConfiguration = null)
    {
        new SupportValidator(Configure(optionConfiguration)).VerifyAllToStrings(codeAssembly);
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
    public virtual Task VerifyToolSetSupportAsync<T>(
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        return VerifyToolSetSupportAsync(typeof(T), canceler, optionConfiguration);
    }

    /// <inheritdoc/>
    public virtual Task VerifyToolSetSupportAsync(
        Type type,
        CancellationToken canceler,
        TesterMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(type);

        return new SupportValidator(Configure(optionConfiguration)).VerifyToolSetSupportAsync(
            type,
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
