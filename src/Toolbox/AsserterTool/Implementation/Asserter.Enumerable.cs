using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterEnumerable
{
    /// <inheritdoc/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public virtual void Fail(IEnumerable? collection, string? details = null)
    {
        Fail(collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public virtual void Fail(
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        HandleFail("Test failed.", details, optionConfiguration, details);
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    private void HandleFail(
        string message,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                message,
                details,
                localOptions.Gen.InitialSeed,
                (string?)null
            );
        }

        StringBuilder contents = new();

        int i = 0;
        foreach (object item in collection)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, localOptions.Valuer.Options.IterationLimit);
            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        throw new AssertException(
            message,
            details,
            localOptions.Gen.InitialSeed,
            contents.ToString()
        );
    }

    /// <inheritdoc/>
    public void Debug(IEnumerable? collection, string? details = null)
    {
        Debug(collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public void Debug(
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        HandleFail(
            $"{nameof(AsserterOptions.DebugAssertsFail)} set to '{true}'.",
            details,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual void IsEmpty(IEnumerable? collection, string? details = null)
    {
        IsEmpty(collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void IsEmpty(
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        HasCount(0, collection, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual void IsNotEmpty(IEnumerable? collection, string? details = null)
    {
        IsNotEmpty(collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void IsNotEmpty(
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                "Expected collection with elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        IEnumerator gen = collection.GetEnumerator();
        try
        {
            if (!gen.MoveNext())
            {
                throw new AssertException(
                    "Expected collection with elements, but was empty.",
                    details,
                    localOptions.Gen.InitialSeed
                );
            }
        }
        finally
        {
            Disposer.Cleanup(gen);
        }
    }

    /// <inheritdoc/>
    public virtual void HasCount(int count, IEnumerable? collection, string? details = null)
    {
        HasCount(count, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void HasCount(
        int count,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                $"Expected collection of '{count}' elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        StringBuilder contents = new();

        int i = 0;
        foreach (object item in collection)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, localOptions.Valuer.Options.IterationLimit);
            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (i != count)
        {
            throw new AssertException(
                $"Expected collection of '{count}' elements, but was '{i}'.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual void HasCountLessThan(int count, IEnumerable? collection, string? details = null)
    {
        HasCountLessThan(count, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void HasCountLessThan(
        int count,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                $"Expected collection of '< {count}' elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        StringBuilder contents = new();

        int i = 0;
        foreach (object item in collection)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, localOptions.Valuer.Options.IterationLimit);
            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (i >= count)
        {
            throw new AssertException(
                $"Expected collection of '< {count}' elements, but was '{i}'.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual void HasCountLessOrExactly(
        int count,
        IEnumerable? collection,
        string? details = null
    )
    {
        HasCountLessOrExactly(count, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void HasCountLessOrExactly(
        int count,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                $"Expected collection of '<= {count}' elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        StringBuilder contents = new();

        int i = 0;
        foreach (object item in collection)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, localOptions.Valuer.Options.IterationLimit);
            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (i > count)
        {
            throw new AssertException(
                $"Expected collection of '<= {count}' elements, but was '{i}'.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual void HasCountMoreThan(int count, IEnumerable? collection, string? details = null)
    {
        HasCountMoreThan(count, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void HasCountMoreThan(
        int count,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                $"Expected collection of '> {count}' elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        StringBuilder contents = new();

        int i = 0;
        foreach (object item in collection)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, localOptions.Valuer.Options.IterationLimit);
            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (i <= count)
        {
            throw new AssertException(
                $"Expected collection of '> {count}' elements, but was '{i}'.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual void HasCountMoreOrExactly(
        int count,
        IEnumerable? collection,
        string? details = null
    )
    {
        HasCountMoreOrExactly(count, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void HasCountMoreOrExactly(
        int count,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                $"Expected collection of '>= {count}' elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        StringBuilder contents = new();

        int i = 0;
        foreach (object item in collection)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, localOptions.Valuer.Options.IterationLimit);
            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (i < count)
        {
            throw new AssertException(
                $"Expected collection of '>= {count}' elements, but was '{i}'.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual void Contains(object? content, IEnumerable? collection, string? details)
    {
        Contains(content, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void Contains(
        object? content,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}', but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        bool found = false;
        StringBuilder contents = new();

        int i = 0;
        foreach (object item in collection)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, localOptions.Valuer.Options.IterationLimit);

            found |= localOptions.Valuer.Equals(content, item);

            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (!found)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}' but didn't.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task ContainsAsync(
        object? content,
        IEnumerable? collection,
        CancellationToken canceler,
        string? details
    )
    {
        return ContainsAsync(content, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ContainsAsync(
        object? content,
        IEnumerable? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}', but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        bool found = false;
        StringBuilder contents = new();

        int i = 0;
        foreach (object item in collection)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, localOptions.Valuer.Options.IterationLimit);

            found |= await localOptions
                .Valuer.EqualsAsync(content, item, canceler)
                .ConfigureAwait(false);

            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (!found)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}' but didn't.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual void ContainsNot(
        object? content,
        IEnumerable? collection,
        string? details = null
    )
    {
        ContainsNot(content, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ContainsNot(
        object? content,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            return;
        }

        bool notFound = true;
        StringBuilder contents = new();

        int i = 0;
        foreach (object item in collection)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, localOptions.Valuer.Options.IterationLimit);

            notFound &= !localOptions.Valuer.Equals(content, item);

            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (!notFound)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}' but didn't.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task ContainsNotAsync(
        object? content,
        IEnumerable? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ContainsNotAsync(content, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ContainsNotAsync(
        object? content,
        IEnumerable? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            return;
        }

        bool notFound = true;
        StringBuilder contents = new();

        int i = 0;
        foreach (object item in collection)
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, localOptions.Valuer.Options.IterationLimit);

            notFound &= !await localOptions
                .Valuer.EqualsAsync(content, item, canceler)
                .ConfigureAwait(false);

            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (!notFound)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}' but didn't.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }
}
