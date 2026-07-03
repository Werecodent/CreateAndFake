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

    /// <inheritdoc cref="Debug(Exception,AsserterMod,string)"/>
    void Debug(Exception? exception, string? details = null);

    /// <param name="exception">Exception responsible for the failure.</param>
    /// <inheritdoc cref="IAsserter.Debug(AsserterMod,string)"/>
    void Debug(Exception? exception, AsserterMod? optionConfiguration, string? details = null);
}
