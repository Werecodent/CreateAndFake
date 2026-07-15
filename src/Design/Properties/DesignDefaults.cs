using System.Collections;

namespace CreateAndFake.Design.Properties;

/// <summary>Handles default settings for the Design namespace.</summary>
internal static class DesignDefaults
{
    /// <summary>Cap to iterating <see cref="IEnumerable"/>s and loops.</summary>
    internal const int IterationLimit = 5000;

    /// <summary>
    ///     Flag to include generating invalid floating-point values (NaN, -∞ and +∞).
    /// </summary>
    internal const bool IncludeInfinityAndNaNGeneration = false;
}
