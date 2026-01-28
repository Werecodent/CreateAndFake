using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Reiteration;

/// <inheritdoc cref="ILimiter"/>
/// <param name="timeout"><inheritdoc cref="_timeout" path="/summary"/></param>
/// <param name="tries"><inheritdoc cref="_tries" path="/summary"/></param>
/// <param name="delay"><inheritdoc cref="_delay" path="/summary"/></param>
[TypeConverter(typeof(LimiterTypeConverter))]
public sealed partial class Limiter(TimeSpan timeout, int tries, TimeSpan? delay = null)
    : ILimiter,
        IEquatable<Limiter>
{
    /// <summary>Instance set to 1 attempt.</summary>
    public static Limiter Once { get; } = new Limiter(1);

    /// <summary>Instance set to 5 attempts.</summary>
    public static Limiter Few { get; } = new Limiter(5);

    /// <summary>Instance set to 12 attempts.</summary>
    public static Limiter Dozen { get; } = new Limiter(12);

    /// <summary>Instance set to 20 attempts.</summary>
    public static Limiter Score { get; } = new Limiter(20);

    /// <summary>Instance set to 100 attempts.</summary>
    public static Limiter Hundred { get; } = new Limiter(100);

    /// <summary>Instance set to 10,000 attempts.</summary>
    public static Limiter Myriad { get; } = new Limiter(10000);

    /// <summary>Instance set to 25ms with a 1ms delay.</summary>
    public static Limiter Quick { get; } =
        new Limiter(new TimeSpan(0, 0, 0, 0, 25), new TimeSpan(0, 0, 0, 0, 1));

    /// <summary>Instance set to 0.5 seconds with a 20ms delay.</summary>
    public static Limiter Fast { get; } =
        new Limiter(new TimeSpan(0, 0, 0, 0, 500), new TimeSpan(0, 0, 0, 0, 20));

    /// <summary>Instance set to 5 seconds with a 200ms delay.</summary>
    public static Limiter Slow { get; } =
        new Limiter(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 0, 0, 200));

    /// <summary>Specific names that can be converted.</summary>
    private static readonly Dictionary<string, Limiter> _NamedLimiters = new()
    {
        { nameof(Once), Once },
        { nameof(Few), Few },
        { nameof(Dozen), Dozen },
        { nameof(Score), Score },
        { nameof(Hundred), Hundred },
        { nameof(Myriad), Myriad },
        { nameof(Quick), Quick },
        { nameof(Fast), Fast },
        { nameof(Slow), Slow },
    };

    /// <summary>Specific limiters with their name.</summary>
    private static readonly Dictionary<Limiter, string> _LimiterNames = _NamedLimiters.ToDictionary(
        pair => pair.Value,
        pair => pair.Key
    );

    /// <summary>Maximum attempts to try.</summary>
    private readonly int _tries =
        (tries > 0)
            ? tries
            : throw new ArgumentOutOfRangeException(
                nameof(tries),
                tries,
                "Minimum number of attempts is 1."
            );

    /// <summary>Maximum duration to attempt for.</summary>
    private readonly TimeSpan _timeout =
        (timeout > TimeSpan.Zero)
            ? timeout
            : throw new ArgumentOutOfRangeException(
                nameof(timeout),
                timeout,
                "Duration must be greater than 0."
            );

    /// <summary>Delay between attempts; no delay by default.</summary>
    private readonly TimeSpan _delay =
        (delay == null || delay >= TimeSpan.Zero)
            ? delay ?? TimeSpan.Zero
            : throw new ArgumentOutOfRangeException(nameof(delay), delay, "Minimum delay is 0.");

    /// <inheritdoc cref="Limiter(TimeSpan,int,TimeSpan?)"/>
    public Limiter(int tries, TimeSpan? delay = null)
        : this(TimeSpan.MaxValue, tries, delay) { }

    /// <inheritdoc cref="Limiter(TimeSpan,int,TimeSpan?)"/>
    public Limiter(TimeSpan timeout, TimeSpan? delay = null)
        : this(timeout, int.MaxValue, delay) { }

    /// <inheritdoc/>
    public TimeSpan GetMaxDurationEstimate()
    {
        double duration = _delay.TotalMilliseconds + 1;
        double timeTries = Math.Floor(_timeout.TotalMilliseconds / duration) + 1;

        double result;
        if (timeTries > _tries)
        {
            result = (timeTries - 1) * duration + 1;
        }
        else
        {
            result = duration * _tries - _delay.TotalMilliseconds;
        }

        return TimeSpan.FromMilliseconds(Math.Min(result, TimeSpan.MaxValue.TotalMilliseconds));
    }

    /// <summary>Compares <see langword="this"/> to <paramref name="obj"/> by value.</summary>
    /// <param name="obj">Instance to compare <see langword="this"/> with.</param>
    /// <returns>
    ///     <see langword="true"/> if <see langword="this"/> is equal to <paramref name="obj"/> by value;
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as Limiter);
    }

    /// <inheritdoc/>
    public bool Equals(ILimiter? other)
    {
        return Equals(other as Limiter);
    }

    /// <inheritdoc cref="IValueEquatable.ValuesEqual"/>
    public bool Equals(Limiter? other)
    {
        return other is not null
            && other._delay == _delay
            && other._tries == _tries
            && other._timeout == _timeout;
    }

    /// <inheritdoc/>
    public int CompareTo(ILimiter? other)
    {
        return GetMaxDurationEstimate().CompareTo(other?.GetMaxDurationEstimate() ?? TimeSpan.Zero);
    }

    /// <inheritdoc cref="IValueEquatable.GetValueHash"/>
    public override int GetHashCode()
    {
        return ValueComparer.Use.GetHashCode(_tries, _timeout, _delay);
    }

    /// <summary>Converts <see langword="this"/> to a <see langword="string"/>.</summary>
    /// <returns><see langword="string"/> representation of <see langword="this"/>.</returns>
    public override string ToString()
    {
        if (_LimiterNames.TryGetValue(this, out string? name))
        {
            return name;
        }

        if (_delay == TimeSpan.Zero)
        {
            if (_timeout == TimeSpan.MaxValue)
            {
                return $"{_tries}";
            }
            else if (_tries == int.MaxValue)
            {
                return $"{_timeout}";
            }
        }
        return $"{_tries}-{_timeout}-{_delay}";
    }

    /// <summary>
    ///     Creates a <see cref="Limiter"/> from its <see langword="string"/> representation.
    /// </summary>
    /// <param name="data"><see langword="string"/> representation to convert from.</param>
    /// <param name="culture">Culture used for conversion.</param>
    /// <returns>The created instance.</returns>
    /// <exception cref="FormatException">When <paramref name="data"/> is not valid.</exception>
    public static Limiter ConvertFrom(string data, CultureInfo? culture)
    {
        if (_NamedLimiters.TryGetValue(data, out Limiter? named))
        {
            return named;
        }
        else if (int.TryParse(data, NumberStyles.Any, culture, out int justTries))
        {
            return new Limiter(justTries);
        }
        else if (TimeSpan.TryParse(data, culture, out TimeSpan justTimeout))
        {
            return new Limiter(justTimeout);
        }
        else
        {
            string[] parts = data?.Split('-') ?? [];
            if (
                parts.Length == 3
                && int.TryParse(parts[0], NumberStyles.Any, culture, out int tries)
                && TimeSpan.TryParse(parts[1], culture, out TimeSpan timeout)
                && TimeSpan.TryParse(parts[2], culture, out TimeSpan delay)
            )
            {
                return new Limiter(timeout, tries, delay);
            }
            else
            {
                throw new FormatException($"Invalid format for {nameof(Limiter)}: {data}");
            }
        }
    }

    /// <summary>Throws a <see cref="TimeoutException"/>.</summary>
    /// <param name="error">Issue causing the <see cref="TimeoutException"/>.</param>
    /// <param name="message">Details to include in the <see cref="TimeoutException"/>.</param>
    /// <param name="ex">Encountered exception causing the <see cref="TimeoutException"/>.</param>
    /// <exception cref="TimeoutException">With the error and message details.</exception>
    [DoesNotReturn]
    private void Fault(string error, string message, Exception? ex = null)
    {
        string details =
            $" with Limiter '{ToString()}'"
            + (string.IsNullOrWhiteSpace(message) ? "." : $": {message}");
        if (ex != null)
        {
            throw new TimeoutException(error + details, ex);
        }
        else
        {
            throw new TimeoutException(error + details);
        }
    }

    /// <summary>Throws a <see cref="OperationCanceledException"/>.</summary>
    /// <param name="error">Issue causing the <see cref="OperationCanceledException"/>.</param>
    /// <param name="message">Details to include in the <see cref="OperationCanceledException"/>.</param>
    /// <param name="ex">Encountered exception causing the <see cref="OperationCanceledException"/>.</param>
    /// <exception cref="OperationCanceledException">With the error and message details.</exception>
    [DoesNotReturn]
    private void CancelFault(string error, string message, Exception? ex = null)
    {
        string details =
            $" with Limiter '{ToString()}'"
            + (string.IsNullOrWhiteSpace(message) ? "." : $": {message}");
        if (ex != null)
        {
            throw new OperationCanceledException(error + details, ex);
        }
        else
        {
            throw new OperationCanceledException(error + details);
        }
    }

    /// <summary>Compares <paramref name="left"/> to <paramref name="right"/> by value.</summary>
    /// <param name="left">Instance to compare against.</param>
    /// <param name="right">Instance to compare with.</param>
    /// <returns>True if equal, false otherwise.</returns>
    public static bool operator ==(Limiter? left, Limiter? right)
    {
        return left is null ? right is null : left.Equals(right);
    }

    /// <returns>True if not equal, false otherwise.</returns>
    /// <inheritdoc cref="operator =="/>
    public static bool operator !=(Limiter? left, Limiter? right)
    {
        return !(left == right);
    }

    /// <returns>True if less than, false otherwise.</returns>
    /// <inheritdoc cref="operator =="/>
    public static bool operator <(Limiter? left, Limiter? right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    /// <returns>True if less than or equal, false otherwise.</returns>
    /// <inheritdoc cref="operator =="/>
    public static bool operator <=(Limiter? left, Limiter? right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    /// <returns>True if greater than, false otherwise.</returns>
    /// <inheritdoc cref="operator =="/>
    public static bool operator >(Limiter? left, Limiter? right)
    {
        return left?.CompareTo(right) > 0;
    }

    /// <returns>True if greater than or equal, false otherwise.</returns>
    /// <inheritdoc cref="operator =="/>
    public static bool operator >=(Limiter? left, Limiter? right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }
}
