using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.TesterTool.Validators;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
internal sealed class SupportValidator(TesterOptions options)
{
    /// <inheritdoc cref="Tester.Options"/>
    internal TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc cref="ITester.VerifyAllToStrings"/>
    public void VerifyAllToStrings(Assembly codeAssembly)
    {
        ArgumentGuard.ThrowIfNull(codeAssembly);

        Options.Asserter.Debug(
            ScopeChecker
                .FindLoadedSpecificTypes(codeAssembly)
                .Where(t => !t.IsAbstract)
                .Where(t => ScopeChecker.IsVisible(t, Assembly.GetExecutingAssembly().GetName()))
                .ToDictionary(
                    GenericConverter.ExpandName,
                    t =>
                    {
                        try
                        {
                            return Options.Randomizer.Create(t);
                        }
                        catch (ToolException)
                        {
                            return null;
                        }
                    }
                )
        );
    }

    /// <inheritdoc cref="ITester.ValidateRandomDataParametersAsync"/>
    public async Task ValidateRandomDataParametersAsync(
        Assembly testAssembly,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(testAssembly, canceler);

        IEnumerable<MethodInfo> testMethods = ScopeChecker
            .FindLoadedTypes(testAssembly)
            .Where(t => !t.IsGenericType)
            .Select(TypeDescriber.For)
            .SelectMany(d => d.Methods.PublicOrInternal.Concat(d.StaticMethods.PublicOrInternal))
            .Where(m =>
                !m.IsGenericMethod && m.GetCustomAttributes(true).Any(a => a is IRandomDataMarker)
            );

        List<string> generatedData = [];

        foreach (MethodInfo method in testMethods.OrderBy(m => m.Name))
        {
            MethodCallWrapper? data = null;
            try
            {
                data = await Options.Runner.CreateForAsync(method, canceler).ConfigureAwait(false);
                foreach (object? item in data.Args)
                {
                    _ = Options.TestDisplayNameConverter.Invoke(item);
                }
                generatedData.Add(data.ToString());
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

        Options.Asserter.Debug(generatedData, "All generated data for random data parameters.");
    }

    /// <inheritdoc cref="ITester.VerifyToolSetIntegrityAsync"/>
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

    /// <inheritdoc cref="ITester.VerifyToolSetSupportAsync(IEnumerable{Type},CancellationToken,TesterMod)"/>
    public async Task VerifyToolSetSupportAsync(IEnumerable<Type> types, CancellationToken canceler)
    {
        ArgumentGuard.ThrowIfNull(types, canceler);

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
    public async Task VerifyToolSetSupportAsync(Type type, CancellationToken canceler)
    {
        ArgumentGuard.ThrowIfNull(type, canceler);

        object? original = null,
            variant = null,
            dupe = null;
        try
        {
            original = Options.Randomizer.Create(type);
            if (original is IFaked)
            {
                return;
            }

            dupe = Options.Duplicator.Copy(original);

            string failMessage =
                "Behavior did not work for type '"
                + GenericConverter.ExpandName(type)
                + $"' randomized to '{GenericConverter.ExpandName(original)}'.";

            await Options
                .Asserter.ValuesEqualAsync(
                    original,
                    dupe,
                    canceler,
                    failMessage + " Cloned data was not equal."
                )
                .ConfigureAwait(false);

            Options.Asserter.Is(
                original.GetType(),
                dupe.GetType(),
                failMessage
                    + $" Cloned type '{GenericConverter.ExpandName(dupe.GetType())}' mismatch."
            );

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

            if (Options.Faker.Supports(type) && !type.Inherits<IToolOptions>())
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
