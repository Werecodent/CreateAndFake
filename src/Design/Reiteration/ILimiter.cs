namespace CreateAndFake.Design.Reiteration;

/// <summary>Provides the core functionality for repetition.</summary>
public interface ILimiter
    : ITaskLimiter,
        IAsyncLimiter,
        ISyncLimiter,
        IEquatable<ILimiter>,
        IComparable<ILimiter>
{
    /// <summary>Calculates how long the <see cref="ILimiter"/> constrains to.</summary>
    /// <returns>The calculated duration.</returns>
    /// <remarks>An attempt is presumed to last 1 millisecond for calculations.</remarks>
    TimeSpan GetMaxDurationEstimate();
}
