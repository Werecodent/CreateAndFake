using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.Design.Types;
using CreateAndFake.RunnerTool;
using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.TesterTool.Validators;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
internal sealed class SupportValidator(TesterOptions options)
{
    /// <inheritdoc/>
    internal TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public async Task ValidateRandomDataParametersAsync(
        Assembly testAssembly,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(testAssembly);

        IEnumerable<MethodInfo> testMethods = ScopeChecker
            .FindLoadedTypes(testAssembly)
            .Where(t => !t.IsGenericType)
            .SelectMany(t =>
                t.GetMethods(
                    BindingFlags.NonPublic
                        | BindingFlags.Public
                        | BindingFlags.Static
                        | BindingFlags.Instance
                        | BindingFlags.FlattenHierarchy
                )
            )
            .Where(m =>
                !m.IsGenericMethod && m.GetCustomAttributes(true).Any(a => a is IRandomDataMarker)
            );

        foreach (MethodInfo method in testMethods)
        {
            MethodCallWrapper? data = null;
            try
            {
                data = Options.Runner.CreateFor(method, canceler);
                foreach (object? item in data.Args)
                {
                    _ = Options.TestDisplayNameConverter.Invoke(item);
                }
            }
            catch (Exception e)
            {
                Options.Asserter.Fail(e, $"Randomization failed for method '{method}'");
            }
            finally
            {
                await Disposer.CleanupAsync(data?.Args).ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc/>
    public Task VerifyToolSetIntegrityAsync(CancellationToken canceler)
    {
        return VerifyToolSetSupportAsync(
            Enumerable
                .Empty<Type>()
                .Concat(Tools.Randomizer.SupportedTypes)
                .Concat(Tools.Duplicator.SupportedTypes)
                .Concat(Tools.Extractor.SupportedTypes)
                .Concat(Tools.Mutator.SupportedTypes)
                .Concat(Tools.Valuer.SupportedTypes),
            canceler
        );
    }

    /// <inheritdoc/>
    public async Task VerifyToolSetSupportAsync(IEnumerable<Type> types, CancellationToken canceler)
    {
        ArgumentGuard.ThrowIfNull(types);

        Type[] testTypes =
        [
            .. types.Where(t => !Options.IntegrityIgnorableTypes.Contains(t)).Distinct(),
        ];

        Dictionary<Type, Exception> failures = [];
        for (int i = 0; i < testTypes.Length; i++)
        {
            try
            {
                await VerifyToolSetSupportAsync(testTypes[i], canceler).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                failures.Add(testTypes[i], e);
            }
        }
        Options.Asserter.IsEmpty(failures, "Not all types were supported as expected.");
    }

    /// <summary>Validates the <paramref name="type"/> is fully compatible with the framework as configured.</summary>
    /// <param name="type">The <see cref="Type"/> to test with.</param>
    /// <inheritdoc cref="VerifyToolSetSupportAsync(IEnumerable{Type},CancellationToken)"/>
    private async Task VerifyToolSetSupportAsync(Type type, CancellationToken canceler)
    {
        object? original = null,
            variant = null,
            dupe = null;
        try
        {
            original = Options.Randomizer.Create(type);
            dupe = Options.Duplicator.Copy(original);

            string failMessage =
                "Behavior did not work for type '"
                + GenericTypeConverter.ExpandedName(type)
                + $"' randomized to '{GenericTypeConverter.ExpandedName(original)}'.";

            await Options
                .Asserter.ValuesEqualAsync(
                    original,
                    dupe,
                    canceler,
                    failMessage + " Cloned data was not equal."
                )
                .ConfigureAwait(false);

            if (
                type.IsAbstract
                || TypeDescriber.For(type).IsMutable()
                || TypeDescriber.For(type).HasInitializableOnlyState()
                || (!type.IsSealed && TypeDescriber.For(type).FindLoadedSubclasses().Skip(1).Any())
            )
            {
                variant = Options.Mutator.Variant(type, original);

                await Options
                    .Asserter.ValuesNotEqualAsync(
                        original,
                        variant,
                        canceler,
                        failMessage + " Variant data was still equal."
                    )
                    .ConfigureAwait(false);
            }

            if (Options.Mutator.Modify(original))
            {
                await Options
                    .Asserter.ValuesNotEqualAsync(
                        dupe,
                        original,
                        canceler,
                        failMessage + " Modified data was still equal."
                    )
                    .ConfigureAwait(false);
            }

            if (
                Options.Faker.Supports(type)
                && !type.Inherits<IDisposable>()
                && !type.Inherits<IToolOptions>()
            )
            {
                _ = Options.Faker.Mock(type);
            }
        }
        finally
        {
            await Disposer.CleanupAsync(original, variant, dupe).ConfigureAwait(false);
        }
    }
}
