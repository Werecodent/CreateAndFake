namespace Werecodent.CreateAndFake.AsserterTool.Categories;

#pragma warning disable CA1711 // Follows existing pattern.

/// <summary>Handles common delegate test scenarios.</summary>
public interface IAsserterDelegate
{
    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Delegate? behavior, string? details = null)
        where T : Exception;

    /// <summary>Verifies <paramref name="behavior"/> throws a <typeparamref name="T"/> exception.</summary>
    /// <typeparam name="T">Expected exception type.</typeparam>
    /// <param name="behavior">Delegate to run assertion checks with.</param>
    /// <inheritdoc cref="IAsserterObject.Is(object,object,AsserterMod,string)"/>
    T Throws<T>(Delegate? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsException(Delegate,AsserterMod,string)"/>
    Exception ThrowsException(Delegate? behavior, string? details = null);

    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    Exception ThrowsException(
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Delegate? behavior, string? details = null)
        where T : Exception;

    /// <summary>Verifies <c>behavior</c> does not throw a <typeparamref name="T"/> exception.</summary>
    /// <typeparam name="T">Expected missing exception type.</typeparam>
    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Delegate? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsNoException(Delegate,AsserterMod,string)"/>
    void ThrowsNoException(Delegate? behavior, string? details = null);

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNoException(
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="HasResult{T}(Delegate,AsserterMod,string)"/>
    T HasResult<T>(Delegate? behavior, string? details = null);

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
    ///     Verifies the <paramref name="behavior"/> successfully
    ///     executes with a resulting <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T">Expected return <see cref="Type"/> of the <paramref name="behavior"/>.</typeparam>
    /// <param name="content">Expected present element.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>Result from invoking the <paramref name="behavior"/>.</returns>
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
