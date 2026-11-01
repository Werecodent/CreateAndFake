using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.TesterTool.Validators;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
internal sealed class EqualityValidator(TesterOptions options)
{
    /// <inheritdoc cref="Tester.Options"/>
    internal TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc cref="ITester.VerifyEqualsMatchesHashCodesAsync(Type,CancellationToken,TesterMod)"/>
    public Task VerifyEqualsMatchesHashCodesAsync(Type type, CancellationToken canceler)
    {
        return VerifyValueEqualityAsync(type, false, canceler);
    }

    /// <inheritdoc cref="ITester.VerifyValueEqualityAsync(Type,CancellationToken,TesterMod)"/>
    public Task VerifyValueEqualityAsync(Type type, CancellationToken canceler)
    {
        return VerifyValueEqualityAsync(type, true, canceler);
    }

    private Task VerifyValueEqualityAsync(
        Type type,
        bool mustIncludeAllValues,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(type);

        TypeDescriber describer = TypeDescriber.For(type);

        if (describer.Inherits<IValuerAsyncComparable>())
        {
            IValuerAsyncComparable x = (IValuerAsyncComparable)Options.Randomizer.Create(type);
            IValuerAsyncComparable y = Options.Duplicator.Copy(x);
            return VerifyValueEqualityAsync(
                describer,
                x,
                y,
                z =>
                    AsyncSeriesHelper.HasNoneAsync(
                        x.CompareAsync(z, Options.Valuer, canceler),
                        canceler
                    ),
                z =>
                    AsyncSeriesHelper.HasNoneAsync(
                        y.CompareAsync(z, Options.Valuer, canceler),
                        canceler
                    ),
                () => x.GetValueHashAsync(Options.Valuer, canceler),
                () => y.GetValueHashAsync(Options.Valuer, canceler),
                mustIncludeAllValues,
                canceler
            );
        }
        else if (describer.Inherits<IValuerEquatable>())
        {
            IValuerEquatable x = (IValuerEquatable)Options.Randomizer.Create(type);
            IValuerEquatable y = Options.Duplicator.Copy(x);
            return VerifyValueEqualityAsync(
                describer,
                x,
                y,
                z => Task.FromResult(x.ValuesEqual(z, Options.Valuer)),
                z => Task.FromResult(y.ValuesEqual(z, Options.Valuer)),
                () => Task.FromResult(x.GetValueHash(Options.Valuer)),
                () => Task.FromResult(y.GetValueHash(Options.Valuer)),
                mustIncludeAllValues,
                canceler
            );
        }
        else if (describer.Inherits<IValueEquatable>())
        {
            IValueEquatable x = (IValueEquatable)Options.Randomizer.Create(type);
            IValueEquatable y = Options.Duplicator.Copy(x);
            return VerifyValueEqualityAsync(
                describer,
                x,
                y,
                z => Task.FromResult(x.ValuesEqual(z)),
                z => Task.FromResult(y.ValuesEqual(z)),
                () => Task.FromResult(x.GetValueHash()),
                () => Task.FromResult(y.GetValueHash()),
                mustIncludeAllValues,
                canceler
            );
        }
        else
        {
            object x = Options.Randomizer.Create(type);
            object y = Options.Duplicator.Copy(x);
            return VerifyValueEqualityAsync(
                describer,
                x,
                y,
                z => Task.FromResult(x.Equals(z)),
                z => Task.FromResult(y.Equals(z)),
                () => Task.FromResult(x.GetHashCode()),
                () => Task.FromResult(y.GetHashCode()),
                mustIncludeAllValues,
                canceler
            );
        }
    }

    private async Task VerifyValueEqualityAsync(
        TypeDescriber describer,
        object x,
        object y,
        Func<object, Task<bool>> xEquals,
        Func<object, Task<bool>> yEquals,
        Func<Task<int>> xGetHash,
        Func<Task<int>> yGetHash,
        bool mustIncludeAllValues,
        CancellationToken canceler
    )
    {
        Options.Asserter.Is(
            true,
            await xEquals(y).ConfigureAwait(false) && await yEquals(x).ConfigureAwait(false),
            $"Equality for type '{GenericConverter.ExpandName(describer.SupportedType)}' failed with clone."
        );
        Options.Asserter.Is(
            await xGetHash().ConfigureAwait(false),
            await yGetHash().ConfigureAwait(false),
            $"Hash codes for type '{GenericConverter.ExpandName(describer.SupportedType)}' failed with clone."
        );

        async Task switchValueAsync(PropertyInfo prop, object? originalValue, object? newValue)
        {
            string originalAsText = $"{originalValue ?? "default"}";
            string newAsText = $"{newValue ?? "default"}";

            prop.SetValue(x, newValue);
            if (mustIncludeAllValues)
            {
                Options.Asserter.Is(
                    false,
                    await xEquals(y).ConfigureAwait(false)
                        || await yEquals(x).ConfigureAwait(false),
                    $"Equality for type '{GenericConverter.ExpandName(describer.SupportedType)}' failed to"
                        + $" differ when modifying '{prop.Name}' from '{originalAsText}' -> '{newAsText}'."
                );
            }
            else
            {
                Options.Asserter.Is(
                    await xEquals(y).ConfigureAwait(false),
                    await yEquals(x).ConfigureAwait(false),
                    $"Equality result for type '{GenericConverter.ExpandName(describer.SupportedType)}'"
                        + $" mismatched when modifying '{prop.Name}' from '{originalAsText}' -> '{newAsText}'."
                );
            }

            prop.SetValue(y, newValue);
            Options.Asserter.Is(
                true,
                await xEquals(y).ConfigureAwait(false) && await yEquals(x).ConfigureAwait(false),
                $"Equality for type '{GenericConverter.ExpandName(describer.SupportedType)}' failed to"
                    + $" equal when modifying '{prop.Name}' from '{originalAsText}' -> '{newAsText}'."
            );

            Options.Asserter.Is(
                await xGetHash().ConfigureAwait(false),
                await yGetHash().ConfigureAwait(false),
                $"Hash codes for type '{GenericConverter.ExpandName(describer.SupportedType)}' failed to"
                    + $" equal when modifying '{prop.Name}' from '{originalAsText}' -> '{newAsText}'."
            );
        }

        foreach (PropertyInfo prop in describer.Properties.SetAndGetable)
        {
            object? originalValue = prop.GetValue(x);
            object newValue = await Options
                .Mutator.VariantAsync(prop.PropertyType, originalValue, canceler)
                .ConfigureAwait(false);

            await switchValueAsync(prop, originalValue, null).ConfigureAwait(false);
            await switchValueAsync(prop, null, newValue).ConfigureAwait(false);
        }
    }
}
