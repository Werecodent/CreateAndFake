using System.Collections;

namespace Werecodent.CreateAndFake.Design.Properties;

/// <summary>Handles default settings for the Design namespace.</summary>
internal static class DesignDefaults
{
    /// <summary>Cap to iterating <see cref="IEnumerable"/>s and loops.</summary>
    /// <remarks>Used to prevent stack overflows and infinite execution cycles.</remarks>
    /// <value><c>19999</c></value>
    internal const int IterationLimit = 19999;

    /// <summary>Flag to include generating invalid floating-point values (NaN, -∞ and +∞).</summary>
    /// <remarks>Only finite valid numbers are generated unless this flag is set to <see langword="true"/>.</remarks>
    /// <value><see langword="false"/></value>
    internal const bool IncludeInfinityAndNaNGeneration = false;
}
