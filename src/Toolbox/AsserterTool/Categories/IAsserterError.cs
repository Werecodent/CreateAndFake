using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common exception test scenarios.</summary>
public interface IAsserterError
{
    /// <inheritdoc cref="Fail(Exception,AsserterMod,string)"/>
    [DoesNotReturn]
    void Fail(Exception? exception, string? details = null);

    /// <param name="exception">Exception responsible for the failure.</param>
    /// <inheritdoc cref="IAsserter.Fail(AsserterMod,string)"/>
    [DoesNotReturn]
    void Fail(Exception? exception, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="HasInner{T}(Exception,AsserterMod,string)"/>
    TException HasInner<TException>(Exception? exception, string? details = null)
        where TException : Exception;

    /// <summary>Ensures <paramref name="exception"/> has <see cref="Exception.InnerException"/> populated.</summary>
    /// <typeparam name="TException">Exception <see cref="Type"/> to check for.</typeparam>
    /// <param name="exception">Exception to check on.</param>
    /// <returns>The inner exception.</returns>
    /// <inheritdoc cref="Debug(Exception,AsserterMod,string)"/>
    TException HasInner<TException>(
        Exception? exception,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception;

    /// <inheritdoc cref="HasInnerException(Exception,Exception,AsserterMod,string)"/>
    void HasInnerException(Exception? outer, Exception? inner, string? details = null);

    /// <inheritdoc cref="HasInner{T}(Exception,AsserterMod,string)"/>
    void HasInnerException(
        Exception? outer,
        Exception? inner,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="Debug(Exception,AsserterMod,string)"/>
    void Debug(Exception? exception, string? details = null);

    /// <param name="exception">Exception responsible for the failure.</param>
    /// <inheritdoc cref="IAsserter.Debug(AsserterMod,string)"/>
    void Debug(Exception? exception, AsserterMod? optionConfiguration, string? details = null);
}
