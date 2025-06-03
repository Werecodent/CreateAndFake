using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Reiteration;

/// <inheritdoc cref="ILimiter"/>
/// <param name="timeout"><inheritdoc cref="_timeout" path="/summary"/></param>
/// <param name="tries"><inheritdoc cref="_tries" path="/summary"/></param>
/// <param name="delay"><inheritdoc cref="_delay" path="/summary"/></param>
public sealed partial class Limiter(TimeSpan timeout, int tries, TimeSpan? delay = null)
    : ILimiter,
        IEquatable<Limiter>
{
    /// <summary>Instance that defaults to a single attempt.</summary>
    public static Limiter Once { get; } = new Limiter(1);

    /// <summary>Instance that defaults to five attempts.</summary>
    public static Limiter Few { get; } = new Limiter(5);

    /// <summary>Instance that defaults to a dozen attempts.</summary>
    public static Limiter Dozen { get; } = new Limiter(12);

    /// <summary>Instance that defaults to a twenty attempts.</summary>
    public static Limiter Score { get; } = new Limiter(20);

    /// <summary>Instance that defaults to a hundred attempts.</summary>
    public static Limiter Hundred { get; } = new Limiter(100);

    /// <summary>Instance that defaults to a thousand attempts.</summary>
    public static Limiter Myriad { get; } = new Limiter(1000);

    /// <summary>Instance that defaults to 25 ms with a minimal delay.</summary>
    public static Limiter Quick { get; } =
        new Limiter(new TimeSpan(0, 0, 0, 0, 25), new TimeSpan(0, 0, 0, 0, 1));

    /// <summary>Instance that defaults to half a second with a small delay.</summary>
    public static Limiter Fast { get; } =
        new Limiter(new TimeSpan(0, 0, 0, 0, 500), new TimeSpan(0, 0, 0, 0, 20));

    /// <summary>Instance that defaults to five seconds with a large delay.</summary>
    public static Limiter Slow { get; } =
        new Limiter(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 0, 0, 200));

    /// <summary>Maximum attempts to try.</summary>
    private readonly int _tries = tries;

    /// <summary>Maximum duration to attempt for.</summary>
    private readonly TimeSpan _timeout = timeout;

    /// <summary>Delay between attempts.</summary>
    private readonly TimeSpan _delay = delay ?? TimeSpan.Zero;

    /// <inheritdoc cref="Limiter(TimeSpan,int,TimeSpan?)"/>
    public Limiter(int tries, TimeSpan? delay = null)
        : this(TimeSpan.MaxValue, tries, delay) { }

    /// <inheritdoc cref="Limiter(TimeSpan,int,TimeSpan?)"/>
    public Limiter(TimeSpan timeout, TimeSpan? delay = null)
        : this(timeout, int.MaxValue, delay) { }

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

    /// <inheritdoc cref="IValueEquatable.ValuesEqual"/>
    public bool Equals(Limiter? other)
    {
        return other != null
            && other._delay == _delay
            && other._tries == _tries
            && other._timeout == _timeout;
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
        return $"{_tries}-{_timeout}-{_delay}";
    }

    /// <summary>Throws a <see cref="TimeoutException"/>.</summary>
    /// <param name="error">Issue causing the <see cref="TimeoutException"/>.</param>
    /// <param name="message">Details to include in the <see cref="TimeoutException"/>.</param>
    /// <param name="ex">Encountered exception causing the <see cref="TimeoutException"/>.</param>
    /// <exception cref="TimeoutException">With the error and message details.</exception>
    [DoesNotReturn]
    private static void Fault(string error, string message, Exception? ex = null)
    {
        string details = string.IsNullOrWhiteSpace(message) ? "." : $": {message}";
        if (ex != null)
        {
            throw new TimeoutException(error + details, ex);
        }
        else
        {
            throw new TimeoutException(error + details);
        }
    }
}
