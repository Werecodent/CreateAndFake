using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Design.Randomization;

/// <summary>Provides the core functionality for generic randomization.</summary>
[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords",
    MessageId = "Next", Justification = "Matches the Random convention.")]
public interface IRandom
{
    /// <summary>Initial seed used to begin generating values.</summary>
    int? InitialSeed { get; }

    /// <summary>Checks if <typeparamref name="T"/> can be used for <see cref="Next{T}()"/>.</summary>
    /// <typeparam name="T"><c>Type</c> to determine randomization support for.</typeparam>
    /// <returns><c>true</c> if <typeparamref name="T"/> is supported; <c>false</c> otherwise.</returns>
    bool Supports<T>() where T : struct, IComparable, IComparable<T>, IEquatable<T>;

    /// <summary>Checks if <paramref name="type"/> can be used for <see cref="Next(Type)"/>.</summary>
    /// <param name="type"><c>Type</c> to determine randomization support for.</param>
    /// <returns><c>true</c> if <paramref name="type"/> is supported; <c>false</c> otherwise.</returns>
    bool Supports([NotNullWhen(true)] Type? type);

    /// <summary>Generates a random <typeparamref name="T"/> value.</summary>
    /// <typeparam name="T">Value type to generate.</typeparam>
    /// <returns>The generated <typeparamref name="T"/> value.</returns>
    /// <exception cref="NotSupportedException">If <typeparamref name="T"/> isn't supported.</exception>
    T Next<T>() where T : struct, IComparable, IComparable<T>, IEquatable<T>;

    /// <summary>Generates a random <paramref name="valueType"/> value.</summary>
    /// <param name="valueType">Value type to generate.</param>
    /// <returns>The generated <paramref name="valueType"/> value.</returns>
    /// <exception cref="NotSupportedException">If <paramref name="valueType"/> isn't supported.</exception>
    object Next(Type valueType);

    /// <summary>Generates a positive constrained <typeparamref name="T"/> value.</summary>
    /// <typeparam name="T">Value type to generate.</typeparam>
    /// <param name="max">Positive exclusive upper boundary for the value.</param>
    /// <returns>The generated <typeparamref name="T"/> value &lt; <paramref name="max"/>.</returns>
    /// <exception cref="NotSupportedException">If <typeparamref name="T"/> isn't supported.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="max"/> &lt; <c>0</c>.</exception>
    T Next<T>(T max) where T : struct, IComparable, IComparable<T>, IEquatable<T>;

    /// <summary>Generates a constrained <typeparamref name="T"/> value.</summary>
    /// <typeparam name="T">Value type to generate.</typeparam>
    /// <param name="min">Inclusive lower boundary for the value.</param>
    /// <param name="max">Exclusive upper boundary for the value.</param>
    /// <returns>
    ///     The generated <typeparamref name="T"/> value &gt;= <paramref name="min"/> and &lt; <paramref name="max"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">If <typeparamref name="T"/> isn't supported.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     If <paramref name="max"/> &lt; <paramref name="min"/>.
    /// </exception>
    T Next<T>(T min, T max) where T : struct, IComparable, IComparable<T>, IEquatable<T>;

    /// <summary>Picks a random item from <paramref name="items"/>.</summary>
    /// <typeparam name="T">Type of items being picked from.</typeparam>
    /// <param name="items">Collection of items to pick from.</param>
    /// <returns>The picked item from <paramref name="items"/>.</returns>
    /// <exception cref="InvalidOperationException">If <paramref name="items"/> is <c>null</c> or empty.</exception>
    T NextItem<T>(IEnumerable<T> items);

    /// <inheritdoc cref="NextItem"/>
    /// <returns>The picked item from <paramref name="items"/> if any; default for the type otherwise.</returns>
    [return: MaybeNull, NotNullIfNotNull(nameof(items))]
    T NextItemOrDefault<T>(IEnumerable<T>? items);

    /// <summary>Generates a group of random predefined data.</summary>>
    /// <returns>The generated group of random predefined data.</returns>
    DataRandom NextData();
}
