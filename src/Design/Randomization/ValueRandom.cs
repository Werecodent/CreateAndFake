using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Properties;
using CreateAndFake.Design.Randomization.Handlers;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Randomization;

/// <inheritdoc cref="IRandom"/>
/// <param name="iterationLimit"><inheritdoc cref="IterationLimit" path="/summary"/></param>
/// <param name="onlyValidValues"><inheritdoc cref="OnlyValidValues" path="/summary"/></param>
public abstract class ValueRandom(
    int iterationLimit = DesignDefaults.IterationLimit,
    bool onlyValidValues = !DesignDefaults.IncludeInfinityAndNaNGeneration
) : IRandom
{
    /// <summary>Handlers for all the supported types.</summary>
    private static readonly IValueHandler[] _Handlers =
    [
        .. TimeValueHandlers.Handlers,
        new DecimalValueHandler(),
        new DoubleValueHandler(),
        new FloatValueHandler(),
        new BoolValueHandler(),
        new IntegralValueHandler<long>(8, BitConverter.ToInt64),
        new IntegralValueHandler<ulong>(8, BitConverter.ToUInt64),
        new IntegralValueHandler<int>(4, BitConverter.ToInt32),
        new IntegralValueHandler<uint>(4, BitConverter.ToUInt32),
        new IntegralValueHandler<short>(2, BitConverter.ToInt16),
        new IntegralValueHandler<ushort>(2, BitConverter.ToUInt16),
        new IntegralValueHandler<char>(2, BitConverter.ToChar),
        new IntegralValueHandler<byte>(1, (bytes, _) => bytes[0]),
        new IntegralValueHandler<sbyte>(1, (bytes, _) => (sbyte)bytes[0]),
    ];

    /// <summary>Supported types paired with the handler used to generate them.</summary>
    private static readonly IDictionary<Type, IValueHandler> _HandlersByType =
        TypeSupporter.GroupBySupportedType(_Handlers);

    /// <summary>All supported value types.</summary>
    public static IEnumerable<Type> SupportedTypes { get; } = _HandlersByType.Keys.ToFrozenSet();

    /// <summary>Max supported size for iterating uncollected sequences.</summary>
    protected int IterationLimit { get; } = iterationLimit;

    /// <inheritdoc/>
    public bool OnlyValidValues { get; } = onlyValidValues;

    /// <inheritdoc/>
    public abstract int? InitialSeed { get; }

    /// <inheritdoc/>
    public abstract byte[] NextBytes(short length);

    /// <inheritdoc/>
    public double NextPercent()
    {
        return (Next<ulong>() >> 11) * (1.0 / (1ul << 53));
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
        return (type != null) && SupportedTypes.Contains(type);
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
            throw new UnsupportedException(
                $"Type '{TypeHelper.ExpandedName(valueType)}' not supported."
            );
        }
    }

    /// <inheritdoc/>
    public T Next<T>(T max)
        where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        if (_HandlersByType.TryGetValue(typeof(T), out IValueHandler? gen))
        {
            T min = default;
            if (min.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(max),
                    max,
                    $"Must be greater than or equal to: '{min}'."
                );
            }
            else
            {
                T result = (T)gen.CreateSupported(this, max);

                // Prevent any issues stemming from scaling imprecision.
                return result.CompareTo(max) < 0 ? result : min;
            }
        }
        else
        {
            throw new UnsupportedException($"Type '{TypeHelper.ExpandedName<T>()}' not supported.");
        }
    }

    /// <inheritdoc/>
    public T Next<T>(T min, T max)
        where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        if (_HandlersByType.TryGetValue(typeof(T), out IValueHandler? gen))
        {
            if (min.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(max),
                    max,
                    $"Must be greater than or equal to the specified min: '{min}'."
                );
            }
            else
            {
                T result = (T)gen.CreateSupported(this, min, max);

                // Prevent any issues stemming from scaling imprecision.
                return result.CompareTo(max) <= 0 ? result : min;
            }
        }
        else
        {
            throw new UnsupportedException($"Type '{TypeHelper.ExpandedName<T>()}' not supported.");
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
            return NextSequence(items).First();
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
            return NextSequence(items).FirstOrDefault();
        }
    }

    /// <inheritdoc/>
    public DataRandom NextData()
    {
        return new(this);
    }

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(items))]
    public IEnumerable<T>? NextSequence<T>(IEnumerable<T>? items)
    {
        return items == null ? null : CapSize(items).OrderBy(_ => Next<int>());
    }

    /// <summary>Iterates <paramref name="items"/>.</summary>
    /// <typeparam name="T">The collection's item <see cref="Type"/>.</typeparam>
    /// <param name="items">Series to iterate.</param>
    /// <returns><paramref name="items"/></returns>
    /// <exception cref="IterationLimitException">
    ///     If the <paramref name="items"/> size is <c>&gt;= IterationLimit</c>.
    /// </exception>
    private IEnumerable<T> CapSize<T>(IEnumerable<T> items)
    {
        int i = 0;
        foreach (T item in items)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, IterationLimit);
            yield return item;
        }
    }
}
