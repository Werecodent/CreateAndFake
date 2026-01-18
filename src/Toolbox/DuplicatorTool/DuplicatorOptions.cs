using System.Collections.Frozen;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.ExtractorTool;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.DuplicatorTool;

/// <summary>Configuration for controlling duplication behavior.</summary>
public sealed record DuplicatorOptions : ToolHintOptions<DuplicatorOptions, CopyHint>
{
    /// <summary>Verifies duplicates are valid.</summary>
    public required IAsserter Asserter { get; init; }

    /// <summary>Finds contents for objects.</summary>
    public required IExtractor Extractor { get; init; }

    /// <summary>If results are verified via the <see cref="Asserter"/>.</summary>
    [ConfigurableOption]
    public bool VerifyCloneResult { get; init; } = true;

    /// <summary>Types that need no further inspection for serialization/deserialization.</summary>
    public FrozenSet<Type> SerializableTypes { get; init; } =
        FrozenSet.ToFrozenSet([typeof(string), typeof(Type)]);

    /// <summary>
    ///     Creates options from <see langword="this"/>
    ///     overridden with values from <paramref name="config"/>.
    /// </summary>
    /// <param name="config">Configuration with overrides to use.</param>
    /// <returns>The created options.</returns>
    internal DuplicatorOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(nameof(Duplicator));
        if (section == null)
        {
            return this;
        }

        return this with
        {
            VerifyCloneResult = section.GetValue(nameof(VerifyCloneResult), VerifyCloneResult),
            MaxHintRecursion = section.GetValue(nameof(MaxHintRecursion), MaxHintRecursion),
        };
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return ValueComparer.Use.GetHashCode(
            Asserter,
            Extractor,
            IncludeDefaultHints,
            Hints,
            VerifyCloneResult,
            SerializableTypes
        );
    }
}
