using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Randomization;

/// <inheritdoc cref="IRandom"/>
/// <param name="onlyValidValues"><inheritdoc cref="OnlyValidValues" path="/summary"/></param>
public abstract class ValueRandom(bool onlyValidValues) : IRandom
{
    /// <summary>Abnormal double values to potentially exclude.</summary>
    private static readonly FrozenSet<double> _SpecialDoubles = FrozenSet.Create(
        double.NaN,
        double.NegativeInfinity,
        double.PositiveInfinity
    );

    /// <summary>Abnormal float values to potentially exclude.</summary>
    private static readonly FrozenSet<float> _SpecialFloats = FrozenSet.Create(
        float.NaN,
        float.NegativeInfinity,
        float.PositiveInfinity
    );

    /// <summary>Handlers for all the supported types.</summary>
    private static readonly IValueHandler[] _Handlers =
    [
        new ValueHandler<ushort>(
            gen => BitConverter.ToUInt16(gen.NextBytes(2), 0),
            (min, max, percent) => (ushort)Math.Floor(percent * (max - min) + min)
        ),
        new ValueHandler<ulong>(
            gen => BitConverter.ToUInt64(gen.NextBytes(8), 0),
            (min, max, percent) => (ulong)(percent * (max - min) + min)
        ),
        new ValueHandler<short>(
            gen => BitConverter.ToInt16(gen.NextBytes(2), 0),
            (min, max, percent) => (short)Math.Floor(percent * (max * 1.0 - min) + min)
        ),
        new ValueHandler<uint>(
            gen => BitConverter.ToUInt32(gen.NextBytes(4), 0),
            (min, max, percent) => (uint)(percent * (max - min) + min)
        ),
        new ValueHandler<long>(
            gen => BitConverter.ToInt64(gen.NextBytes(8), 0),
            (min, max, percent) => (long)Math.Floor(percent * (max * 1.0 - min) + min)
        ),
        new ValueHandler<char>(
            gen => BitConverter.ToChar(gen.NextBytes(2), 0),
            (min, max, percent) => (char)(percent * (max - min) + min)
        ),
        new ValueHandler<int>(
            gen => BitConverter.ToInt32(gen.NextBytes(4), 0),
            (min, max, percent) => (int)Math.Floor(percent * (max * 1.0 - min) + min)
        ),
        new ValueHandler<byte>(
            gen => gen.NextBytes(1)[0],
            (min, max, percent) => (byte)(percent * (max - min) + min)
        ),
        new ValueHandler<sbyte>(
            gen => (sbyte)gen.NextBytes(1)[0],
            (min, max, percent) => (sbyte)Math.Floor(percent * (max * 1.0 - min) + min)
        ),
        new ValueHandler<bool>(
            gen => gen.NextBytes(1)[0] > byte.MaxValue / 2,
            (_, __, ___) => default
        ),
        new ValueHandler<decimal>(
            gen => new decimal(
                gen.Next<int>(),
                gen.Next<int>(),
                gen.Next<int>(),
                gen.Next<bool>(),
                gen.Next<byte>(29)
            ),
            (min, max, percent) =>
            {
                decimal result = max * (decimal)percent + min * (1 - (decimal)percent);
                return result.CompareTo(min) > 0 ? result : min;
            }
        ),
        new ValueHandler<double>(
            gen =>
            {
                double value;
                do
                {
                    value = BitConverter.ToDouble(gen.NextBytes(8), 0);
                } while (gen.OnlyValidValues && _SpecialDoubles.Contains(value));
                return value;
            },
            (min, max, percent) => max * percent + min * (1 - percent)
        ),
        new ValueHandler<float>(
            gen =>
            {
                float value;
                do
                {
                    value = BitConverter.ToSingle(gen.NextBytes(4), 0);
                } while (gen.OnlyValidValues && _SpecialFloats.Contains(value));
                return value;
            },
            (min, max, percent) => (float)(max * percent + min * (1 - percent))
        ),
    ];

    /// <summary>Supported types paired with the handler used to generate them.</summary>
    private static readonly IDictionary<Type, IValueHandler> _HandlersByType =
        TypeSupporter.GroupBySupportedType(_Handlers);

    /// <summary>All supported value types.</summary>
    public static IEnumerable<Type> ValueTypes { get; } = _HandlersByType.Keys.ToFrozenSet();

    /// <summary>Flag to prevent generating invalid values (NaN, -∞ and +∞).</summary>
    public bool OnlyValidValues { get; } = onlyValidValues;

    /// <inheritdoc/>
    public abstract int? InitialSeed { get; }

    /// <summary>Generates a <see langword="byte"/> array filled with random bytes.</summary>
    /// <param name="length">Length of the <see langword="byte"/> array to generate.</param>
    /// <returns>The generated <see langword="byte"/> array.</returns>
    public abstract byte[] NextBytes(short length);

    /// <summary>Generates a [0,1) value for scaling.</summary>
    /// <returns>The generated value <c>&gt;= 0</c> and <c>&lt; 1</c>.</returns>
    public double NextPercent()
    {
        return (Next<ulong>() >> 11) * (1.0 / (1ul << 53)); // * (1.0 / (int.MaxValue - 1));
    }

    /// <inheritdoc/>
    public bool Supports<T>()
        where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        return Supports(typeof(T));
    }

    /// <inheritdoc/>
    public bool Supports([NotNullWhen(true)] Type? type)
    {
        return (type != null) && ValueTypes.Contains(type);
    }

    /// <inheritdoc/>
    public T Next<T>()
        where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        return (T)Next(typeof(T));
    }

    /// <inheritdoc/>
    public object Next(Type valueType)
    {
        if (valueType != null && _HandlersByType.TryGetValue(valueType, out IValueHandler? gen))
        {
            return gen.CreateSupported(this);
        }
        else
        {
            throw new NotSupportedException($"Type '{valueType?.Name}' not supported.");
        }
    }

    /// <inheritdoc/>
    public T Next<T>(T max)
        where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        return Next(default, max);
    }

    /// <inheritdoc/>
    public T Next<T>(T min, T max)
        where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        if (_HandlersByType.TryGetValue(typeof(T), out IValueHandler? gen))
        {
            if (min.Equals(max))
            {
                return min;
            }
            else if (min.CompareTo(max) >= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(max),
                    max,
                    $"Value must be greater than the specified min: '{min}'."
                );
            }
            else
            {
                T result = (T)gen.CreateSupported(min, max, NextPercent());

                // Prevent any issues stemming from scaling imprecision.
                return result.CompareTo(max) < 0 ? result : min;
            }
        }
        else
        {
            throw new NotSupportedException($"Type '{typeof(T).Name}' not supported.");
        }
    }

    /// <inheritdoc/>
    public T NextItem<T>(IEnumerable<T> items)
    {
        ArgumentGuard.ThrowIfNull(items);

        if (items is ICollection<T> collection && collection.Count > 0)
        {
            return collection.ElementAt(Next(collection.Count));
        }
        else if (items is IReadOnlyCollection<T> readOnlyCollection && readOnlyCollection.Count > 0)
        {
            return readOnlyCollection.ElementAt(Next(readOnlyCollection.Count));
        }
        else
        {
            return items.OrderBy(_ => Next<int>()).First();
        }
    }

    /// <inheritdoc/>
    public T? NextItemOrDefault<T>(IEnumerable<T>? items)
    {
        if (items == null)
        {
            return default;
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
            return items.OrderBy(_ => Next<int>()).FirstOrDefault();
        }
    }

    /// <inheritdoc/>
    public DataRandom NextData()
    {
        return new(this);
    }
}
