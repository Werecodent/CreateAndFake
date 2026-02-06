using System.Collections;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public sealed class ObjectMutateHint : MutateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.ObjectHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes { get; } = [typeof(object)];

    /// <inheritdoc/>
    protected override bool Supports(object instance)
    {
        return instance is not IEnumerable;
    }

    /// <inheritdoc/>
    protected override bool Modify(object instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(instance, chainer);

        Type type = instance.GetType();
        DataRandom data = chainer.Options.Gen.NextData();
        bool modified = false;

        foreach (
            FieldInfo field in TypeDescriber
                .GetAllFields(type, true)
                .OrderBy(_ => chainer.Options.Gen.Next<int>())
        )
        {
            if (modified && chainer.Options.Gen.Next<bool>())
            {
                break;
            }

            object? smartData = (field.FieldType == typeof(string)) ? data.Find(field.Name) : null;
            try
            {
                field.SetValue(
                    instance,
                    smartData ?? chainer.Variant(field.FieldType, field.GetValue(instance))
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
                .GetAllProperties(type, true)
                .Where(p => p.CanWrite && p.CanRead)
                .Where(p => p.GetGetMethod() != null)
                .Where(p => p.GetSetMethod() != null)
                .OrderBy(_ => chainer.Options.Gen.Next<int>())
        )
        {
            if (modified && chainer.Options.Gen.Next<bool>())
            {
                break;
            }

            object? smartData =
                (property.PropertyType == typeof(string)) ? data.Find(property.Name) : null;
            try
            {
                property.SetValue(
                    instance,
                    smartData ?? chainer.Variant(property.PropertyType, property.GetValue(instance))
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
}
