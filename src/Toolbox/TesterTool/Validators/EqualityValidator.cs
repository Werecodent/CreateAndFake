using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Comparisons;
using CreateAndFake.Design.Types;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.TesterTool.Validators;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
internal sealed class EqualityValidator(TesterOptions options)
{
    /// <inheritdoc/>
    internal TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    public void VerifyEqualsMatchesHashCodes(Type type)
    {
        VerifyValueEquality(type, false);
    }

    public void VerifyValueEquality(Type type)
    {
        VerifyValueEquality(type, true);
    }

    private void VerifyValueEquality(Type type, bool mustIncludeAllValues)
    {
        ArgumentGuard.ThrowIfNull(type);

        TypeDescriber describer = TypeDescriber.For(type);

        if (describer.Inherits<IValuerEquatable>())
        {
            IValuerEquatable x = (IValuerEquatable)Options.Randomizer.Create(type);
            IValuerEquatable y = Options.Duplicator.Copy(x);
            VerifyValueEquality(
                describer,
                x,
                y,
                y => x.ValuesEqual(y, Options.Valuer),
                x => y.ValuesEqual(x, Options.Valuer),
                x.GetHashCode,
                y.GetHashCode,
                mustIncludeAllValues
            );
        }
        else if (describer.Inherits<IValueEquatable>())
        {
            IValueEquatable x = (IValueEquatable)Options.Randomizer.Create(type);
            IValueEquatable y = Options.Duplicator.Copy(x);
            VerifyValueEquality(
                describer,
                x,
                y,
                x.ValuesEqual,
                y.ValuesEqual,
                x.GetHashCode,
                y.GetHashCode,
                mustIncludeAllValues
            );
        }
        else
        {
            object x = Options.Randomizer.Create(type);
            object y = Options.Duplicator.Copy(x);
            VerifyValueEquality(
                describer,
                x,
                y,
                x.Equals,
                y.Equals,
                x.GetHashCode,
                y.GetHashCode,
                mustIncludeAllValues
            );
        }
    }

    private void VerifyValueEquality(
        TypeDescriber describer,
        object x,
        object y,
        Func<object, bool> xEquals,
        Func<object, bool> yEquals,
        Func<int> xGetHash,
        Func<int> yGetHash,
        bool mustIncludeAllValues
    )
    {
        Options.Asserter.Is(
            true,
            xEquals(y) && yEquals(x),
            $"Equality for type '{GenericConverter.ExpandName(describer.SupportedType)}' failed with clone."
        );
        Options.Asserter.Is(
            xGetHash(),
            yGetHash(),
            $"Hash codes for type '{GenericConverter.ExpandName(describer.SupportedType)}' failed with clone."
        );

        void SwitchValue(PropertyInfo prop, object? originalValue, object? newValue)
        {
            string originalAsText = $"{originalValue ?? "default"}";
            string newAsText = $"{newValue ?? "default"}";

            prop.SetValue(x, newValue);
            if (mustIncludeAllValues)
            {
                Options.Asserter.Is(
                    false,
                    xEquals(y) || yEquals(x),
                    $"Equality for type '{GenericConverter.ExpandName(describer.SupportedType)}' failed to"
                        + $" differ when modifying '{prop.Name}' from '{originalAsText}' -> '{newAsText}'."
                );
            }
            else
            {
                Options.Asserter.Is(
                    xEquals(y),
                    yEquals(x),
                    $"Equality result for type '{GenericConverter.ExpandName(describer.SupportedType)}'"
                        + $" mismatched when modifying '{prop.Name}' from '{originalAsText}' -> '{newAsText}'."
                );
            }

            prop.SetValue(y, newValue);
            Options.Asserter.Is(
                true,
                xEquals(y) && yEquals(x),
                $"Equality for type '{GenericConverter.ExpandName(describer.SupportedType)}' failed to"
                    + $" equal when modifying '{prop.Name}' from '{originalAsText}' -> '{newAsText}'."
            );

            Options.Asserter.Is(
                xGetHash(),
                yGetHash(),
                $"Hash codes for type '{GenericConverter.ExpandName(describer.SupportedType)}' failed to"
                    + $" equal when modifying '{prop.Name}' from '{originalAsText}' -> '{newAsText}'."
            );
        }

        foreach (
            PropertyInfo prop in TypeDescriber.For(describer.SupportedType).Properties.SetAndGetable
        )
        {
            object? originalValue = prop.GetValue(x);
            object newValue = Options.Mutator.Variant(prop.PropertyType, originalValue);

            SwitchValue(prop, originalValue, null);
            SwitchValue(prop, null, newValue);
        }
    }
}
