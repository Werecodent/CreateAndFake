using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.Properties;

/// <summary>Handles deserializing configuration values.</summary>
internal static class Config
{
    /// <summary>Loads a configured value from the <paramref name="section"/>.</summary>
    /// <typeparam name="T"><see cref="Type"/> of the value to retrieve.</typeparam>
    /// <param name="section">Section potentially containing the value.</param>
    /// <param name="property">Property to contain the value.</param>
    /// <param name="name">Configuration name for the value to retrieve.</param>
    /// <returns>
    ///     The deserialized value if present, the <paramref name="property"/> otherwise.
    /// </returns>
    internal static T GetValue<T>(
        IConfigurationSection section,
        T property,
        [CallerArgumentExpression(nameof(property))] string? name = null
    )
    {
        ArgumentGuard.ThrowIfNull(name, section, property);
        return section.GetValue(name, property);
    }

    /// <returns>
    ///     The deserialized collection if present, the <paramref name="property"/> otherwise.
    /// </returns>
    /// <inheritdoc cref="GetList"/>
    /// <inheritdoc cref="GetArray"/>
    internal static FrozenSet<T> GetSet<T>(
        IConfigurationSection section,
        FrozenSet<T> property,
        [CallerArgumentExpression(nameof(property))] string? name = null
    )
    {
        return GetList<T>(section, name)?.ToFrozenSet() ?? property;
    }

    /// <inheritdoc cref="GetSet"/>
    internal static ImmutableArray<T> GetArray<T>(
        IConfigurationSection section,
        ImmutableArray<T> property,
        [CallerArgumentExpression(nameof(property))] string? name = null
    )
    {
        return GetList<T>(section, name)?.ToImmutableArray() ?? property;
    }

    /// <inheritdoc cref="GetSet"/>
    internal static FrozenSet<char> GetChars(
        IConfigurationSection section,
        FrozenSet<char> property,
        [CallerArgumentExpression(nameof(property))] string? name = null
    )
    {
        ArgumentGuard.ThrowIfNull(property);

        return GetValue(section, string.Join("", property), name)
            .ToCharArray()
            .Distinct()
            .ToFrozenSet();
    }

    /// <summary>Deserializes a collection from the configuration.</summary>
    /// <typeparam name="T">Item <see cref="Type"/> for the collection.</typeparam>
    /// <param name="config">Root configuration section for the options.</param>
    /// <param name="sectionName">Name of the subsection representing the collection.</param>
    /// <returns>The deserialized collection if present, <see langword="null"/> otherwise.</returns>
    private static List<T>? GetList<T>(IConfigurationSection config, string? sectionName)
    {
        ArgumentGuard.ThrowIfNull(sectionName, config);

        IConfigurationSection section = config.GetSection(sectionName);
        if (section.Exists())
        {
            List<T> bindResult = [];
            section.Bind(bindResult);
            return bindResult;
        }
        else
        {
            return null;
        }
    }
}
