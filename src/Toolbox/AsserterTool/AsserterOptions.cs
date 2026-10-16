using System.Reflection;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.AsserterTool;

/// <summary>Settings for manipulating <see cref="IAsserter"/> behavior.</summary>
public sealed record AsserterOptions : IToolOptions
{
    /// <summary>Identifier for the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = nameof(Asserter);

    /// <inheritdoc/>
    public required IRandom Gen { get; init; }

    /// <inheritdoc cref="IExtractor"/>
    public required IExtractor Extractor { get; init; }

    /// <inheritdoc cref="IValuer"/>
    public required IValuer Valuer { get; init; }

    /// <summary>Comparison method used for <see langword="string"/> assertions.</summary>
    /// <value><c>StringComparison.Ordinal</c></value>
    /// <remarks>Useful for specifying cultures or ignoring case.</remarks>
    [ConfigurableOption]
    public StringComparison ComparisonModeForStringAsserts { get; init; } =
        StringComparison.Ordinal;

    /// <summary>
    ///     Triggers all <see cref="Exception"/>s to be rethrown instead of successfully caught in Throw assertions.
    /// </summary>
    /// <remarks>Useful for looking at the <see cref="Exception"/>s being thrown during testing.</remarks>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool DisableCatchingForThrowAsserts { get; init; } = false;

    /// <summary>
    ///     Enables <see cref="Exception.InnerException"/> to satisfy the target condition
    ///     for Throw assertions if the caught <see cref="Exception"/> doesn't itself match.
    /// </summary>
    /// <remarks>
    ///     Helps test for the intended <see cref="Exception"/> without worrying about wrappers such
    ///     as <see cref="TargetInvocationException"/>. <see cref="AggregateException"/> is affected
    ///     only when not wrapping multiple <see cref="Exception"/>s. The behavior is not recursive,
    ///     only checks the first <see cref="Exception.InnerException"/>.
    /// </remarks>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeInternalExceptionForThrowAsserts { get; init; } = true;

    /// <summary>Causes Debug assertions to throw in order to fail the containing test.</summary>
    /// <remarks>
    ///     Useful for creating tests with data that can be manually reviewed
    ///     via the resulting <see cref="AssertException"/>'s contents, such
    ///     as the formatting of <see cref="object.ToString"/> implementations.
    /// </remarks>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool TriggerFailureForDebugAsserts { get; init; } = false;

    /// <summary>
    ///     Creates options from <see langword="this"/> overridden by values in the <paramref name="config"/>.
    /// </summary>
    /// <param name="config">Configuration with overrides to use.</param>
    /// <returns>The created options.</returns>
    internal AsserterOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
        if (section == null)
        {
            return this;
        }

        return this with
        {
            ComparisonModeForStringAsserts = Config.GetValue(
                section,
                ComparisonModeForStringAsserts
            ),
            DisableCatchingForThrowAsserts = Config.GetValue(
                section,
                DisableCatchingForThrowAsserts
            ),
            IncludeInternalExceptionForThrowAsserts = Config.GetValue(
                section,
                IncludeInternalExceptionForThrowAsserts
            ),
            TriggerFailureForDebugAsserts = Config.GetValue(section, TriggerFailureForDebugAsserts),
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(AsserterOptions);
    }
}
