using System.Reflection;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Toolbox.MutatorTool;

/// <inheritdoc cref="IMutator"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public sealed class Mutator(MutatorOptions options) : IMutator
{
    /// <inheritdoc/>
    public MutatorOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public T Variant<T>(T instance, params IEnumerable<T?>? extraInstances)
    {
        return (T)Variant(typeof(T), instance, extraInstances?.Cast<object>());
    }

    /// <inheritdoc/>
    public object Variant(Type type, object? instance, params IEnumerable<object?>? extraInstances)
    {
        object?[] values = (extraInstances ?? []).Prepend(instance).ToArray();
        try
        {
            return Options.Limiter.StallUntil(
                $"Create variant of type '{type}'",
                () => Options.Randomizer.Create(type),
                result =>
                {
                    if (values.All(o => !Options.Valuer.Equals(result, o)))
                    {
                        return true;
                    }
                    else
                    {
                        Disposer.Cleanup(result);
                        return false;
                    }
                }).Last();
        }
        catch (AggregateException e)
        {
            throw new TimeoutException($"Could not create different instance of type '{type}'.", e);
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
        ContentMap[] maps = (extraInstances ?? [])
            .Prepend(instance)
            .Where(e => e != null)
            .Select(ContentMap.Extract)
            .ToArray();

        try
        {
            return Options.Limiter.StallUntil(
                $"Create unique of type '{type}'",
                () => Options.Randomizer.Create(type),
                result =>
                {
                    if (!ContentMap.Extract(result).HasSharedContent(Options.Valuer, maps))
                    {
                        return true;
                    }
                    else
                    {
                        Disposer.Cleanup(result);
                        return false;
                    }
                }).Last();
        }
        catch (AggregateException e)
        {
            throw new TimeoutException($"Could not create unique instance of type '{type}'.", e);
        }
    }

    /// <inheritdoc/>
    public bool Modify(object? instance, MutatorMod? optionConfiguration = null)
    {
        if (instance == null)
        {
            return false;
        }

        bool modified = false;

        Type type = instance.GetType();
        foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
            field.SetValue(instance, Variant(field.FieldType, field.GetValue(instance)));
            modified = true;
        }
        foreach (PropertyInfo property in type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanWrite && p.CanRead)
            .Where(p => p.GetGetMethod() != null)
            .Where(p => p.GetSetMethod() != null))
        {
            property.SetValue(instance, Variant(property.PropertyType, property.GetValue(instance)));
            modified = true;
        }

        return modified;
    }
}
