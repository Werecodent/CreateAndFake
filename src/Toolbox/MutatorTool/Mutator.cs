using System.Collections;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;

namespace CreateAndFake.MutatorTool;

/// <inheritdoc cref="IMutator"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Mutator(MutatorOptions options) : IMutator
{
    /// <inheritdoc/>
    public MutatorOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public T Variant<T>(T instance, params IEnumerable<T?>? extraInstances)
    {
        return (T)Variant(typeof(T), instance, extraInstances?.Cast<object>());
    }

    /// <inheritdoc/>
    public object Variant(Type type, object? instance, params IEnumerable<object?>? extraInstances)
    {
        object?[] values = [.. (extraInstances ?? []).Prepend(instance)];
        try
        {
            return Options
                .VariantAttempts.StallUntil(
                    $"Create variant of type '{type}'",
                    () => Options.Randomizer.Create(type),
                    result =>
                    {
                        if (
                            values.All(o =>
                                ArgumentGuard.IsAsynchronous(o)
                                || !Options.Valuer.Equals(
                                    result,
                                    o,
                                    opt => opt with { SkipAsyncValues = true }
                                )
                            )
                        )
                        {
                            return true;
                        }
                        else
                        {
                            Disposer.Cleanup(result);
                            return false;
                        }
                    }
                )
                .Last();
        }
        catch (TimeoutException e)
        {
            throw new ToolException($"Could not create different instance of type '{type}'.", e);
        }
    }

    /// <inheritdoc/>
    public T Unique<T>(T instance, params IEnumerable<T?>? extraInstances)
    {
        return (T)Unique(typeof(T), instance, extraInstances);
    }

    /// <inheritdoc/>
    public object Unique(Type type, object? instance, params IEnumerable<object?>? extraInstances)
    {
        IContentMap[] maps =
        [
            .. (extraInstances ?? [])
                .Prepend(instance)
                .Where(e => e != null)
                .Select(e => Options.Extractor.Extract(e)),
        ];

        try
        {
            return Options
                .VariantAttempts.StallUntil(
                    $"Create unique of type '{type}'",
                    () => Options.Randomizer.Create(type),
                    result =>
                    {
                        if (!Options.Extractor.Extract(result).HasSharedContent(maps))
                        {
                            return true;
                        }
                        else
                        {
                            Disposer.Cleanup(result);
                            return false;
                        }
                    }
                )
                .Last();
        }
        catch (TimeoutException e)
        {
            throw new ToolException($"Could not create unique instance of type '{type}'.", e);
        }
    }

    /// <inheritdoc/>
    public bool Modify(object? instance, MutatorMod? optionConfiguration = null)
    {
        MutatorOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        if (instance == null)
        {
            return false;
        }

        if (instance.GetType().Inherits<IEnumerable>())
        {
            return false;
        }

        bool modified = false;

        Type type = instance.GetType();
        foreach (FieldInfo field in TypeDescriber.GetAllFields(type, BindingFlags.Public))
        {
            try
            {
                object? smartData =
                    (field.FieldType == typeof(string))
                        ? new DataRandom(localOptions.Gen).Find(field.Name)
                        : null;

                field.SetValue(
                    instance,
                    smartData ?? Variant(field.FieldType, field.GetValue(instance))
                );
                modified = true;
            }
            catch (Exception)
            {
                // Failed to modify.
            }
        }
        foreach (
            PropertyInfo property in TypeDescriber
                .GetAllProperties(type, BindingFlags.Public)
                .Where(p => p.CanWrite && p.CanRead)
                .Where(p => p.GetGetMethod() != null)
                .Where(p => p.GetSetMethod() != null)
        )
        {
            try
            {
                object? smartData =
                    (property.PropertyType == typeof(string))
                        ? new DataRandom(localOptions.Gen).Find(property.Name)
                        : null;

                property.SetValue(
                    instance,
                    smartData ?? Variant(property.PropertyType, property.GetValue(instance))
                );
                modified = true;
            }
            catch (Exception)
            {
                // Failed to modify.
            }
        }

        return modified;
    }

    /// <inheritdoc/>
    public IMutator WithOptions(MutatorMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Mutator(optionConfiguration.Invoke(Options));
    }
}
