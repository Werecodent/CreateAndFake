using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Design.Randomization;

/// <inheritdoc cref="IRandom"/>
/// <param name="onlyValidValues"><inheritdoc cref="OnlyValidValues" path="/summary"/></param>
public abstract class ValueRandom(bool onlyValidValues) : IRandom
{
    /// <summary>Supported types paired with the method used to generate them.</summary>
    private static readonly FrozenDictionary<Type, Func<ValueRandom, object>> _Gens
        = new Dictionary<Type, Func<ValueRandom, object>>()
        {
            { typeof(double), gen => Create(gen, BitConverter.ToDouble, 8,
                double.NaN, double.NegativeInfinity, double.PositiveInfinity) },
            { typeof(float), gen => Create(gen, BitConverter.ToSingle, 4,
                float.NaN, float.NegativeInfinity, float.PositiveInfinity) },

            { typeof(ushort), gen => Create(gen, BitConverter.ToUInt16, 2) },
            { typeof(ulong), gen => Create(gen, BitConverter.ToUInt64, 8) },
            { typeof(short), gen => Create(gen, BitConverter.ToInt16, 2) },
            { typeof(uint), gen => Create(gen, BitConverter.ToUInt32, 4) },
            { typeof(long), gen => Create(gen, BitConverter.ToInt64, 8) },
            { typeof(char), gen => Create(gen, BitConverter.ToChar, 2) },
            { typeof(int), gen => Create(gen, BitConverter.ToInt32, 4) },

            { typeof(byte), gen => gen.NextBytes(1)[0] },
            { typeof(sbyte), gen => (sbyte)gen.Next<byte>() },
            { typeof(bool), gen => gen.Next<byte>() > byte.MaxValue / 2 },

            { typeof(decimal), gen => new decimal(
                gen.Next<int>(), gen.Next<int>(), gen.Next<int>(), gen.Next<bool>(), gen.Next<byte>(29)) }
        }.ToFrozenDictionary();

    /// <summary>Generates a random <typeparamref name="T"/> value using random bytes.</summary>
    /// <typeparam name="T">Type to create.</typeparam>
    /// <param name="gen">Instance generating the random bytes.</param>
    /// <param name="converter">Behavior used to convert the bytes to <typeparamref name="T"/>.</param>
    /// <param name="size">Number of bytes required to generate <typeparamref name="T"/>.</param>
    /// <param name="invalids">Special invalid values for the type.</param>
    /// <returns>The generated <typeparamref name="T"/> value.</returns>
    private static T Create<T>(ValueRandom gen, Func<byte[], int, T> converter, short size, params T[] invalids)
    {
        T value;
        do
        {
            value = converter.Invoke(gen.NextBytes(size), 0);
        } while (gen.OnlyValidValues && invalids.Any(v => Equals(v, value)));
        return value;
    }

    /// <summary>All supported value types.</summary>
    public static IEnumerable<Type> ValueTypes { get; } = _Gens.Keys.ToFrozenSet();

    /// <summary>Flag to prevent generating invalid values (NaN, -∞ and +∞).</summary>
    public bool OnlyValidValues { get; set; } = onlyValidValues;

    /// <inheritdoc/>
    public abstract int? InitialSeed { get; }

    /// <summary>Generates a <c>byte</c> array filled with random bytes.</summary>
    /// <param name="length">Length of the <c>byte</c> array to generate.</param>
    /// <returns>The generated <c>byte</c> array.</returns>
    protected abstract byte[] NextBytes(short length);

    /// <inheritdoc/>
    public bool Supports<T>() where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        return Supports(typeof(T));
    }

    /// <inheritdoc/>
    public bool Supports([NotNullWhen(true)] Type? type)
    {
        return (type != null) && _Gens.ContainsKey(type);
    }

    /// <inheritdoc/>
    public T Next<T>() where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        return (T)Next(typeof(T));
    }

    /// <inheritdoc/>
    public object Next(Type valueType)
    {
        if (valueType != null && _Gens.TryGetValue(valueType, out Func<ValueRandom, object>? gen))
        {
            return gen.Invoke(this);
        }
        else
        {
            throw new NotSupportedException($"Type '{valueType?.Name}' not supported.");
        }
    }

    /// <inheritdoc/>
    public T Next<T>(T max) where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        return Next(default, max);
    }

    /// <inheritdoc/>
    public T Next<T>(T min, T max) where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        if (!Supports<T>())
        {
            throw new NotSupportedException($"Type '{typeof(T).Name}' not supported.");
        }
        else if (min.Equals(max))
        {
            return min;
        }
        else if (min.CompareTo(max) >= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(max), max,
                $"Value must be greater than given min: '{min}'.");
        }
        else if (typeof(T) == typeof(bool))
        {
            return default;
        }
        else
        {
            try
            {
                return CalcNext(min, max);
            }
            catch (ArithmeticException)
            {
                return StumbleNext(min, max);
            }
        }
    }

    /// <summary>Uses an algorithm to generate the next <typeparamref name="T"/> value.</summary>
    /// <inheritdoc cref="Next{T}(T,T)"/>
    private T CalcNext<T>(T min, T max) where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        dynamic percent;

        // Creates a number in the range of 0.0 to 1.0.
        if (typeof(T) != typeof(decimal))
        {
            percent = (double)Next<uint>() / uint.MaxValue;
        }
        else
        {
            percent = (decimal)Next<uint>() / uint.MaxValue;
        }

        checked
        {
            T value = (T)(percent * ((dynamic)max - min) + min);

            // Prevent any issues stemming from imprecision.
            return (value.CompareTo(max) < 0) ? value : min;
        }
    }

    /// <summary>Randoms until acceptable for the next <typeparamref name="T"/> value.</summary>
    /// <inheritdoc cref="Next{T}(T,T)"/>
    private T StumbleNext<T>(T min, T max) where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        T value;
        do
        {
            value = Next<T>();
        } while (value.CompareTo(min) < 0 || value.CompareTo(max) >= 0);
        return value;
    }

    /// <inheritdoc/>
    public T NextItem<T>(IEnumerable<T> items)
    {
        if (items == null)
        {
            throw new InvalidOperationException("The source sequence is empty.");
        }
        else if (items is ICollection<T> collection && collection.Count > 0)
        {
            return collection.ElementAt(Next(collection.Count));
        }
        else if (items is IReadOnlyCollection<T> readOnlyCollection && readOnlyCollection.Count > 0)
        {
            return readOnlyCollection.ElementAt(Next(readOnlyCollection.Count));
        }
        else
        {
            return items.OrderBy(i => Next<int>()).First();
        }
    }

    /// <inheritdoc/>
    [return: MaybeNull, NotNullIfNotNull(nameof(items))]
    public T NextItemOrDefault<T>(IEnumerable<T>? items)
    {
        if (items == null)
        {
            return default;
        }
        else if (items is ICollection<T> collection && collection.Count > 0)
        {
            return collection.ElementAt(Next(collection.Count))!;
        }
        else if (items is IReadOnlyCollection<T> readOnlyCollection && readOnlyCollection.Count > 0)
        {
            return readOnlyCollection.ElementAt(Next(readOnlyCollection.Count))!;
        }
        else
        {
            return items.OrderBy(i => Next<int>()).FirstOrDefault()!;
        }
    }

    /// <inheritdoc/>
    public DataRandom NextData()
    {
        return new(this);
    }
}
