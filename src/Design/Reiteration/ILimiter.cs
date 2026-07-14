namespace CreateAndFake.Design.Reiteration;

/// <summary>Provides the core functionality for repetition.</summary>
public interface ILimiter
    : ILimiterAsync,
        ILimiterSync,
        ILimiterTask,
        IEquatable<ILimiter>,
        IComparable<ILimiter>
{
    /// <summary>Calculates how long the <see cref="ILimiter"/> constrains to.</summary>
    /// <param name="millisecondsPerAttempt">How long an attempt is presumed to last. Defaults to 1.</param>
    /// <returns>The calculated duration.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="millisecondsPerAttempt"/> is <c>&lt;= 0</c>.</exception>
    TimeSpan GetMaxDurationEstimate(int millisecondsPerAttempt = 1);
}
