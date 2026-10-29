namespace Werecodent.CreateAndFake.AsserterTool.Categories;

#pragma warning disable CA1711 // Follows existing pattern.

/// <summary>Handles common delegate test scenarios.</summary>
public interface IAsserterDelegate
{
    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Delegate? behavior, string? details = null)
        where T : Exception;

    /// <summary>Verifies the <c>behavior</c> throws a <typeparamref name="T"/> <see cref="Exception"/>.</summary>
    /// <typeparam name="T"><see cref="Exception"/> <see cref="Type"/> expected to be thrown.</typeparam>
    /// <returns>The thrown <see cref="Exception"/>.</returns>
    /// <remarks>Important relevant options: <list type="bullet">
    ///     <item><see cref="AsserterOptions.DisableCatchingForThrowAsserts"/></item>
    ///     <item><see cref="AsserterOptions.IncludeInternalExceptionForThrowAsserts"/></item>
    /// </list></remarks>
    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Delegate? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsException(Delegate,AsserterMod,string)"/>
    Exception ThrowsException(Delegate? behavior, string? details = null);

    /// <summary>Verifies the <c>behavior</c> throws an <see cref="Exception"/>.</summary>
    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    Exception ThrowsException(
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Delegate? behavior, string? details = null)
        where T : Exception;

    /// <summary>
    ///     Verifies the <c>behavior</c> does not throw a <typeparamref name="T"/> <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="Exception"/> <see cref="Type"/> expected to not be thrown.</typeparam>
    /// <param name="behavior">Instance to test.</param>
    /// <inheritdoc cref="IAsserterObject.Is(object,object,AsserterMod,string)"/>
    void ThrowsNo<T>(Delegate? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsNoException(Delegate,AsserterMod,string)"/>
    void ThrowsNoException(Delegate? behavior, string? details = null);

    /// <summary>Verifies the <c>behavior</c> does not throw an <see cref="Exception"/>.</summary>
    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNoException(
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="HasResult{T}(Delegate,AsserterMod,string)"/>
    T HasResult<T>(Delegate? behavior, string? details = null);

    /// <summary>
    ///     Verifies the <c>behavior</c> successfully executes resulting with a <typeparamref name="T"/> value.
    /// </summary>
    /// <inheritdoc cref="HasResult{T}(T,Delegate,AsserterMod,string)"/>
    T HasResult<T>(Delegate? behavior, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="HasResult{T}(Delegate,AsserterMod,string)"/>
    T HasResult<T>(T content, Delegate? behavior, string? details = null);

    /// <inheritdoc cref="HasResultAsync{T}(T,Delegate,CancellationToken,AsserterMod,string)"/>
    T HasResult<T>(
        T content,
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="HasResultAsync{T}(T,Delegate,CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        T content,
        Delegate? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <summary>
    ///     Verifies the <c>behavior</c> successfully executes resulting with the given <paramref name="content"/>.
    /// </summary>
    /// <typeparam name="T">Expected return <see cref="Type"/> of the <c>behavior</c>.</typeparam>
    /// <param name="content">Expected result for the <c>behavior</c>.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>Result from invoking the <c>behavior</c>.</returns>
    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        T content,
        Delegate? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}

#pragma warning restore
