using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterComparable
{
    /// <inheritdoc/>
    public virtual void GreaterThan(IComparable? target, IComparable? value, string? details = null)
    {
        GreaterThan(target, value, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void GreaterThan(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        HandleMathCheck(
            () => value!.CompareTo(target) > 0,
            "greater than",
            target,
            value,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual void GreaterThanOrEqualTo(
        IComparable? target,
        IComparable? value,
        string? details = null
    )
    {
        GreaterThanOrEqualTo(target, value, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void GreaterThanOrEqualTo(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        HandleMathCheck(
            () => value!.CompareTo(target) >= 0,
            "greater than or equal to",
            target,
            value,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual void GreaterThanOrIs(
        IComparable? target,
        IComparable? value,
        string? details = null
    )
    {
        GreaterThanOrIs(target, value, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void GreaterThanOrIs(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (!localOptions.Valuer.Equals(value, target))
        {
            GreaterThanOrEqualTo(target, value, _ => localOptions, details);
        }
    }

    /// <inheritdoc/>
    public virtual void LessThan(IComparable? target, IComparable? value, string? details = null)
    {
        LessThan(target, value, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void LessThan(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        HandleMathCheck(
            () => value!.CompareTo(target) < 0,
            "less than",
            target,
            value,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual void LessThanOrEqualTo(
        IComparable? target,
        IComparable? value,
        string? details = null
    )
    {
        LessThanOrEqualTo(target, value, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void LessThanOrEqualTo(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        HandleMathCheck(
            () => value!.CompareTo(target) <= 0,
            "less than or equal to",
            target,
            value,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual void LessThanOrIs(
        IComparable? target,
        IComparable? value,
        string? details = null
    )
    {
        LessThanOrIs(target, value, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void LessThanOrIs(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (!localOptions.Valuer.Equals(value, target))
        {
            LessThanOrEqualTo(target, value, _ => localOptions, details);
        }
    }

    /// <inheritdoc/>
    public virtual void InRange(
        IComparable? min,
        IComparable? max,
        IComparable? value,
        string? details = null
    )
    {
        InRange(min, max, value, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void InRange(
        IComparable? min,
        IComparable? max,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (value == null)
        {
            throw new AssertException(
                $"Value was null and not in range [{min}, {max}].",
                details,
                localOptions.Gen.InitialSeed
            );
        }
        else if (min == null || max == null)
        {
            throw new AssertException(
                $"Min {min} or max {max} was null and not valid for math comparison check.",
                details,
                localOptions.Gen.InitialSeed,
                value.ToString()
            );
        }
        else if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
        {
            throw new AssertException(
                $"Value was not in range [{min}, {max}].",
                details,
                localOptions.Gen.InitialSeed,
                value.ToString()
            );
        }
    }

    /// <summary>Verifies <c>value</c> matches the <paramref name="math"/>.</summary>
    /// <param name="math">Math used to check the <c>value</c>.</param>
    /// <param name="description">Math description to use for error message.</param>
    /// <param name="target">Value to compare with.</param>
    /// <param name="value"></param>
    /// <param name="optionConfiguration"></param>
    /// <param name="details">Optional failure details to include.</param>
    /// <returns>Chainer to make additional assertions with.</returns>
    /// <exception cref="AssertException">If <c>value</c> does not match <paramref name="math"/>.</exception>
    private void HandleMathCheck(
        Func<bool> math,
        string description,
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (value == null)
        {
            throw new AssertException(
                $"Value was null and not {description} '{target}'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
        else if (target == null)
        {
            throw new AssertException(
                "Target was null and not valid for math comparison check.",
                details,
                localOptions.Gen.InitialSeed,
                value.ToString()
            );
        }
        else if (!math())
        {
            throw new AssertException(
                $"Value was not {description} '{target}'.",
                details,
                localOptions.Gen.InitialSeed,
                value.ToString()
            );
        }
    }
}
