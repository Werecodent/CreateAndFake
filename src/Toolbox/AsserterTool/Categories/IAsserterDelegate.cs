namespace CreateAndFake.AsserterTool.Categories;

#pragma warning disable CA1711 // Follows existing pattern.

/// <summary>Handles common delegate test scenarios.</summary>
public interface IAsserterDelegate
{
    /// <summary>Runs each case and aggregates exceptions.</summary>
    /// <param name="cases">Assert cases.</param>
    void CheckAll(params ICollection<Action> cases);

    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Action? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Action? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Delegate? behavior, string? details = null)
        where T : Exception;

    /// <summary>Verifies <paramref name="behavior"/> throws a <typeparamref name="T"/> exception.</summary>
    /// <typeparam name="T">Expected exception type.</typeparam>
    /// <param name="behavior">Delegate to run assertion checks with.</param>
    /// <inheritdoc cref="IAsserterObject.Is(object,object,AsserterMod,string)"/>
    T Throws<T>(Delegate? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Action? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Action? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Delegate? behavior, string? details = null)
        where T : Exception;

    /// <summary>Verifies <c>behavior</c> does not throw a <typeparamref name="T"/> exception.</summary>
    /// <typeparam name="T">Expected missing exception type.</typeparam>
    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Delegate? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="HasResult{T}(Delegate,AsserterMod,string)"/>
    T HasResult<T>(Delegate? behavior, string? details = null);

    /// <summary>
    ///     Verifies the <paramref name="behavior"/> successfully
    ///     executes with a resulting <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T">Return <see cref="Type"/> of the <paramref name="behavior"/>.</typeparam>
    /// <returns>Result from invoking the <paramref name="behavior"/>.</returns>
    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T HasResult<T>(Delegate? behavior, AsserterMod? optionConfiguration, string? details = null);
}

#pragma warning restore CA1711
