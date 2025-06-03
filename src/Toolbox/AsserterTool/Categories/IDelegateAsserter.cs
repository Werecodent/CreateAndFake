namespace CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common delegate test scenarios.</summary>
public interface IDelegateAsserter
{
    /// <summary>Runs each case and aggregates exceptions.</summary>
    /// <param name="cases">Assert cases.</param>
    void CheckAll(params IEnumerable<Action> cases);

    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Action? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Action? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Func<object?>? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Func<object?>? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    T Throws<T>(Delegate? behavior, string? details = null)
        where T : Exception;

    /// <summary>Verifies <paramref name="behavior"/> throws a <typeparamref name="T"/> exception.</summary>
    /// <typeparam name="T">Expected exception type.</typeparam>
    /// <param name="behavior">Delegate to run assertion checks with.</param>
    /// <inheritdoc cref="IObjectAsserter.Is(object,object,AsserterMod,string)"/>
    T Throws<T>(Delegate? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Action? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Action? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Func<object?>? behavior, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(
        Func<object?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Delegate? behavior, string? details = null)
        where T : Exception;

    /// <summary>Verifies <c>behavior</c> does not throw a <typeparamref name="T"/> exception.</summary>
    /// <typeparam name="T">Expected missing exception type.</typeparam>
    /// <inheritdoc cref="Throws{T}(Delegate,AsserterMod,string)"/>
    void ThrowsNo<T>(Delegate? behavior, AsserterMod? optionConfiguration, string? details = null)
        where T : Exception;
}
