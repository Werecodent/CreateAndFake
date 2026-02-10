using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Design.Randomization;

#pragma warning disable CA1716 // Matches the Random convention.

/// <summary>Provides the core functionality for generic value randomization.</summary>
public interface IRandom
{
    /// <summary>First seed used to begin generating values if randomization is seeded.</summary>
    /// <remarks>Can be used to recreate the same sequence of values from randomization.</remarks>
    /// <seealso cref="SeededRandom"/>
    int? InitialSeed { get; }

    /// <summary>Flag to prevent generating invalid values (NaN, -∞ and +∞).</summary>
    bool OnlyValidValues { get; }

    /// <summary>
    ///     Checks if <typeparamref name="T"/> can be used for <see cref="Next{T}()"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to check randomization support for.</typeparam>
    /// <returns>
    ///     <see langword="true"/> if <typeparamref name="T"/> is supported,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    bool Supports<T>()
        where T : struct, IComparable, IComparable<T>, IEquatable<T>;

    /// <summary>
    ///     Checks if the <paramref name="type"/> can be used for <see cref="Next(Type)"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to check randomization support for.</param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="type"/> is supported,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    bool Supports([NotNullWhen(true)] Type? type);

    /// <summary>Generates a random <typeparamref name="T"/> value.</summary>
    /// <typeparam name="T">Value <see cref="Type"/> to generate.</typeparam>
    /// <returns>The generated <typeparamref name="T"/> value.</returns>
    /// <exception cref="NotSupportedException">
    ///     If <typeparamref name="T"/> isn't supported.
    /// </exception>
    T Next<T>()
        where T : struct, IComparable, IComparable<T>, IEquatable<T>;

    /// <summary>Generates a random <paramref name="valueType"/> value.</summary>
    /// <param name="valueType">Value <see cref="Type"/> to generate.</param>
    /// <returns>The generated <paramref name="valueType"/> value.</returns>
    /// <exception cref="NotSupportedException">
    ///     If <paramref name="valueType"/> isn't supported.
    /// </exception>
    object Next(Type valueType);

    /// <summary>Generates a positive constrained <typeparamref name="T"/> value.</summary>
    /// <typeparam name="T">Value <see cref="Type"/> to generate.</typeparam>
    /// <param name="max">Positive exclusive upper boundary for the value.</param>
    /// <returns>
    ///     The generated <typeparamref name="T"/> value <c>&lt;</c> <paramref name="max"/> and
    ///     <c>&gt;= 0</c>. If <paramref name="max"/> <c>== 0</c>, <c>0</c> is returned instead.
    /// </returns>
    /// <exception cref="NotSupportedException">
    ///     If <typeparamref name="T"/> isn't supported.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     If <paramref name="max"/> &lt; <c>0</c>.
    /// </exception>
    T Next<T>(T max)
        where T : struct, IComparable, IComparable<T>, IEquatable<T>;

    /// <summary>Generates a constrained <typeparamref name="T"/> value.</summary>
    /// <typeparam name="T">Value <see cref="Type"/> to generate.</typeparam>
    /// <param name="min">Inclusive lower boundary for the value.</param>
    /// <param name="max">Inclusive upper boundary for the value.</param>
    /// <returns>
    ///     The generated <typeparamref name="T"/> value <c>&lt;=</c>
    ///     <paramref name="max"/> and <c>&gt;=</c> <paramref name="min"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    ///     If <typeparamref name="T"/> isn't supported.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     If <paramref name="max"/> &lt; <paramref name="min"/>.
    /// </exception>
    T Next<T>(T min, T max)
        where T : struct, IComparable, IComparable<T>, IEquatable<T>;

    /// <summary>Generates a [0,1) value for scaling.</summary>
    /// <returns>The generated value <c>&gt;= 0</c> and <c>&lt; 1</c>.</returns>
    double NextPercent();

    /// <summary>Generates a <see langword="byte"/> array filled with random bytes.</summary>
    /// <param name="length">Length of the <see langword="byte"/> array to generate.</param>
    /// <returns>The generated <see langword="byte"/> array.</returns>
    byte[] NextBytes(short length);

    /// <returns>The picked item from <paramref name="items"/>.</returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="items"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <inheritdoc cref="NextItemOrDefault"/>
    T NextItem<T>(IEnumerable<T> items);

    /// <summary>Picks a random item from <paramref name="items"/>.</summary>
    /// <typeparam name="T"><see cref="Type"/> of the items being picked from.</typeparam>
    /// <param name="items">The collection of items to pick from.</param>
    /// <returns>
    ///     The picked item from <paramref name="items"/> if any exist,
    ///     <see langword="default"/> value for <typeparamref name="T"/> otherwise.
    /// </returns>
    T? NextItemOrDefault<T>(IEnumerable<T>? items);

    /// <summary>Generates random predefined data.</summary>>
    /// <returns>The generated group of random predefined data.</returns>
    DataRandom NextData();

    /// <summary>Generates a randomized sequence of the <paramref name="items"/>.</summary>
    /// <typeparam name="T"><see cref="Type"/> of the items being picked from.</typeparam>
    /// <param name="items">The collection of items to pick from.</param>
    /// <returns>The sequence of <paramref name="items"/> randomly ordered.</returns>
    [return: NotNullIfNotNull(nameof(items))]
    IEnumerable<T>? NextSequence<T>(IEnumerable<T>? items);
}

#pragma warning restore CA1716
